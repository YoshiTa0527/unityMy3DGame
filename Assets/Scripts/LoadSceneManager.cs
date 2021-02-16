using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    /// <summary>ロードするシーン名</summary>
    [SerializeField] string m_loadSceneName = null;
    /// <summary>真っ暗な画面からフェードアウトする時間</summary>
    [SerializeField] float m_fadeInTime = 5f;
    /// <summary>フェードに使うパネル</summary>
    [SerializeField] Image m_panelImage = null;
    /// <summary>フェードの時間。アルファ値に徐々にこの値を足していく</summary>
    [SerializeField] float m_fadeValue = 2f;

    SceneStatus m_status;
    void Start()
    {
        if (m_loadSceneName == null)
        {
            Debug.LogError("ロードするシーンを設定してください");
        }
        m_status = SceneStatus.FadeIN;
    }

    // Update is called once per frame
    void Update()
    {

        switch (m_status)
        {
            case SceneStatus.FadeIN:
                if (Input.GetButtonDown("Fire1"))
                {
                    Debug.Log("SceneStatus：Fade");
                    StartCoroutine("FadeIN");
                    m_status = SceneStatus.ClickToStart;
                }
                break;
            case SceneStatus.ClickToStart:
                if (Input.GetButtonDown("Fire1"))
                {
                    Debug.Log("SceneStatus：ClickToStart");
                    SceneManager.LoadScene(m_loadSceneName);
                }
                break;
            default:
                break;
        }
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

enum SceneStatus
{
    FadeIN,
    ClickToStart,
}
