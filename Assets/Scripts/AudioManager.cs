using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

/// <summary>
/// AudioSourceを持っているオブジェクトにつける。
/// プレイヤーの状態によって流す音楽を変える
/// </summary>
public class AudioManager : MonoBehaviour
{
    /*名前で設定する*/
    //[SerializeField] AudioClip[] m_audioClips;
    [SerializeField] AudioClip m_findBgm;
    [SerializeField] AudioClip m_defaultBgm;
    [SerializeField, Range(0, 1)] float m_audioVolume;
    [SerializeField] float fadeTime;
    AudioSource m_audioSource;
    AudioController m_ac;
    GameManager m_gm;
    int m_musicIndex;
    PlayerControllerAI m_pcai;
    List<int> m_idList = new List<int>();
    bool m_isChasing = false;

    private void Start()
    {
        m_pcai = FindObjectOfType<PlayerControllerAI>();
        m_ac = GetComponent<AudioController>();
        m_gm = FindObjectOfType<GameManager>();
        m_audioSource = GetComponent<AudioSource>();
        m_musicIndex = 0;

    }

    /// <summary>
    /// BGMを再生する。引数が0の時はデフォルトのBGM。1の時は敵に見つかったときのBGM
    /// </summary>
    /// <param name="i"></param>
    //public void PlayBGM(int i)
    //{
    //    if (m_audioSource == null || m_audioClips == null || m_audioClips.Length == 0)
    //    {
    //        return;
    //    }

    //    if (0 <= i && i < m_audioClips.Length)
    //    {
    //        if (m_audioClips[i] != null)
    //        {
    //            m_audioSource.clip = m_audioClips[i];
    //            m_audioSource.Play();
    //            m_musicIndex = i;
    //        }
    //    }

    //}

    public void PlayDefault(int id)
    {
        m_idList.Remove(id);
        if (m_idList.Count() < 1)
        {
            m_audioSource.Stop();
            m_audioSource.clip = m_defaultBgm;
            m_audioSource.Play();
            m_isChasing = false;
        }
    }
    public void PlayFindBgm(int id)
    {
        m_idList.Add(id);
        if (m_idList.Count == 1)
        {
            m_audioSource.Stop();
            m_audioSource.clip = m_findBgm;
            m_audioSource.Play();
            m_isChasing = true;
        }

    }
    //public void StopBGM()
    //{
    //    if (m_audioSource == null || m_audioClips == null || m_audioClips.Length == 0)
    //    {
    //        return;
    //    }

    //    if (m_audioSource.isPlaying)
    //    {
    //        m_audioSource.Stop();
    //    }
    //}

    //public void Next()
    //{
    //    m_musicIndex = (int)Mathf.Repeat(++m_musicIndex, m_audioClips.Length);
    //    if (m_audioSource != null && m_audioSource.isPlaying)
    //        PlayBGM(m_musicIndex);
    //}

    //public void Prev()
    //{
    //    m_musicIndex = (int)Mathf.Repeat(--m_musicIndex, m_audioClips.Length);
    //    if (m_audioSource != null && m_audioSource.isPlaying)
    //        PlayBGM(m_musicIndex);
    //}

    //public void PlayBGM()
    //{
    //    PlayBGM(m_musicIndex);
    //}




}

