using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] string m_loadSceneName = null;
    [SerializeField] float m_waitTimeToFadeIn = 2f;
    [SerializeField] float m_waitTimeToFadeOut = 1f;
    [SerializeField] float m_waitTimeToLoad = 2f;
    [SerializeField] float m_timeToAudioFadeOut = 1f;
    [SerializeField] Animator m_clickToStart = null;
    /// <summary>クリックされたときになる音</summary>
    [SerializeField] AudioClip m_audio;
    FadeController m_fc;
    AudioSource m_audioSource;
    AudioController m_ac;
    bool m_canGetInput = false;
    private void Start()
    {
        m_fc = FindObjectOfType<FadeController>();
        m_ac = FindObjectOfType<AudioController>();
        m_audioSource = GetComponent<AudioSource>();
        if (m_loadSceneName != null)
        {
            Debug.Log($"{m_loadSceneName}をロードする準備ができました");
        }
        else { Debug.LogError("シーンを設定してください"); }
        m_fc.FadeIn(m_waitTimeToFadeIn);
    }
    float m_timer;
    private void Update()
    {
        /*フェードインが終わるまで入力を受け付けない*/
        m_timer += Time.deltaTime;
        if (m_timer >= m_waitTimeToFadeIn) { m_canGetInput = true; }

        if (Input.GetButtonDown("Fire1") && m_canGetInput)
        {
            Debug.Log("TitleSceneManager：右クリック");
            if (m_audio)
            {
                AudioSource.PlayClipAtPoint(m_audio, Camera.main.transform.position);
            }
            m_clickToStart.Play("IsClicked");
            m_ac.FadeOut(m_timeToAudioFadeOut);
            m_fc.FadeOut(m_waitTimeToFadeOut);
            StartCoroutine(LoadScene());

        }
    }


    IEnumerator LoadScene()
    {

        yield return new WaitForSeconds(m_waitTimeToLoad);
        SceneManager.LoadScene(m_loadSceneName);
    }


}
