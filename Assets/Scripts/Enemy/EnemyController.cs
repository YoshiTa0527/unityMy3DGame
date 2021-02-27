using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// 敵。enemyEyeFieledにプレイヤーが侵入するか、一定距離まで近づくとプレイヤーを発見し、Canonを起動させる。
/// </summary>
public class EnemyController : MonoBehaviour
{
    /// <summary>敵のステータス</summary>
    [SerializeField] EnemyStatus m_eStatus;
    /// <summary>起動させるCanonnの配列</summary>
    [SerializeField] GameObject[] m_canonnPrefabs = null;
    /// <summary> プレイヤーの方向  </summary>
    Vector3 m_playerDir;
    /// <summary>巡回させるために必要なtransform</summary>
    [SerializeField] GameObject[] m_routeObjects = null;
    [SerializeField] GameObject m_routeObjectParent;
    [SerializeField] bool m_isOrderBy = true;
    /// <summary>巡回しない場合、この地点を順番に見る</summary>
    [SerializeField] GameObject m_lookAtPosParent;
    GameObject[] m_lookAtPosChirdlen;
    /// <summary>インデックス番号</summary>
    int m_routeIndex = 0;
    /// <summary>プレイヤーを見つけた際にstayするか追いかけるか</summary>
    [SerializeField] bool m_stayWhenFoundPlayer = false;
    /// <summary>プレイヤーを見失った後、しばらく動かない時間</summary>
    [SerializeField] float m_roveTime = 2f;
    /// <summary>タイマー</summary>
    float m_timer;
    /// <summary>プレイヤーと一定の距離を取る</summary>
    float m_keepDistance = 2f;
    [SerializeField] public float m_yearDistance = 4;

    //[SerializeField] UnityEvent m_OnFindPlayer;
    //[SerializeField] UnityEvent m_OnLostPlayer;

    EnemyEffects m_ef;
    Rigidbody m_rb;
    NavMeshAgent m_agent;
    EnemyLightController m_elc;
    EnemyAudioManager m_audio;
    public GameObject m_player;

    EnemyEyeField m_eef;
    AutomationDoorController[] m_adc;
    GameManager m_gm;
    AudioManager m_audioManager;


    /// <summary>
    /// デバッグ用。
    /// </summary>
    public void ShowEnemyStatus()
    {
        Debug.Log($"ShowEnemyStatus():{m_eStatus}");
    }

    public float GetYearDistance()
    {
        return this.m_yearDistance;
    }
    public int GetEnemyStatus()
    {
        return (int)this.m_eStatus;
    }

    private void Start()
    {
        /*ルートが設定されているなら、自動で設定する*/
        if (m_routeObjectParent != null)
        {
            GameObject[] routeObjectChirdren = new GameObject[m_routeObjectParent.transform.childCount];
            for (int i = 0; i < m_routeObjectParent.transform.childCount; i++)
            {
                routeObjectChirdren[i] = m_routeObjectParent.transform.GetChild(i).gameObject;
            }

            if (m_isOrderBy)
            {
                m_routeObjects = routeObjectChirdren.OrderBy(route => route.gameObject.name).ToArray();
            }
            else
            {
                m_routeObjects = routeObjectChirdren.OrderByDescending(route => route.gameObject.name).ToArray();
            }

            this.transform.position = m_routeObjects[0].transform.position;
        }

        if (m_lookAtPosParent)
        {
            m_yearDistance = 0.1f;
            m_lookAtPosChirdlen = new GameObject[m_lookAtPosParent.transform.childCount];
            for (int i = 0; i < m_lookAtPosParent.transform.childCount; i++)
            {
                m_lookAtPosChirdlen[i] = m_lookAtPosParent.transform.GetChild(i).gameObject;
            }
            m_lookAtPosChirdlen.ToList().ForEach(pos => Debug.Log(pos.gameObject.name));
            RotateEnemy();
        }

        m_audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        m_audio = GetComponentInChildren<EnemyAudioManager>();
        m_elc = GetComponentInChildren<EnemyLightController>();
        m_rb = GetComponent<Rigidbody>();
        m_agent = GetComponent<NavMeshAgent>();
        m_eStatus = EnemyStatus.Patrol;
        m_player = GameObject.FindGameObjectWithTag("Player");
        if (!m_player) Debug.LogError("プレイヤーが取得できていない");
        else Debug.Log($"プレイヤーを取得。{m_player.name}");
        m_gm = FindObjectOfType<GameManager>();

        m_adc = FindObjectsOfType<AutomationDoorController>();
        if (m_adc != null) Debug.Log($"{this.gameObject.name}::AutomationDoorControllerを取得::{m_adc.Length + 1}個");
        else Debug.LogError("AutomationDoorControllerを取得できていない");

        m_ef = GetComponentInChildren<EnemyEffects>();
        m_eef = GetComponentInChildren<EnemyEyeField>();
        BuildSequence();

        GoToNextPoint();
    }


    /// <summary>
    /// 敵の移動ルートを設定する
    /// </summary>
    void GoToNextPoint()
    {
        /*目的地が設定されていなかったら動かない*/
        if (m_routeObjects.Length == 0)
        {
            Debug.LogError("目的地を設定してください");
            m_agent.SetDestination(this.transform.position);
            return;
        }
        /*agentに目的地を設定*/
        m_agent.SetDestination(m_routeObjects[m_routeIndex].transform.position);
        /*配列内の次の位置を目的地に設定する。配列を超えると出発地点に戻る*/
        m_routeIndex = (m_routeIndex + 1) % m_routeObjects.Length;
    }

    /// <summary>
    /// 自分とプレイヤーとの間に障害物があるとtrueを返す
    /// </summary>
    public bool CheckObstacle(LayerMask obstacle)
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, m_playerDir);
        bool isWall = Physics.Raycast(ray, out hit, Vector3.Distance(this.transform.position, m_player.transform.position), obstacle);
        Debug.DrawRay(this.transform.position, m_playerDir, Color.blue);
        if (isWall) Debug.Log($"CheckObstacle::名前：{this.gameObject.name}。CheckPlayer:{hit.collider.name}に当たっている。isWall = {isWall}");
        else Debug.Log($"CheckObstacle::名前：{this.gameObject.name}。 壁以外に当たっている。isWall = {isWall}");
        return isWall;
    }

    /// <summary>
    /// プレイヤーを見つけたときに呼ばれ、エネミーのステイタスをプレイヤーを見つけた状態に変更する
    /// </summary>
    public void OnFoundPlayer()
    {
        Debug.Log("OnFoundPlayerByEye:プレイヤーを見つけた");
        m_audioManager.PlayFind(this.GetInstanceID());
        //m_OnFindPlayer?.Invoke();

        m_ef.ActiveExclamationMark();
        UpdateCannonState(CanonStatus.Active);
        m_eStatus = EnemyStatus.FoundPlayer;
    }
    /// <summary>
    /// プレイヤーを見失ったときに呼ばれ、エネミーのステイタスをSearchに変更する。
    /// </summary>
    public void OnSerchPlayer()
    {
        Debug.Log("OnLostPlayer():プレイヤーを見失った");
        m_audioManager.PlayDefault(this.GetInstanceID());
        //m_OnLostPlayer?.Invoke();
        m_gm.m_PlayerIsFound = false;
        m_ef.ActiveQuestionMark();
        UpdateCannonState(CanonStatus.NonActive);
        m_eStatus = EnemyStatus.Search;
    }

    void UpdateCannonState(CanonStatus newState)
    {
        if (m_canonnPrefabs != null)
        {
            m_canonnPrefabs.ToList().ForEach(g => g.GetComponentInChildren<CannonControler>().m_status = newState);
        }
    }


    void ChangeGameManagerBool(bool GMbool)
    {
        m_gm.m_PlayerIsFound = GMbool;
    }

    Sequence m_seq;

    void BuildSequence()
    {
        m_seq = DOTween.Sequence();
        m_seq.Append(this.transform.DORotate(new Vector3(0, 180, 0), 15)).SetRelative().Rewind();
    }

    float m_rotateTimer = 0;
    private void Update()
    {
        if (m_player)/*プレイヤーがいるなら行動する*/
        {
            m_playerDir = m_player.transform.position - this.transform.position;
            m_playerDir.y = 0;

            switch (m_eStatus)
            {
                /*プレイヤーを見つけていないときのステータス。指定された場所を巡回する*/
                case EnemyStatus.Patrol:
                    {
                        m_eef.OpenSearchArea();
                        /*目的地が設定されていなかったら、90度ずつ回転する*/
                        if (m_routeObjects.Length == 0)
                        {
                            Debug.Log($"{this.gameObject.name}目的地が設定されていない");
                            m_rotateTimer += Time.deltaTime;
                            if (m_rotateTimer >= 5f)
                            {
                                m_rotateTimer = 0;
                                RotateEnemy();

                            }

                        }
                        else
                        {
                            //Debug.Log($"巡回中。目的地までの距離：{m_agent.remainingDistance}");
                            if (!m_agent.pathPending && m_agent.remainingDistance < 1f)
                            {
                                GoToNextPoint();
                            }
                            else
                            {
                                Debug.Log($"agent.pathPending ：{m_agent.pathPending }");
                            }
                        }
                    }
                    break;
                /*プレイヤーを見つけたときのステータス。プレイヤーの方を向きながら、プレイヤーを追いかける。*/
                case EnemyStatus.FoundPlayer:
                    {
                        m_gm.m_PlayerIsFound = true;
                        m_elc.ChangeLightColorWhenFound();
                        this.transform.LookAt(m_player.transform.position);
                        if (m_stayWhenFoundPlayer)
                        {
                            m_agent.SetDestination(this.transform.position);
                        }
                        else
                        {
                            m_agent.SetDestination(m_player.transform.position);
                        }

                        if (!m_agent.pathPending && m_agent.remainingDistance <= m_keepDistance)
                        {
                            Debug.Log($" EnemyStatus.FoundPlayer：近づきすぎ！プレイヤーとの距離：{m_agent.remainingDistance}　設定された間隔：{m_keepDistance}");
                            m_agent.SetDestination(this.transform.position);
                        }
                        else
                        {
                            Debug.Log($" EnemyStatus.FoundPlayer：追いかけます。プレイヤーとの距離：{m_agent.remainingDistance}　設定された間隔：{m_keepDistance}");
                            m_agent.SetDestination(m_player.transform.position);
                        }

                    }
                    break;

                /*プレイヤーを見失った直後に遷移するステータス。一定時間その場で立ち止まり、その後Patrolステータスに移動する*/
                case EnemyStatus.Search:
                    {
                        //m_eef.CloseSearchArea();

                        StartCoroutine(m_eef.CloseSearchAreaCorutine());

                        //m_audio.PlaySound("EnemyLost");
                        m_elc.ChangeLightColorWhenLost();
                        m_agent.SetDestination(this.transform.position);
                        StartCoroutine("SearchStatusMove");
                    }
                    break;
            }
        }
        else /*プレイヤーがいないなら敵オブジェクトを消す*/
        { Debug.LogError($"EnemyController Update()：プレイヤーが居ない{m_player.name}"); }
    }

    /// <summary>
    /// サーチ状態の時の挙動のコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator SearchStatusMove()
    {
        Debug.Log("SearchStatusMove");
        yield return new WaitForSeconds(m_roveTime);
        m_ef.InActiveQuestionMark();
        m_elc.ResetLightColor();
        yield return new WaitForSeconds(2f);
        m_eStatus = EnemyStatus.Patrol;
    }


    int m_lookAtIndex;

    /// <summary>
    /// 敵を回転させる
    /// </summary>
    /// <returns></returns>
    void RotateEnemy()
    {
        Debug.Log($"RotateEnemy");

        /*設定*/
        this.transform.LookAt(m_lookAtPosChirdlen[m_lookAtIndex].transform.position);

        /*配列内の次の位置を設定する。配列を超えると出発地点に戻る*/
        m_lookAtIndex = (m_lookAtIndex + 1) % m_lookAtPosChirdlen.Length;



    }

}

enum EnemyStatus
{
    Patrol = 1,
    FoundPlayer = 2,
    Search = 3,
}
