using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SaveFile saveFile;
    
    public void OnButtonStart()
    {
        Debug.Log("Start");
        LevelManager.LoadSceneByIndex(saveFile.levelsCompleted+1);
    }

    public static void OnButtonQuit()
    {
        // Quits the application
        Application.Quit();

        // For testing in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
