using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static string SelectedMode = "Easy";

    public void ChangeScene(string modeAndScene)
    {
        string[] parts = modeAndScene.Split(',');
        SelectedMode = parts[0];
        SceneManager.LoadScene(parts[1]); 
    }
}