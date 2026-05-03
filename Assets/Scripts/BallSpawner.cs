using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public int ballCount = 1;

    // �N�޿�ʸˡA��K GameManager �I�s
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