using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    [SerializeField] private Enemy enemy;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ShadowPlayer[] shadowPlayers;
    [SerializeField] private PlayerRecorder playerRecorder;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform enemySpawn;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private int maxShadows = 10;
    private int MaxRounds => maxShadows + 1;



    [SerializeField] private float xClamp = 5f;
    [SerializeField] private float zClamp = 5f;
    [SerializeField] private float minimumDistance = 3f;

    private List<ReplayData> replayDataList = new List<ReplayData>();
    

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

        ReplayData recording = playerRecorder.StopRecording();

        if (recording != null)
        {
            replayDataList.Add(recording);

            Debug.Log(
                $"Round {currentRound} recording saved. " +
                $"Total recordings: {replayDataList.Count}"
            );
        }
        ObjectPooling.instance.DeactivateAllBullets();

        if (currentRound >= MaxRounds)
        {
            foreach (ShadowPlayer shadow in shadowPlayers)
            {
                shadow.StopReplay();
            }

            ObjectPooling.instance.DeactivateAllBullets();

            uiManager.WinUI();
            return;
        }
        currentRound++;
       
        StartRound();
    }

    private void StartRound()
    {
        roundText.text = $"Round: {currentRound} / {MaxRounds}";
        Debug.Log($"Starting Round {currentRound}");

        currentEnemyCount = 1;

        // ROUND 1 → fixed positions
        if (currentRound == 1)
        {
            playerController.ResetPlayerPosition(playerSpawn.position);
            enemy.ResetEnemyPosition(enemySpawn.position, playerController.transform.position);
        }
        // ROUND 2+ → random positions
        else
        {
            Vector3 playerPosition = GetRandomPosition();

            playerController.ResetPlayerPosition(playerPosition);

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

            enemy.ResetEnemyPosition(enemyPosition, playerController.transform.position);

        }

        int shadowsToActivate = currentRound - 1;

        for (int i = 0; i < shadowsToActivate; i++)
        {
            if (i >= shadowPlayers.Length)
                break;

            if (i >= replayDataList.Count)
                break;

            Debug.Log($"Shadow {i + 1} using recording {i + 1}");

            shadowPlayers[i].StartReplay(replayDataList[i]);
        }

        playerRecorder.StartRecording();
    }

    public void PlayerDied()
    {
        Debug.Log("Player died. Game Over.");

        playerRecorder.StopRecording();

        ObjectPooling.instance.DeactivateAllBullets();

        foreach (ShadowPlayer shadow in shadowPlayers)
        {
            shadow.StopReplay();
        }

        uiManager.GameOver();
    }
    public void RestartGame()
    {
        ObjectPooling.instance.DeactivateAllBullets();

        // Deactivate all shadows
        foreach (ShadowPlayer shadow in shadowPlayers)
        {
            shadow.gameObject.SetActive(false);
        }

        // Clear previous round recordings
        replayDataList.Clear();

        // Reset round
        currentRound = 1;
        currentEnemyCount = 1;

        // Reset player state
        playerController.gameObject.SetActive(true);

        // We need to clear the dead state
        playerController.ResetPlayer();

        StartRound();
    }
}