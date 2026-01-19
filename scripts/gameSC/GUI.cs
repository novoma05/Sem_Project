using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    public pauseMenu pause;

    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI healthUI;

    private void OnGUI()
    {
        currencyUI.text = "Money "+LevelManager.main.currency.ToString();
        healthUI.text = "Health " + LevelManager.main.LvlHealth.ToString();
    }

    public void DoubleSpeed()
    {
        Time.timeScale = 2;
    }
    public void NormalSpeed()
    {
        Time.timeScale = 1;
    }
    public void Pause()
    {
        pause.Setup();
    }
}