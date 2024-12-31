using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static float transitionDuration = 1f; // Duration of the transition effect
    public static CanvasGroup fadeCanvasGroup;  // Optional CanvasGroup for fading
    private static bool isTransitioning = false;
    public SaveFile saveFile;

    /// <summary>
    /// Transitions to a scene by its index.
    /// </summary>
    /// <param name="sceneIndex">The index of the scene to load.</param>
    public static void LoadSceneByIndex(int sceneIndex)
    {
        if (!isTransitioning)
        {
            Instance.StartCoroutine(TransitionToScene(sceneIndex));
        }
    }

    private void Awake()
    {
        if (fadeCanvasGroup == null && GameObject.FindGameObjectWithTag("Fade Canvas"))
            fadeCanvasGroup = GameObject.FindGameObjectWithTag("Fade Canvas").GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public static void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadSceneByIndex(currentSceneIndex);
    }

    /// <summary>
    /// Loads the next scene in the build order.
    /// </summary>
    public static void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadSceneByIndex(currentSceneIndex + 1);
    }

    /// <summary>
    /// Loads the previous scene in the build order.
    /// </summary>
    public static void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadSceneByIndex(currentSceneIndex - 1);
    }

    private static IEnumerator TransitionToScene(int sceneIndex)
    {
        isTransitioning = true;

        // Fade out if a CanvasGroup is assigned
        if (fadeCanvasGroup != null)
        {
            yield return Instance.StartCoroutine(Fade(1f));
        }

        // Load the new scene
        SceneManager.LoadScene(sceneIndex);

        // Fade in after the scene is loaded
        if (fadeCanvasGroup != null)
        {
            yield return Instance.StartCoroutine(Fade(0f));
        }

        isTransitioning = false;
    }

    private static IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < transitionDuration)
        {
            timeElapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / transitionDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }

    // Ensures there is an active MonoBehaviour instance for coroutines
    private static LevelManager _instance;
    private static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("LevelManager");
                _instance = obj.AddComponent<LevelManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    private void Update()
    {
        int aliveEnemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (aliveEnemiesCount <= 0)
        {
            SaveLevelComplete((saveFile.levelsCompleted + 1)%2);
        }
    }

    public void SaveLevelComplete(int levelIndex)
    {
        saveFile.levelsCompleted = levelIndex;
    }
}
