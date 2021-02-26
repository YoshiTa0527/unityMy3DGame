using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

/// <summary>
/// AudioSourceを持っているオブジェクトにつける。
/// プレイヤーの状態によって流す音楽を変える
/// 後々イベントを使って改良する。
/// </summary>
public class AudioManager : MonoBehaviour
{
    /*名前で設定する*/
    //[SerializeField] AudioClip[] m_audioClips;
    [SerializeField] AudioClip m_findBgm;
    [SerializeField] AudioClip m_defaultBgm;
    [SerializeField] AudioClip m_resultBgm;
    [SerializeField, Range(0, 1)] float m_audioVolume;
    [SerializeField] float fadeTime;
    AudioSource m_audioSource;
    AudioController m_ac;
    List<int> m_idList = new List<int>();
    bool m_isChasing = false;

    private void Start()
    {
        m_ac = GetComponent<AudioController>();
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.volume = 0;
        m_ac.Fade(m_audioSource, m_audioVolume, fadeTime);
        m_audioSource.Play();

    }


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
    public void PlayFind(int id)
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

    public void PlayResult()
    {
        m_audioSource.Stop();
        m_audioSource.clip = m_resultBgm;
        m_audioSource.Play();
    }

}

