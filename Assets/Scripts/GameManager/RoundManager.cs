using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    [SerializeField] private Enemy enemy;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ShadowPlayer shadowPlayer;
    [SerializeField] private PlayerRecorder playerRecorder;


    [SerializeField] private float xClamp = 5f;
    [SerializeField] private float zClamp = 5f;
    [SerializeField] private float minimumDistance = 3f;
   
    private ReplayData lastRoundRecording;
    public ReplayData LastRoundRecording => lastRoundRecording;

    private int currentEnemyCount;
    private int currentRound = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartRound();
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-xClamp, xClamp);
        float z = Random.Range(-zClamp, zClamp);

        return new Vector3(x, 0f, z);
    }
    public void EnemyCount()
    {
        currentEnemyCount--;

        Debug.Log($"Enemies remaining: {currentEnemyCount}");

        if (currentEnemyCount <= 0)
        {
            CompleteRound();
        }
    }

    private void CompleteRound()
    {
        Debug.Log($"Round {currentRound} Complete");
       
        lastRoundRecording = playerRecorder.StopRecording();
        if (lastRoundRecording != null)
        {
            Debug.Log
            (
                $"Round {currentRound} recording saved: " +
                $"{lastRoundRecording.frames.Count} frames"
            );
        }
        ObjectPooling.instance.DeactivateAllBullets();

        currentRound++;
       
        StartRound();
    }

    private void StartRound()
    {
        Debug.Log($"Starting Round {currentRound}");

        currentEnemyCount = 1;

        playerController.ResetPlayerPosition();

        Vector3 enemyPosition;

        do
        {
            enemyPosition = GetRandomPosition();

        } while (
            Vector3.Distance(
                playerController.transform.position,
                enemyPosition
            ) < minimumDistance
        );

        enemy.ResetEnemyPosition(enemyPosition);

        if (currentRound > 1 && lastRoundRecording != null)
        {
            shadowPlayer.StartReplay(lastRoundRecording);
        }

        playerRecorder.StartRecording();
    }

    public void PlayerDied()
    {
        Debug.Log("Player died. Restarting round.");

        ObjectPooling.instance.DeactivateAllBullets();

      

        // We'll reset the enemy too.
        // We'll handle the shadow state here as well later.
    }
}