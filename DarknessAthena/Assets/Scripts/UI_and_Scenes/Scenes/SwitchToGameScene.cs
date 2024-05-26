using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToGameScene : MonoBehaviour
{
    public void SwitchToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void SwitchToAnimation()
    {
        SceneManager.LoadScene("AnimationScene_One");
    }
}
