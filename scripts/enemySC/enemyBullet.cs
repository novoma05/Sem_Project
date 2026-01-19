using UnityEngine;

public class EnBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private int damage = 1;

    private bool isTurret = false;

    private Transform target;

    public void SetTarget(Transform target, bool isTurret = false)
    {
        this.target = target;
        this.isTurret = isTurret;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTurret && collision.gameObject.CompareTag("Turret")) // Use the flag
        {
            Turret turret = collision.gameObject.GetComponent<Turret>();
            if (turret != null)
            {
                turret.TakeDamage(damage); // Assuming you have a 'damage' variable
            }
            Destroy(gameObject);
        }
    }
}