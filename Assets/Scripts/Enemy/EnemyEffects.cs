using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// enemyの状態によってエフェクトをクローンして敵の頭上に出したり消したりする
/// </summary>
public class EnemyEffects : MonoBehaviour
{
    /// <summary>for debug</summary>
    [SerializeField] bool m_checkEffects = false;
    [SerializeField] GameObject[] m_MarkPrefabs = null;
    [SerializeField] float m_deleteTime = 1f;

    private void Start()
    {
        if (m_checkEffects) ActiveExclamationMark();
        if (m_MarkPrefabs == null) { Debug.Log("配列が空"); }
        else m_MarkPrefabs.ToList().ForEach(i => Debug.Log($"配列の中身:{i.gameObject.name}"));
    }

    public void ActiveExclamationMark()
    {
        m_MarkPrefabs.Where(i => i.gameObject.name != "ExclamationMark").FirstOrDefault().SetActive(false);
        m_MarkPrefabs.Where(i => i.gameObject.name == "ExclamationMark").FirstOrDefault().SetActive(true);
    }
    public void InActiveExxlamationMark()
    {
        m_MarkPrefabs.Where(i => i.gameObject.name == "ExclamationMark").FirstOrDefault().SetActive(false);
    }

    public void ActiveQuestionMark()
    {
        m_MarkPrefabs.Where(i => i.gameObject.name != "QuestionMark").FirstOrDefault().SetActive(false);
        m_MarkPrefabs.Where(i => i.gameObject.name == "QuestionMark").FirstOrDefault().SetActive(true);
    }

    public void InActiveQuestionMark()
    {
        m_MarkPrefabs.Where(i => i.gameObject.name == "QuestionMark").FirstOrDefault().SetActive(false);
    }



}
