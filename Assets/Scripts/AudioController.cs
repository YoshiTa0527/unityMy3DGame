using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// オーディオをフェードイイン・アウトさせるスクリプト。
/// </summary>
public class AudioController : MonoBehaviour
{
    AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// 2秒かけて音を0にする
    /// </summary>
    public void FadeOut()
    {
        m_audioSource.DOFade(0, 2f);
    }
    /// <summary>
    /// 指定した時間かけ音量を0にする
    /// </summary>
    /// <param name="time"></param>
    public void FadeOut(float time)
    {
        m_audioSource.DOFade(0, time);
    }
    /// <summary>
    /// 音をフェードイン・アウトさせる
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="time"></param>
    /// <param name="targetVolume"></param>
    public void Fade(AudioSource audioSource, float targetVolume, float time)
    {
        audioSource.DOFade(targetVolume, time);
    }

    /// <summary>
    /// 指定した時間かけ音量を指定した値にする
    /// </summary>
    /// <param name="time"></param>
    public void Fadein(float time, float defaultVolume)
    {
        m_audioSource.DOFade(defaultVolume, time);
    }


}
