using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bestiar : MonoBehaviour
{
    [Header("Data Listy")]
    public List<BestiaryItemData> towerList;
    public List<BestiaryItemData> enemyList;

    [Header("UI Reference - Seznam")]
    public Transform contentContainer;
    public GameObject itemButtonPrefab;

    [Header("UI Reference - Kategorie Tlačítka")]

    public Button towerCategoryBtn;
    public Button enemyCategoryBtn;

    [Header("UI Reference - Detail")]
    public TextMeshProUGUI detailNameText;
    public Image detailImage;
    public TextMeshProUGUI detailDescriptionText;
    public GameObject detailPanel;

    [Header("Nastavení Barev")]
    public Color normalItemColor = Color.white;      
    public Color selectedItemColor = Color.green;    
    public Color activeCategoryColor = Color.yellow; 
    public Color inactiveCategoryColor = Color.gray; 

    private Image currentSelectedImage;

    private void Start()
    {
        detailPanel.SetActive(false);

        ShowTowers();
    }

    // --- Funkce pro tlačítka kategorií (levý panel) ---

    public void ShowTowers()
    {
        SetCategoryColors(towerCategoryBtn, enemyCategoryBtn);

        GenerateList(towerList);
    }

    public void ShowEnemies()
    {
        SetCategoryColors(enemyCategoryBtn, towerCategoryBtn);

        GenerateList(enemyList);
    }

    private void SetCategoryColors(Button activeBtn, Button inactiveBtn)
    {
        activeBtn.image.color = activeCategoryColor;
        inactiveBtn.image.color = inactiveCategoryColor;
    }

    // --- Logika generování a výběru ---

    private void GenerateList(List<BestiaryItemData> dataList)
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        currentSelectedImage = null;
        detailPanel.SetActive(false);

        foreach (BestiaryItemData item in dataList)
        {
            GameObject newButtonObj = Instantiate(itemButtonPrefab, contentContainer);

            TextMeshProUGUI btnText = newButtonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null) btnText.text = item.itemName;

            Image btnImage = newButtonObj.GetComponent<Image>();
            btnImage.color = normalItemColor;

            Button btn = newButtonObj.GetComponent<Button>();

            btn.onClick.AddListener(() => OnItemClicked(item, btnImage));
        }
    }

    private void OnItemClicked(BestiaryItemData item, Image clickedImage)
    {
        if (currentSelectedImage != null)
        {
            currentSelectedImage.color = normalItemColor;
        }

        clickedImage.color = selectedItemColor;

        currentSelectedImage = clickedImage;

        detailPanel.SetActive(true);
        detailNameText.text = item.itemName;
        detailDescriptionText.text = item.description;
        detailImage.sprite = item.icon;
        detailImage.preserveAspect = true;
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}