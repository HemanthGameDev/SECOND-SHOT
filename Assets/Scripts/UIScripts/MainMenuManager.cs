using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject[] tutorialCameras;

    [Header("UI")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject[] tutorialUI;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Scene")]
    [SerializeField] private string gameplaySceneName = "Gameplay";

    private int currentTutorialIndex = -1;
    private bool isTransitioning;

    private void Start()
    {
        // Main menu state
        mainCamera.SetActive(true);
        mainMenuUI.SetActive(true);

        // Disable all tutorial cameras and UI
        for (int i = 0; i < tutorialCameras.Length; i++)
        {
            tutorialCameras[i].SetActive(false);
        }

        for (int i = 0; i < tutorialUI.Length; i++)
        {
            tutorialUI[i].SetActive(false);
        }

        // Start with no fade
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    // PLAY BUTTON
    public void Play()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ShowTutorial(0));
    }

    // NEXT BUTTON
    public void NextTutorial()
    {
        if (isTransitioning)
            return;

        int nextIndex = currentTutorialIndex + 1;

        if (nextIndex >= tutorialCameras.Length ||
            nextIndex >= tutorialUI.Length)
        {
            return;
        }

        StartCoroutine(ShowTutorial(nextIndex));
    }

    private IEnumerator ShowTutorial(int tutorialIndex)
    {
        isTransitioning = true;

        // Fade OUT to black
        yield return StartCoroutine(Fade(1f));

        // Disable current camera and UI
        if (currentTutorialIndex >= 0)
        {
            tutorialCameras[currentTutorialIndex].SetActive(false);
            tutorialUI[currentTutorialIndex].SetActive(false);
        }
        else
        {
            // First tutorial → disable main menu
            mainCamera.SetActive(false);
            mainMenuUI.SetActive(false);
        }

        // Enable new tutorial camera and UI
        tutorialCameras[tutorialIndex].SetActive(true);
        tutorialUI[tutorialIndex].SetActive(true);

        currentTutorialIndex = tutorialIndex;

        // Fade IN
        yield return StartCoroutine(Fade(0f));

        isTransitioning = false;
    }

    // START BUTTON ON TUTORIAL 4
    public void StartGame()
    {
        if (isTransitioning)
            return;

        StartCoroutine(LoadGameplayScene());
    }

    private IEnumerator LoadGameplayScene()
    {
        isTransitioning = true;

        // Fade to black
        yield return StartCoroutine(Fade(1f));

        // Load gameplay scene
        SceneManager.LoadScene(gameplaySceneName);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeDuration;

            fadeCanvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                t
            );

            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }

    // QUIT BUTTON
    public void Quit()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}