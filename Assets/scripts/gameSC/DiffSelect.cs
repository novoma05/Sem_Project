using UnityEngine;
using UnityEngine.SceneManagement;

public class DiffSelect : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;

        if (EnemySpawner.main != null)
            EnemySpawner.main.SetupDifficulty();
    }

    public void EasyButton()
    {
        EnemySpawner.selectedDifficulty = EnemySpawner.Difficulty.Easy;
        ApplyDifficultyAndStart();
    }

    public void NormalButton()
    {
        EnemySpawner.selectedDifficulty = EnemySpawner.Difficulty.Normal;
        ApplyDifficultyAndStart();
    }

    public void HardButton()
    {
        EnemySpawner.selectedDifficulty = EnemySpawner.Difficulty.Hard;
        ApplyDifficultyAndStart();
    }

    private void ApplyDifficultyAndStart()
    {
        if (EnemySpawner.main != null)
        {
            EnemySpawner.main.SetupDifficulty();
        }
        else
        {
            Debug.LogError("Chyba: EnemySpawner nebyl nalezen ve scéně!");
        }

        Time.timeScale = 1;

        this.gameObject.SetActive(false);
    }
}