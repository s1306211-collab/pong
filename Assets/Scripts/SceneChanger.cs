using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static string SelectedMode = "Normal"; // 預設模式

    public void ChangeScene(string modeAndScene)
    {
        string[] parts = modeAndScene.Split(',');
        SelectedMode = parts[0];
        SceneManager.LoadScene(parts[1]); 
    }
}