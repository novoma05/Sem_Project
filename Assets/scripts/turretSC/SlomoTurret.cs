using System.Collections;
using UnityEngine;

public class TurretSlomo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private SpriteRenderer rangeHighlighter;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float aps = 0.5f;
    [SerializeField] private float highlightDuration = 0.5f; // Duration to keep highlighter active after firing

    private float timeUntilFire;

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            FreezeEnemies();
            StartCoroutine(DisableHighlightAfterDelay());
            timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                EnemyMovement em = hits[i].transform.GetComponent<EnemyMovement>();
                if (em != null)
                {
                    em.UpdateSpeed(0f);
                }
            }

            // Enable the range highlighter when enemies are frozen
            if (rangeHighlighter != null)
            {
                rangeHighlighter.enabled = true;
                rangeHighlighter.transform.localScale = new Vector3(targetingRange * 5.5f, targetingRange * 5.5f, 1f);
            }
        }
    }

    private IEnumerator DisableHighlightAfterDelay()
    {
        yield return new WaitForSeconds(highlightDuration);

        // Disable the range highlighter after the delay
        if (rangeHighlighter != null)
        {
            rangeHighlighter.enabled = false;
        }
    }
}