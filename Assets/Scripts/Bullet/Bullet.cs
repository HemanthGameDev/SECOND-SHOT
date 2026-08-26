using UnityEngine;

public enum BulletType
{
    Player,
    Shadow
}

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private BulletType bulletType;

    private Rigidbody rb;
    private Vector3 direction;
    private float lifeTimer;
    private bool hasBounced;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 shootDirection)
    {
        direction = shootDirection;

        direction.y = 0f;
        direction.Normalize();

        hasBounced = false;
        lifeTimer = 0f;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.forward = direction;

        rb.linearVelocity = direction * speed;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // --------------------------------
        // WALL
        // --------------------------------
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;

            direction = Vector3.Reflect(direction, normal);

            direction.y = 0f;
            direction.Normalize();

            transform.forward = direction;

            rb.linearVelocity = direction * speed;

            hasBounced = true;

            return;
        }

        // --------------------------------
        // PLAYER BULLET → ENEMY
        // --------------------------------
        if (bulletType == BulletType.Player &&
            collision.gameObject.CompareTag("Enemy"))
        {
            // Direct shots are not allowed to kill the enemy.
            if (!hasBounced)
            {
                gameObject.SetActive(false);
                return;
            }

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage();
            }

            gameObject.SetActive(false);

            return;
        }

        // --------------------------------
        // SHADOW BULLET → PLAYER
        // --------------------------------
        if (bulletType == BulletType.Shadow &&
            collision.gameObject.CompareTag("Player"))
        {
            // Shadow also needs to bounce first.
            if (!hasBounced)
            {
                return;
            }

            PlayerController player =
                collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.Die();
            }

            gameObject.SetActive(false);
        }
    }
}