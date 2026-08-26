using UnityEngine;

public class ShadowPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform shootPoint;

    private Rigidbody rb;

    private ReplayData replayData;

    
    private bool isReplaying;
    private float replayTime;
    private int nextFrameIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StartReplay(ReplayData data)
    {
        if (data == null || data.frames.Count == 0)
        {
            Debug.LogWarning("No replay data available.");
            return;
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        replayData = data;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        rb.position = data.startPosition;
        rb.rotation = Quaternion.identity;

        nextFrameIndex = 0;
        replayTime = 0f;
        isReplaying = true;

        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (!isReplaying)
            return;

        replayTime += Time.fixedDeltaTime;

        while (
            nextFrameIndex < replayData.frames.Count &&
            replayData.frames[nextFrameIndex].time <= replayTime
        )
        {
            ReplayFrame frame = replayData.frames[nextFrameIndex];

            ApplyFrame(frame);

            nextFrameIndex++;
        }

        if (replayTime > replayData.duration)
        {
            StopReplay();
            return;
        }
    }
    private void ApplyFrame(ReplayFrame frame)
    {
        // Movement
        Vector3 move = new Vector3(
            frame.moveInput.x,
            0f,
            frame.moveInput.y
        );

        rb.linearVelocity = move * moveSpeed;

        // Aim
        Vector3 aimDirection = frame.aimDirection;
        aimDirection.y = 0f;

        if (aimDirection.sqrMagnitude > 0.001f)
        {
            rb.MoveRotation(Quaternion.LookRotation(aimDirection));
        }

        // Shoot
        if (frame.shoot)
        {
            Shoot(frame.aimDirection);
        }
    }

    private void Shoot(Vector3 shootDirection)
    {
        GameObject bulletObject = ObjectPooling.instance.GetShadowBullet();

        if (bulletObject == null)
        {
            Debug.Log("No available bullet for shadow.");
            return;
        }

        bulletObject.transform.position = shootPoint.position;
        bulletObject.SetActive(true);

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        bullet.Initialize(shootDirection);
    }

    private void StopReplay()
    {
        isReplaying = false;

        rb.linearVelocity = Vector3.zero;

        Debug.Log("Shadow replay finished.");
    }
}