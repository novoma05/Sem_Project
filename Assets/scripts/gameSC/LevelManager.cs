using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager main;
    public EndScreen END;

    public Transform startPoint;
    public Transform[] path;

    public int currency;
    public int LvlHealth;
    public int Score;

    [SerializeField] private int startingCurrency = 1000;
    [SerializeField] private int startingLvlHealth = 50;
    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = startingCurrency;
        LvlHealth = startingLvlHealth;
    }

    public void IncreaseCurrency(int amount)
    {
        Score += 1;
        currency += amount;
    }

    public void DecreaseHealth(int amount)
    {
        LvlHealth -= amount;
        if (LvlHealth == 0) 
        {
            END.Setup();
        }
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item");
            return false;
        }
    }

}