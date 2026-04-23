using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public int ballCount = 1;

    // 將邏輯封裝，方便 GameManager 呼叫
    public void SpawnByMode()
    {
        if (SceneChanger.SelectedMode == "Multi-ball Mode")
        {
            ballCount = 2;
        }
        else
        {
            ballCount = 1;
        }
        SpawnMultipleBalls(ballCount);
    }

    public void SpawnMultipleBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
            newBall.GetComponent<Ball>().AddStartingForce();
        }
    }
}