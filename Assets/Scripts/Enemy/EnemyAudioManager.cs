using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] m_audioClips;
    AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 音を再生する。再生する曲の名前はフルネームでいれること。
    /// </summary>
    /// <param name="str"></param>
    public void PlaySound(string str)
    {
        m_audioSource.clip = m_audioClips.Where(clip => clip.name.Contains(str)).FirstOrDefault();
        m_audioSource.PlayOneShot(m_audioSource.clip);
    }
}
