using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] public float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    public float baseSpeed;
    public bool frozen=false;
    private void Start()
    {
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                LevelManager.main.DecreaseHealth(1);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    public void UpdateSpeed(float newSpeed)
    {
        if (frozen == true)
        {
            moveSpeed = 0;
        }
        else {
            moveSpeed = newSpeed;
            StartCoroutine(waiter());
        }
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(2);
        ResetSpeed();
    }

}
