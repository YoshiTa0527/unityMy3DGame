using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState m_gameState;
    FadeController m_fc;
    private void Start()
    {
        m_fc = FindObjectOfType<FadeController>();
        m_fc.FadeIn();
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
