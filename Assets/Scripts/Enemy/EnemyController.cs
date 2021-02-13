using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵。enemyEyeFieledにプレイヤーが侵入するか、一定距離まで近づくとプレイヤーを発見し、Canonを起動させる。
/// </summary>
public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyStatus m_eStatus;

    GameObject m_player;
    [SerializeField] bool m_foundPlayer = false;
    [SerializeField] float m_movingSpeed = 5f;
    /// <summary>一定の距離まで近づいたらストップ</summary>
    [SerializeField] float m_keepDistance = 1f;
    /// <summary>起動させるCanonnの配列</summary>
    [SerializeField] GameObject[] m_canonnPrefabs = null;
    /// <summary> 敵の聴覚。後ろからでもこの距離まで近づいたら気づく </summary>
    [SerializeField] float m_yearDistance = 2f;
    /// <summary> プレイヤーの方向  </summary>
    public Vector3 m_playersDirection { get; set; }
    Vector3 m_pDir;
    /// <summary>巡回させるために必要なtransform</summary>
    [SerializeField] Transform[] m_routeObjects = null;
    int m_routeIndex = 0;
    /// <summary>敵の目的地</summary>
    Vector3 m_destination;
    /// <summary>最後に着いた目的地</summary>
    Vector3 m_currentDestination;
    /// <summary>プレイヤーを見つけた際にstayするか追いかけるか</summary>
    [SerializeField] bool m_stayWhenFoundPlayer = false;

    public float m_originPoint;
    [SerializeField] LayerMask m_obstacle;
    /// <summary>プレイヤーを見失った後、しばらく動かない時間</summary>
    [SerializeField] float m_roveTime = 2f;
    /// <summary>タイマー</summary>
    float m_timer;

    EnemyEffects m_ef;
    Rigidbody m_rb;
    NavMeshAgent m_agent;
    Animator m_anim;
    /// <summary>
    /// デバッグ用
    /// </summary>
    public void ShowEnemyStatus()
    {
        Debug.Log($"ShowEnemyStatus():{m_eStatus}");
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_agent = GetComponent<NavMeshAgent>();
        m_eStatus = EnemyStatus.Patrol;
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_ef = GetComponentInChildren<EnemyEffects>();
        m_anim = GetComponent<Animator>();
        GoToNextPoint();
    }

    /// <summary>
    /// 敵の移動ルートを設定する
    /// </summary>
    void GoToNextPoint()
    {
        /*目的地が設定されていなかったら返す*/
        if (m_routeObjects.Length == 0)
        {
            Debug.LogError("目的地を設定してください");
            m_agent.SetDestination(this.transform.position);
            return;
        }
        /*agentに目的地を設定*/
        m_agent.SetDestination(m_routeObjects[m_routeIndex].position);
        //m_agent.destination = m_routeObjects[m_routeIndex].position;
        /*配列内の次の位置を目的地に設定する。配列を超えると出発地点に戻る*/
        m_routeIndex = (m_routeIndex + 1) % m_routeObjects.Length;
    }
    public void LookAtPlayer()
    {
        this.transform.LookAt(m_player.transform.position);
    }
    /// <summary>
    /// 自分とプレイヤーとの間に障害物があるとtrueを返す
    /// </summary>
    public bool CheckObstacle(LayerMask obstacle)
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, m_pDir);
        bool isWall = Physics.Raycast(ray, out hit, Vector3.Distance(this.transform.position, m_pDir), obstacle);
        Debug.DrawRay(this.transform.position, m_pDir, Color.red);

        if (isWall) Debug.Log($"CheckPlayer:{hit.collider.name}に当たっている。isWall = {isWall}");

        return isWall;
    }
    /// <summary>
    /// プレイヤーが一定の距離まで近づいてくるとプレイヤーの方向を見る
    /// </summary>
    public bool FindPlayerByYear()
    {
        Debug.Log("OnFoundPlayerByYear：プレイヤーを見つけた");
        bool found = false;
        /*プレイヤーまでの距離*/
        float distance = Vector3.Distance(this.transform.position, m_playersDirection);
        if (distance <= m_yearDistance)
        {
            found = true;
            Debug.Log($"OnFoundPlayerByYear：{found}");

            return found;
        }
        else { found = false; ; Debug.Log($"OnFoundPlayerByYear：{found}"); return false; }
    }
    /// <summary>
    /// プレイヤーを見つけたときに呼び、エネミーのステイタスを変更する
    /// </summary>
    public void OnFoundPlayerByEye()
    {
        Debug.Log("OnFoundPlayerByEye:プレイヤーを見つけた");
        m_ef.ActiveExclamationMark();
        UpdateCannonState(CanonStatus.Active);
        if (m_stayWhenFoundPlayer) m_eStatus = EnemyStatus.Stay;
        else m_eStatus = EnemyStatus.FoundPlayer;
    }

    public void OnLostPlayer()
    {
        Debug.Log("OnLostPlayer():プレイヤーを見失った");
        m_ef.InActiveExxlamationMark();
        UpdateCannonState(CanonStatus.NonActive);
        m_eStatus = EnemyStatus.Search;
        // m_eStatus = EnemyStatus.Patrol;
    }

    void UpdateCannonState(CanonStatus newState)
    {
        if (m_canonnPrefabs != null)
        {
            m_canonnPrefabs.ToList().ForEach(g => g.GetComponentInChildren<CannonControler>().m_status = newState);
        }
    }


    private void Update()
    {
        if (m_player)
        {
            m_playersDirection = m_player.transform.position - this.transform.position;
            m_pDir = m_player.transform.position - this.transform.position;
            m_pDir.y = 0;
            Vector3 lookAtPos = m_player.transform.position + Vector3.up;

            switch (m_eStatus)
            {
                case EnemyStatus.Patrol:
                    {
                        //Debug.Log($"巡回中。目的地までの距離：{m_agent.remainingDistance}");
                        if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
                        {
                            GoToNextPoint();
                        }
                        else
                        {
                            Debug.Log($"agent.pathPending ：{m_agent.pathPending }");
                        }
                    }
                    break;

                case EnemyStatus.FoundPlayer:
                    {
                        this.transform.LookAt(lookAtPos);
                        m_agent.SetDestination(m_pDir);
                    }
                    break;

                case EnemyStatus.Search:
                    {
                        m_timer += Time.deltaTime;
                        //Debug.Log($"ステータス：{m_eStatus}。タイマー：{m_timer}");
                        m_ef.ActiveQuestionMark();
                        m_anim.Play("EnemyRoving");
                        m_agent.SetDestination(this.transform.position);
                        if (m_timer >= m_roveTime)
                        {
                            Debug.Log("ifの中に到達");
                            m_timer = 0f;
                            m_ef.InActiveQuestionMark();

                            m_eStatus = EnemyStatus.Patrol;
                        }
                    }
                    break;

                case EnemyStatus.Stay:
                    {
                        this.transform.LookAt(lookAtPos);
                        m_agent.SetDestination(this.transform.position);
                    }
                    break;
            }
        }

    }


}

enum EnemyStatus
{
    Patrol,
    FoundPlayer,
    Stay,
    Search,
}
