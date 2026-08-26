using UnityEngine;


public class PlayerRecorder : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerMouseAim playerMouseAim;
    [SerializeField] private Weapon weapon;

    [SerializeField] private float recordInterval = 0.02f;

    private ReplayData currentRecording;

    private float recordingTime;
    private float recordTimer;

    private bool isRecording;
    private bool shotQueued;
    private Vector3 shotDirection;
    private bool hasStartedAction;

    private void Awake()
    {
        weapon.OnShot += HandleShot;
    }

    private void OnDestroy()
    {
        weapon.OnShot -= HandleShot;
    }

    private void Start()
    {
      
    }
    private void HandleShot(Vector3 direction)
    {
        shotQueued = true;
        shotDirection = direction;
    }

    public void StartRecording()
    {
        currentRecording = new ReplayData();

        recordingTime = 0f;
        recordTimer = 0f;

        shotQueued = false;
        hasStartedAction = false;
        isRecording = true;

        currentRecording.startPosition = playerController.transform.position;

        Debug.Log(
            $"RECORDING START POSITION: {currentRecording.startPosition}"
        );

        Debug.Log("Recording started.");
    }

    private void FixedUpdate()
    {
        if (!isRecording)
            return;

        bool hasAction =
            playerController.MoveInput.sqrMagnitude > 0.001f ||
            shotQueued;

        // Ignore idle time before the player's first action
        if (!hasStartedAction)
        {
            if (!hasAction)
                return;

            hasStartedAction = true;
        }

        recordingTime += Time.fixedDeltaTime;
        recordTimer += Time.fixedDeltaTime;

        if (recordTimer >= recordInterval)
        {
            RecordFrame();
            recordTimer = 0f;
        }
    }

    private void RecordFrame()
    {
        ReplayFrame frame = new ReplayFrame();

        frame.time = recordingTime;
        frame.moveInput = playerController.MoveInput;

        if (shotQueued)
        {
            frame.shoot = true;
            frame.aimDirection = shotDirection;

            shotQueued = false;
        }
        else
        {
            frame.shoot = false;
            frame.aimDirection = playerMouseAim.AimDirection;
        }

        currentRecording.frames.Add(frame);
    }

    public ReplayData StopRecording()
    {
        if (!isRecording)
            return null;

        isRecording = false;

        currentRecording.duration = recordingTime;

        Debug.Log(
            $"Recording stopped. " +
            $"Duration: {currentRecording.duration:F2}s | " +
            $"Frames: {currentRecording.frames.Count}"
        );

        return currentRecording;
    }
}