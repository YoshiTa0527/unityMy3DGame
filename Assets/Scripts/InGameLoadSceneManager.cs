using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム中に呼び出すLoadSceneManager
/// </summary>
public class InGameLoadSceneManager : MonoBehaviour
{
    /// <summary>ロードするシーン名</summary>
    [SerializeField] string m_loadSceneName = null;
    /// <summary>真っ暗な画面からフェードアウトする時間</summary>
    [SerializeField] float m_fadeInTime = 1f;
    /// <summary>フェードに使うパネル</summary>
    [SerializeField] Image m_panelImage = null;
    /// <summary>フェードの時間。アルファ値に徐々にこの値を足していく</summary>
    [SerializeField] float m_fadeValue = 0.1f;

    InGameSceneStatus m_status;
    void Start()
    {
        if (m_loadSceneName == null)
        {
            Debug.LogError("ロードするシーンを設定してください");
        }
        m_status = InGameSceneStatus.FadeIN;
        StartCoroutine("FadeIN");
    }

    void Update()
    {
        /*ステージ開始*/
        /*ステージ終了*/
    }

    IEnumerator FadeIN()
    {
        Debug.Log("IEnumerator FadeIN()：called");
        yield return new WaitForSeconds(m_fadeInTime);
        for (float f = 1f; f >= 0; f -= m_fadeValue)
        {
            Color c = m_panelImage.color;
            c.a = f;
            Debug.Log($"Fade(){c.a}");
            m_panelImage.color = c;
            yield return null;
        }
    }

    IEnumerator FadeOUT()
    {
        Debug.Log("IEnumerator FadeOUT()：called");
        yield return new WaitForSeconds(m_fadeInTime);
        for (float f = 0; f <= 1; f += m_fadeValue)
        {
            Color c = m_panelImage.color;
            c.a = f;
            Debug.Log($"Fade(){c.a}");
            m_panelImage.color = c;
            yield return null;
        }
    }
}

enum InGameSceneStatus

{
    FadeIN,
    ClickToStart,
}
