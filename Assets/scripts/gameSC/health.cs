using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Health main;
    [Header("Attributes")]
    [SerializeField] public int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;
    [SerializeField] HealthBar healthBar;
    [SerializeField] TextMeshProUGUI BossHealthText;
    public int MaxHP;

    public bool isDestroyed = false;
    private void Start()
    {
        MaxHP = hitPoints;
    }

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        BossHealthText.text = "Král Elfů: "+hitPoints+"/"+MaxHP;
        healthBar.UpdatehealthBar(hitPoints, MaxHP);

        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
        
    }

    public void Heal(int dmg)
    {
        hitPoints += dmg;
        
        if (hitPoints > MaxHP)
        {
            hitPoints = MaxHP;
        }
        healthBar.UpdatehealthBar(hitPoints, MaxHP);
    }

}
