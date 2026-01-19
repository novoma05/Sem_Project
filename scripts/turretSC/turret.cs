using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting;
using System;
using System.Security.Cryptography;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.Burst.CompilerServices;

public class Turret : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] public GameObject upgradeUI;
    [SerializeField] public Button upgradeButton;
    [SerializeField] public Button sellButton;
    [SerializeField] public Button closeButton;
    [SerializeField] TextMeshProUGUI UpdateCost;
    [SerializeField] TextMeshProUGUI UpdateRange;
    [SerializeField] TextMeshProUGUI UpdateBPS;
    [SerializeField] TextMeshProUGUI SellMoney;
    [SerializeField] TextMeshProUGUI LVLNumber;

    [Header("Addition")]
    [SerializeField] private LayerMask DMGenemyMask;
    [SerializeField] private float DMGRange = 1f;


    [SerializeField] private LayerMask HealerMask;
    [SerializeField] private float HealingRange = 4f;


    [SerializeField] HealthBar healthBar;
    public bool isDestroyed = false;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f;
    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] public int hitPoints, MaxHP = 100;

    public float engle;
    private float bpsBase;
    private float targetingRangeBase;
    private static Turret activeTurret;

    private Transform target;
    private float timeUntilFire;

    private int level = 1;

    private void Start()
    {

        bpsBase = bps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
        sellButton.onClick.AddListener(Sell);
        closeButton.onClick.AddListener(CloseUpgradeUI);
        HealerTrt();
        DMG();
    }

    private void Update()
    {

        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {

            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }

        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        engle = angle;
    }

    public void DMG()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, DMGRange, (Vector2)transform.position, 0f, DMGenemyMask);
        TakeDamage(hits.Length);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.frozen = true;
                em.UpdateSpeed(0);
            }
        }
    }

    public void HealerTrt()
    {
        if (hitPoints >= MaxHP)
        {
            Heal(1);
        }
        else
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, HealingRange, (Vector2)transform.position, 0f, HealerMask);
            Heal(hits.Length);

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];

                    HeallerTurret turr = hit.transform.GetComponent<HeallerTurret>();
                    turr.HealerAnim.SetBool("Healing", true);
                    turr.rangeHighlighter.SetActive(true);
                    turr.StopAnim();
                }
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        healthBar.UpdatehealthBar(hitPoints, MaxHP);

        if (hitPoints <= 0 && !isDestroyed)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, DMGRange, Vector2.zero, 0f, DMGenemyMask);

            foreach (RaycastHit2D hit in hits)
            {
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                if (em != null)
                {
                    em.frozen = false;
                    em.UpdateSpeed(em.baseSpeed);
                }
            }
            isDestroyed = true;
            Destroy(gameObject);
        }

        StartCoroutine(dmging());
    }

    public void Heal(int heal)
    {
        hitPoints += heal;

        if (hitPoints > MaxHP)
        {
            hitPoints = MaxHP;
        }
        healthBar.UpdatehealthBar(hitPoints, MaxHP);
        StartCoroutine(healing());
    }

    public void OpenUpgradeUI()
    {
        if (activeTurret != null && activeTurret != this)
        {
            activeTurret.CloseUpgradeUI();
        }

        upgradeUI.SetActive(true);
        UpdateCost.text = "Cost: " + CalculateCost().ToString();
        UpdateBPS.text = "BPS " + Math.Round(CalculateBPS(), 2).ToString() + " --> " + Math.Round(NextCalculateBPS(), 2).ToString();
        UpdateRange.text = "Range " + Math.Round(CalculateRange(), 2).ToString() + " --> " + Math.Round(NextCalculateRange(), 2).ToString();
        activeTurret = this;
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
    }

    public void Upgrade()
    {
        if (CalculateCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;
        LVLNumber.text = level.ToString();

        bps = CalculateBPS();
        targetingRange = CalculateRange();
        hitPoints = MaxHP;

        OpenUpgradeUI();
    }

    public void Sell()
    {
        SellMoney.text = "Sell: " + baseUpgradeCost.ToString();
        LevelManager.main.IncreaseCurrency(baseUpgradeCost);
        isDestroyed = true;
        Destroy(gameObject);
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.9f));
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.4f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }

    private float NextCalculateBPS()
    {
        return bpsBase * Mathf.Pow(level + 1, 0.4f);
    }

    private float NextCalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level + 1, 0.4f);
    }
    IEnumerator healing()
    {
        yield return new WaitForSeconds(1);
        HealerTrt();
    }
    IEnumerator dmging()
    {
        yield return new WaitForSeconds(0.5f);
        DMG();
    }
}