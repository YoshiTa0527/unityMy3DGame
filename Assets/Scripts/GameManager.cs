using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState m_gameState;
    [SerializeField] float m_waitoToStartFadeIn = 2f;
    FadeController m_fc;
    AudioManager m_am;
    /// <summary>
    /// フェードが終わった後に表示する
    /// </summary>
    [SerializeField] GameObject m_hudCanvas;
    public static bool PlayerIsFound { get; set; }
    private void Start()
    {
        m_fc = FindObjectOfType<FadeController>();
        m_am = FindObjectOfType<AudioManager>();
        if (m_fc) { Debug.Log($"GameManager::{m_fc.GetType().ToString()}"); }
        PlayerIsFound = false;
        m_fc.FadeIn(m_waitoToStartFadeIn);
    }

    /*アップデートの中にトゥルーかフォルスを書くと毎フレーム音が鳴ってしまう
     状態が切り替わった時にだけ音楽を再生したい
    できればオーデディオソースを使いまわす方法で*/
    private void Update()
    {

    }
    public void ChangeGameState(GameState newState)
    {
        m_gameState = newState;
    }
}
public enum GameState
{
    /*ステージ開始*/
    StartStage,
    /*ステージプレイ中*/
    InGame,
    /*ステージ終了*/
    EndStage,
}
