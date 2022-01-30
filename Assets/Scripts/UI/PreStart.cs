using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreStart : MonoBehaviour
{
    public void PlayButtonFunc()
    {
        LevelManager.gameState = GameState.Normal;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
