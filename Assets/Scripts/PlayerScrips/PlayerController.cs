using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private InputSystem_Actions action;
    private Rigidbody rb;

    [SerializeField] private float xClamp = 5f;
    [SerializeField] private float zClamp = 5f;

    public Vector2 MoveInput { get; private set; }

    private bool isDead;
    public bool IsDead => isDead;

    private void Awake()
    {
        action = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void FixedUpdate()
    {
        MoveInput = action.Player.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(
            MoveInput.x,
            0f,
            MoveInput.y
        ) * moveSpeed;

        rb.linearVelocity = move;
    }

    public void ResetPlayerPosition(Vector3 spawnPosition)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = new Vector3(
            spawnPosition.x,
            transform.position.y,
            spawnPosition.z
        );
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        gameObject.SetActive(false);

        Debug.Log("Player Died");

        RoundManager.instance.PlayerDied();
    }

    public void ResetPlayer()
    {
        isDead = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        gameObject.SetActive(true);
    }
}