using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState m_gameState;
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
