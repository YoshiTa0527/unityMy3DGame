using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] string m_loadSceneName = null;
    [SerializeField] float m_waitTime = 2f;
    FadeController m_fc;

    private void Start()
    {
        m_fc = FindObjectOfType<FadeController>();
        if (m_loadSceneName != null)
        {
            Debug.Log($"{m_loadSceneName}をロードする準備ができました");
        }
        else { Debug.LogError("シーンを設定してください"); }
        m_fc.FadeIn(m_waitTime);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("TitleSceneManager：右クリック");
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        m_fc.FadeOut();
        yield return new WaitForSeconds(m_waitTime);
        SceneManager.LoadScene(m_loadSceneName);
        yield return null;
    }


}
