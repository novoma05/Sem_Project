using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers; // Index 0 = null (nic), 1+ = věže
    [SerializeField] private Button[] towerButtons; // Tlačítka nákupu věží

    // NOVÉ: Odkaz na celý objekt tlačítka pro zrušení (abychom ho mohli skrývat)
    [SerializeField] private GameObject cancelButtonObject;

    [Header("Settings")]
    public Color selectedColor = Color.green;
    public Color defaultColor = Color.white;

    // 0 = žádná věž
    public int selectedTower = 0;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        // Na začátku nastavíme správné barvy a skryjeme tlačítko zrušení
        RefreshUI();
    }

    public Tower GetSelectedTower()
    {
        if (selectedTower == 0 || selectedTower >= towers.Length)
        {
            return null;
        }
        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        // Jednoduše nastavíme vybranou věž
        // Pokud tlačítko "Zrušit" pošle 0, nastaví se 0.
        // Pokud tlačítko věže pošle 1, nastaví se 1.
        selectedTower = _selectedTower;

        // Aktualizujeme vzhled (barvy i viditelnost tlačítka zrušení)
        RefreshUI();
    }

    private void RefreshUI()
    {
        // 1. Aktualizace barev tlačítek věží
        for (int i = 0; i < towerButtons.Length; i++)
        {
            if (towerButtons[i] == null) continue;

            Image btnImage = towerButtons[i].GetComponent<Image>();

            // Tlačítko na indexu 0 odpovídá věži 1, atd.
            int towerIndexForButton = i + 1;

            if (selectedTower == towerIndexForButton)
            {
                btnImage.color = selectedColor;
            }
            else
            {
                btnImage.color = defaultColor;
            }
        }

        // 2. Aktualizace viditelnosti tlačítka "Zrušit"
        if (cancelButtonObject != null)
        {
            // Tlačítko je aktivní POUZE pokud NENÍ vybrána 0 (tedy je vybrána nějaká věž)
            if (selectedTower != 0)
            {
                cancelButtonObject.SetActive(true);
            }
            else
            {
                cancelButtonObject.SetActive(false);
            }
        }
    }
}