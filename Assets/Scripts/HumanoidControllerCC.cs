using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidControllerCC : MonoBehaviour
{
    /// <summary>移動速度</summary>
    [SerializeField] float m_walkSpeed = 1f;
    /// <summary>走る移動速度</summary>
    [SerializeField] float m_runSpeed = 2f;

    /// <summary>移動速度</summary>
    [SerializeField] float m_moveSpeed = 1f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary>重力のスケール</summary>
    [SerializeField] float m_gravityScale = 2f;

    [SerializeField] float m_isGroundedLength = 5f;

    CharacterController m_cc;
    Animator m_anim;
    /// <summary>キャラクターの移動方向</summary>
    Vector3 m_moveDirection;

    void Start()
    {
        m_cc = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();


    }

    void Update()
    {
        Debug.Log(m_cc.isGrounded);
        // 方向の入力を取得し、入力方向の単位ベクトルを計算する
        float h = Input.GetAxisRaw("Horizontal");
        //Debug.Log(h);
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;  // dir = 入力された方向（xz 平面）の単位ベクトル
        //Debug.Log(dir);

        if (dir != Vector3.zero)
        {
            // キャラクターを入力された方向に向ける
            this.transform.forward = dir;
        }

        //移動スティックが入力されているなら
        if (h != 0 || v != 0)
        {
            //Fire1で走る
            if (Input.GetButton("Fire1"))
            {
                m_anim.SetFloat("Speed", m_runSpeed);
                m_moveDirection.x = dir.x * m_runSpeed;
                m_moveDirection.z = dir.z * m_runSpeed;
            }
            else　//それ以外は歩く
            {
                m_anim.SetFloat("Speed", m_walkSpeed);
                m_moveDirection.x = dir.x * m_walkSpeed;
                m_moveDirection.z = dir.z * m_walkSpeed;
            }
        }
        //入力されてないなら待機モーション
        else
        {
            m_anim.SetFloat("Speed", 0f);
            m_moveDirection.x = dir.x * 0;
            m_moveDirection.z = dir.z * 0;
        }

        if (IsGrounded())    // Character Controller コンポーネントには isGrounded プロパティがある
        {
            m_anim.SetBool("IsFalling", false);
            m_moveDirection.y = 0;

        }
        else
        {
            m_anim.SetBool("IsFalling", true);
            // 空中にいる時は、重力に従って下に移動する
            m_moveDirection += Physics.gravity * m_gravityScale * Time.deltaTime;
        }

        // Character Controller を使って移動する
        m_cc.Move(m_moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Character Controller の接触判定を行う関数
    /// Collider の時とは違うものを使うことに注意すること
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name + " と接触した");
    }

    bool IsGrounded()
    {
        if (m_cc.isGrounded) { return true; }


        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position + Vector3.up * 0.5f;   // start: オブジェクトの中心
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
                                    //bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
                                    //  Physics.CapsuleCast(start, end, 0.5f, Vector3.down);
        Ray ray = new Ray(start, end);
        bool isGrounded = Physics.SphereCast(ray, 5f);
        return isGrounded;
    }

}
