using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 1f;
    
    private float currentHealth;
    Rigidbody rb;
    private void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        gameObject.SetActive(false);
        RoundManager.instance.EnemyCount();
    }

    //public void ResetEnemy(Vector3 spawnPosition)
    //{
    //    currentHealth = maxHealth;

    //    transform.position = spawnPosition;
    //    gameObject.SetActive(true);
    //}

    public void ResetEnemyPosition(Vector3 newPosition, Vector3 playerPosition)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = new Vector3(
            newPosition.x,
            transform.position.y,
            newPosition.z
        );

        Vector3 directionToPlayer = playerPosition - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude > 0.001f)
        {
            transform.forward = directionToPlayer.normalized;
        }

        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }
}