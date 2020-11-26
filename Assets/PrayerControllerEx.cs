using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rigidbody を使ってプレイヤーを動かすコンポーネント
/// 入力を受け取り、それに従ってオブジェクトを動かす。
/// PlayerControllerRb との違いは以下の通り。
/// 1. Rigidbody.AddForce() ではなく Rigidbody.velocity で動かしている（※１）
/// 2. World 座標系ではなく、カメラの座標系に対して動かしている（※２）
/// 3. 方向転換時に Quartenion.Slerp() を使って滑らかに方向転換している
/// （※１）AddForce() 動かすことは問題ではなく、挙動や実装を比較するために変えている。
/// （※２）World 座標系で動かすと、カメラの回転に対応できないため
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PrayerControllerEx : MonoBehaviour
{
    /// <summary>動く速さ</summary>
    [SerializeField] float m_movingSpeed = 5f;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float m_turnSpeed = 3f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary>接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ</summary>
    [SerializeField] float m_isGroundedLength = 1.1f;
    /// <summary>壁があるか否か</summary>
    [SerializeField] float m_isWallLength = 1.0f;
    /// <summary>壁に捕まっているときの落下速度</summary>
    [SerializeField] float m_holdPower = 1f;

    [SerializeField] bool m_canMoveInTheAir = false;

    bool m_isWallRunning = false;
    [SerializeField] float mm_maxWallRunSpeed = 20f;
    [SerializeField] float m_wallRunForce = 5f;
    [SerializeField] float m_pushPower = 3f;

    bool m_isRightWall = false;
    bool m_isLeftWall = false;

    Rigidbody m_rb;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        IsGrounded();
        IsWall();
        // 方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // 入力方向のベクトルを組み立てる
        Vector3 dir = Vector3.forward * v + Vector3.right * h;

        if (dir == Vector3.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を保持するだけ
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
        }
        else //if(m_canMoveInTheAir||IsGrounded())
        {
            // カメラを基準に入力が上下=奥/手前, 左右=左右にキャラクターを向ける
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);  // Slerp を使うのがポイント

            if (m_canMoveInTheAir || IsGrounded())
            {
                Vector3 velo = dir.normalized * m_movingSpeed; // 入力した方向に移動する
                velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
                m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
            }
        }

        // ジャンプの入力を取得し、接地している時に押されていたらジャンプする
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        }

        //壁に捕まる処理。空中にいるときと近くに壁があるとき
        if (Input.GetButton("Fire1") && m_isRightWall)
        {
            StartWallRun();
        }
        if (Input.GetButton("Fire1") && m_isLeftWall)
        {
            StartWallRun();
        }

    }



    /// <summary>
    /// 地面に接触しているか判定する
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position;   // start: オブジェクトの中心
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする

        return isGrounded;
    }

    void IsWall()
    {
        //線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position;   // start: オブジェクトの中心
        Vector3 rightEnd = start + Vector3.right * m_isWallLength;  // 右にレイを出す
        Vector3 leftEnd = start + Vector3.left * m_isWallLength;  // 右にレイを出す
        Debug.DrawLine(start, rightEnd); // 動作確認用に Scene ウィンドウ上で線を表示する
        Debug.DrawLine(start, leftEnd);

        m_isRightWall = Physics.Raycast(start, rightEnd);
        m_isLeftWall = Physics.Raycast(start, leftEnd);

        if (!m_isRightWall && !m_isLeftWall)
        {
            StopWallRun();
        }


    }

    void StartWallRun()
    {
        m_rb.useGravity = false;
        m_isWallRunning = true;


        if (m_rb.velocity.magnitude <= mm_maxWallRunSpeed)
        {
            m_rb.AddForce(this.transform.forward * m_wallRunForce);
        }

        if (m_isRightWall)
        {
            m_rb.AddForce(this.transform.right * m_wallRunForce / m_pushPower); //壁に張り付きながら走りたいので、壁のある方向へ力をかけている
        }
        else
        {
            m_rb.AddForce(-this.transform.right * m_wallRunForce / m_pushPower);
            Debug.Log(this.transform.right);
        }

    }

    void StopWallRun()
    {
        m_isWallRunning = false;
        m_rb.useGravity = true;
    }
}
