using System.Collections;
using UnityEngine;
using UnityEditor;

public class HealingEnm : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask turretMask;

    [Header("Attributes")]
    [SerializeField] private float healingRange = 5f;
    [SerializeField] private float hps = 0.33f;  // Healing per second.
    [SerializeField] private int heal = 1; //health heal by this unit
    EnemyMovement mvt;
    public Animator anim;
    public GameObject rangeHighlighter;

    private float timeUntilHeal;

    private void Update()
    {
        timeUntilHeal += Time.deltaTime;

        if (timeUntilHeal >= 1f / hps)
        {
            mvt = GetComponent<EnemyMovement>();
            HealEnm();
            anim.SetBool("healing", true);
            mvt.moveSpeed = 0;
            StartCoroutine(waiter());
            timeUntilHeal = 0f;
        }
    }

    private void HealEnm()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, healingRange, (Vector2)transform.position, 0f, turretMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                Health em = hit.transform.GetComponent<Health>();
                em.Heal(heal);
            }
        }
    }

    IEnumerator waiter()
    {
        rangeHighlighter.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        mvt.moveSpeed = mvt.baseSpeed;
        anim.SetBool("healing", false);
        rangeHighlighter.SetActive(false);
    }
}