using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PrayerController : MonoBehaviour
{
    /// <summary>動く力</summary>
    [SerializeField] float m_movePower = 10f;
    //速度制限
    [SerializeField] float m_maxSpeed = 5f;
    //減速する際に使う係数
    [SerializeField] float m_coefficient = 0.9f;
    //回転速度
    [SerializeField] float m_turnSpeed = 0.2f;
    /// <summary> 入力されている方向</summary>
    Vector2 m_inputDirection;

    //ジャンプ力
    [SerializeField] float m_junpPower = 5f;
    //設置判定をする際に、中心からどれくらいの距離を設置していると判定するかの長さ
    [SerializeField] float m_isGroundLength = 1.2f;
    //空中制御可能か
    [SerializeField] bool m_moveInTheAir = false;

    Rigidbody m_rb;
    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //入力されている方向を保存する
        m_inputDirection.x = Input.GetAxisRaw("Horizontal");
        m_inputDirection.y = Input.GetAxisRaw("Vertical");

        //ジャンプの入力を取得し、設置しているときに押されたらジャンプする
        if (Input.GetButtonDown("Jump") && IsGround())
        {
            m_rb.AddForce(Vector3.up * m_junpPower, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        //入力された方向に従ってオブジェクトを動かす

        Vector3 dir = Vector3.forward * m_inputDirection.y + Vector3.right * m_inputDirection.x;

        if (m_inputDirection == Vector2.zero)
        {
            //何も入力されていないときは、徐々に減速する
            Vector3 v = new Vector3(m_rb.velocity.x * m_coefficient, m_rb.velocity.y, m_rb.velocity.z * m_coefficient);
            m_rb.velocity = v;
        }
        else
        {
            //最初に、xz 平面上の速度を求める
            float speed = new Vector2(m_rb.velocity.x, m_rb.velocity.z).magnitude;

            //入力されている方向（x,y平面）を動く方向（ｘ、ｚ平面）に変換する
            dir = Camera.main.transform.TransformDirection(dir);
            dir.y = 0;

            //入力されている方向にキャラクターを向ける
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);
                      
            if (m_moveInTheAir || IsGround())
            {
                if (speed <= m_maxSpeed)
                {
                    m_rb.AddForce(dir * m_movePower);
                }
            }

        }
    }

    bool IsGround()
    {
        //Physics.Linecastを使って設置判定をする
        Vector3 start = this.transform.position;
        Vector3 end = start + Vector3.down * m_isGroundLength;
        Debug.DrawLine(start, end);
        bool isGround = Physics.Linecast(start, end);//引いたラインに何かがぶつかっていたらtrueを返す
        return isGround;
    }
}
