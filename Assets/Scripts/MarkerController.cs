using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MarkerController : MonoBehaviour
{
    /// <summary>Ray が何にも当たらなかった時、Scene に表示する Ray の長さ</summary>
    [SerializeField] float m_debugRayLength = 100f;
    /// <summary>Ray が何かに当たった時に Scene に表示する Ray の色</summary>
    [SerializeField] Color m_debugRayColorOnHit = Color.red;
    /// <summary>ここに GameObject を設定すると、飛ばした Ray が何かに当たった時にそこに m_marker オブジェクトを移動する</summary>
    [SerializeField] GameObject m_marker;
    /// <summary>飛ばした Ray が当たった座標に m_marker を移動する際、Ray が当たった座標からどれくらいずらした場所に移動するかを設定する</summary>
    [SerializeField] Vector3 m_markerOffset = Vector3.up * 0.01f;
    /// <summary>マーカーを削除する時間</summary>
    [SerializeField] float m_deleteTime = 2f;
    /// <summary>マーカーを隠す場所</summary>
    [SerializeField] Transform m_hidePos = null;
    /// <summary>マーカーがアクティブかどうか</summary>
    bool m_isActive = false;

    GameObject[] m_childObjects;
    float m_timer;
    Animator m_anim;
    private void Start()
    {
        GetAllChild();
        /*マーカーを隠す*/
        InActiveChild(m_childObjects);
        m_anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 子要素を全て取得する
    /// </summary>
    void GetAllChild()
    {
        m_childObjects = new GameObject[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            m_childObjects[i] = this.transform.GetChild(i).gameObject;
        }
    }

    /// <summary>
    /// 子要素を非アクティブにする
    /// </summary>
    void InActiveChild(GameObject[] childObjects)
    {
        childObjects.ToList().ForEach(g => { Debug.Log($"配列の中身：{g}"); g.SetActive(false); });
        m_isActive = false;
    }

    /// <summary>
    /// 子要素をアクティブにする
    /// </summary>
    void ActiveChild(GameObject[] childObjects)
    {
        childObjects.ToList().ForEach(g => { Debug.Log($"配列の中身：{g}"); g.SetActive(true); });
        m_isActive = true;
    }

    void Update()
    {
        /*一定時間たつとマーカーを消す*/
        if (m_isActive)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= m_deleteTime)
            {
                m_timer = 0;
                InActiveChild(m_childObjects);
            }
        }

        // クリックで Ray を飛ばす
        if (Input.GetButtonDown("Fire1"))
        {
            // カメラの位置 → マウスでクリックした場所に Ray を飛ばすように設定する
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; // out パラメータで Ray の衝突情報を受け取るための変数
            // Ray を飛ばして、コライダーに当たったかどうかを戻り値で受け取る
            bool isHit = Physics.Raycast(ray, out hit); // オーバーライドがたくさんあることに注意すること

            // Ray が当たったかどうかで異なる処理をする
            if (isHit)
            {
                // Ray が当たった時は、当たった座標まで赤い線を引く
                Debug.DrawLine(ray.origin, hit.point, m_debugRayColorOnHit);
                // m_marker がアサインされていたら、それを移動する
                if (m_marker)
                {
                    ActiveChild(m_childObjects);
                    m_marker.transform.position = hit.point + m_markerOffset;
                    m_anim.Play("Marker 0");
                }
            }
            else
            {
                // Ray が当たらなかった場合は、Ray の方向に白い線を引く
                Debug.DrawRay(ray.origin, ray.direction * m_debugRayLength);
            }
        }
    }


}
