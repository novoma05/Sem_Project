using System.Collections;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float attackInterval = 3f; // Attack every 3 seconds
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private LayerMask turretMask; // Specific mask for turrets

    private float timeSinceLastAttack = 0f;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeSinceLastAttack += Time.deltaTime;

            if (timeSinceLastAttack >= attackInterval)
            {
                AttackTurret();
                timeSinceLastAttack = 0f;
            }
        }
    }

    private void AttackTurret()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        EnBullet bulletScript = bulletObj.GetComponent<EnBullet>();
        bulletScript.SetTarget(target, true); // Set isTurret to true for turrets
    }

    private void FindTarget()
    {
        // Use RaycastHit2D[] for multiple potential targets if needed
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, targetingRange, Vector2.zero, 0f, turretMask);

        if (hit.collider != null)
        {
            target = hit.transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
}