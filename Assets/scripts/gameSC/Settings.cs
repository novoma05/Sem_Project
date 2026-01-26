using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void OnResolutionChange(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                break;
            case 2:
                Screen.SetResolution(2560, 1440, FullScreenMode.FullScreenWindow);
                break;
            case 3:
                Screen.SetResolution(3840, 2160, FullScreenMode.FullScreenWindow);
                break;
        }

        //Debug.Log("Rozlišení změněno na index: " + index);
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}