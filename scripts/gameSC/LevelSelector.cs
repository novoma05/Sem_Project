using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void Level1()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("level 1");
    }

    public void Level2()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("level 2");
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
