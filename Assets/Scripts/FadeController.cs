using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    /// <summary>フェードに使うパネル</summary>
    [SerializeField] Image m_panelImage = null;
    /// <summary>フェードインにかける時間</summary>
    [SerializeField] float m_fadeInTime = 1f;
    /// <summary>フェードアウトにかける時間</summary>
    [SerializeField] float m_fadeOutTime = 1f;

    /// <summary>
    /// 真っ黒なイメージに徐々にアルファ値を引いていき透明にする
    /// </summary>
    /// <param name="m_waitTime">フェードインする待ち時間</param>
    /// <returns></returns>
    public void FadeIn(float m_waitTime)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(m_waitTime).
            Append(m_panelImage.DOFade(0, m_fadeInTime));
        seq.Play();
    }

    /// <summary>
    /// 真っ黒なイメージに徐々にアルファ値を引いていき透明にする
    /// </summary>
    /// <returns></returns>
    public void FadeIn()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(m_panelImage.DOFade(0, m_fadeInTime));
        seq.Play();
    }

    /// <summary>
    /// 透明なイメージに徐々にアルファ値を足していき真っ黒にする
    /// </summary>
    /// <param name="m_waitTime"></param>
    public void FadeOut(float m_waitTime)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(m_waitTime)
           .Append(m_panelImage.DOFade(1, m_fadeInTime));
        seq.Play();
    }

    /// <summary>
    /// 透明なイメージに徐々にアルファ値を足していき真っ黒にする
    /// </summary>
    public void FadeOut()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(m_panelImage.DOFade(1, m_fadeInTime));
        seq.Play();
    }
}
