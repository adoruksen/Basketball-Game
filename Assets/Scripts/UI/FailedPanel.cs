using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailedPanel : MonoBehaviour
{
    #region Singleton Pattern
    public static FailedPanel instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    public void TryAgainButtonFunc()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
