using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム中に呼び出すLoadSceneManager
/// </summary>
public class ReturnToTitle : MonoBehaviour
{
    /// <summary>ロードするシーン名</summary>
    [SerializeField] string m_loadSceneName = null;

    public void OnReturnToTitle()
    {
        SceneManager.LoadScene(m_loadSceneName);
    }

}


