using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// AudioSourceを持っているオブジェクトにつける
/// </summary>
public class AudioManager : MonoBehaviour
{
    AudioSource m_audioSource;
    [SerializeField] AudioClip[] m_audioClips;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }



}

