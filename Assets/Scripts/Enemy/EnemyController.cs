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
    /// <summary>敵のステータス</summary>
    [SerializeField] EnemyStatus m_eStatus;
    /// <summary>起動させるCanonnの配列</summary>
    [SerializeField] GameObject[] m_canonnPrefabs = null;
    /// <summary> 敵の聴覚。後ろからでもこの距離まで近づいたら気づく </summary>
    [SerializeField] float m_yearDistance = 2f;
    /// <summary> プレイヤーの方向  </summary>
    Vector3 m_playerDir;
    /// <summary>巡回させるために必要なtransform</summary>
    [SerializeField] Transform[] m_routeObjects = null;
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

    EnemyEffects m_ef;
    Rigidbody m_rb;
    NavMeshAgent m_agent;
    EnemyLightController m_elc;
    EnemyAudioManager m_audio;
    public GameObject m_player;

    /// <summary>
    /// デバッグ用。
    /// </summary>
    public void ShowEnemyStatus()
    {
        Debug.Log($"ShowEnemyStatus():{m_eStatus}");
    }

    private void Start()
    {
        m_audio = GetComponentInChildren<EnemyAudioManager>();
        m_elc = GetComponentInChildren<EnemyLightController>();
        m_rb = GetComponent<Rigidbody>();
        m_agent = GetComponent<NavMeshAgent>();
        m_eStatus = EnemyStatus.Patrol;
        m_player = GameObject.FindGameObjectWithTag("Player");
        if (!m_player) Debug.LogError("プレイヤーが取得できていない");
        else Debug.Log($"プレイヤーを取得。{m_player.name}");
        m_ef = GetComponentInChildren<EnemyEffects>();
        this.transform.position = m_routeObjects[0].position;
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
        m_agent.SetDestination(m_routeObjects[m_routeIndex].position);
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

        if (isWall) Debug.Log($"CheckPlayer:{hit.collider.name}に当たっている。isWall = {isWall}");

        return isWall;
    }
    /// <summary>
    /// プレイヤーが一定の距離にいるなら立ち止まり、プレイヤーの方を見てtrueを返す
    /// </summary>
    public bool CheckDistance()
    {
        Debug.Log("CheckDistance()：CheckDistance()が呼ばれた");
        bool found = false;
        /*プレイヤーまでの距離*/
        float distance = Vector3.Distance(this.transform.position, m_player.transform.position);
        if (distance <= m_yearDistance)
        {
            found = true;
            Debug.Log($"CheckDistance()：プレイヤーが近くにいる！：{found}");
            m_agent.SetDestination(this.transform.position);
            this.transform.LookAt(m_player.transform.position);
            return found;
        }
        else
        {
            found = false;
            Debug.Log($"CheckDistance()：{found}");
            return found;
        }
    }
    /// <summary>
    /// プレイヤーを見つけたときに呼ばれ、エネミーのステイタスをプレイヤーを見つけた状態に変更する
    /// </summary>
    public void OnFoundPlayer()
    {
        Debug.Log("OnFoundPlayerByEye:プレイヤーを見つけた");
        m_ef.ActiveExclamationMark();
        UpdateCannonState(CanonStatus.Active);
        if (m_stayWhenFoundPlayer) m_eStatus = EnemyStatus.Stay;
        else m_eStatus = EnemyStatus.FoundPlayer;
    }
    /// <summary>
    /// プレイヤーを見失ったときに呼ばれ、エネミーのステイタスをSearchに変更する。
    /// </summary>
    public void OnLostPlayer()
    {
        Debug.Log("OnLostPlayer():プレイヤーを見失った");
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


    private void Update()
    {
        if (m_player)
        {
            m_playerDir = m_player.transform.position - this.transform.position;
            m_playerDir.y = 0;

            switch (m_eStatus)
            {
                /*プレイヤーを見つけていないときのステータス。指定された場所を巡回する*/
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
                /*プレイヤーを見つけたときのステータス。インスペクターでm_stayWhenFoundPlayerがtrueにされているとこちらになる。
                 　その場で立ち止まり、プレイヤーの方を向き続ける*/
                case EnemyStatus.Stay:
                    {
                        this.transform.LookAt(m_player.transform.position);
                        m_agent.SetDestination(this.transform.position);
                    }
                    break;
                /*プレイヤーを見つけたときのステータス。インスペクターでm_stayWhenFoundPlayerがfalseにされているとこちらになる
                  プレイヤーの方を向きながら、プレイヤーを追いかける。*/
                case EnemyStatus.FoundPlayer:
                    {
                        m_audio.PlaySound("EnemyFound");
                        m_elc.ChangeLightColorWhenFound();
                        this.transform.LookAt(m_player.transform.position);
                        m_agent.SetDestination(m_player.transform.position);
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
                        m_audio.PlaySound("EnemyLost");
                        m_elc.ChangeLightColorWhenLost();
                        /*プレイヤーを見失った時のステータス*/
                        m_timer += Time.deltaTime;
                        //Debug.Log($"ステータス：{m_eStatus}。タイマー：{m_timer}");
                        m_agent.SetDestination(this.transform.position);
                        if (m_timer >= m_roveTime)
                        {
                            m_timer = 0f;
                            m_ef.InActiveQuestionMark();
                            m_elc.ResetLightColor();
                            Debug.Log($"パトロールに切り替えます");
                            m_eStatus = EnemyStatus.Patrol;
                        }
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
