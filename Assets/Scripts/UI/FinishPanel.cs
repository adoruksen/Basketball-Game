using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPanel : MonoBehaviour
{
    #region Singleton Pattern
    public static FinishPanel instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion


    public void NextButtonFunc()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

}
