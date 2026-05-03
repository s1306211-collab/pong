using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    // 移除 [SerializeField] private Ball ball; 
    // 改為引用你的 Spawner
    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private Paddle computerPaddle;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text computerScoreText;
    [SerializeField] private GameObject survivalWall;

    private int playerScore;
    private int computerScore;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        // 按下 R 鍵重玩
        if (Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }

        // 按下 Esc 鍵回到選單
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void NewGame()
    {
        SetPlayerScore(0);
        SetComputerScore(0);
        NewRound();
        
    }

    public void NewRound()
    {
        playerPaddle.ResetPosition();

        if (SceneChanger.SelectedMode == "Survival Mode")
        {
            computerPaddle.gameObject.SetActive(false); // 隱藏電腦
            survivalWall.SetActive(true);
            if (computerScoreText != null) computerScoreText.gameObject.SetActive(false);
        }
        else
        {
            computerPaddle.gameObject.SetActive(true);  // 顯示電腦
            survivalWall.SetActive(true);
            computerPaddle.ResetPosition();
        }

        // 清除舊球並生成新球
        GameObject[] existingBalls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject b in existingBalls) { Destroy(b); }
        CancelInvoke();
        Invoke(nameof(StartRound), 1f);
    }

    private void StartRound()
    {
        // 呼叫你的 Spawner 根據模式生成球
        // 這裡我們需要微調一下 BallSpawner，讓它能被外部呼叫
        ballSpawner.SpawnByMode();
        string currentMode = SceneChanger.SelectedMode;
        Debug.Log(currentMode);
    }

    public void OnPlayerScored()
    {
        SetPlayerScore(playerScore + 1);
        NewRound();
    }

    public void OnComputerScored()
    {
        SetComputerScore(computerScore + 1);
        NewRound();
    }

    private void SetPlayerScore(int score)
    {
        playerScore = score;
        playerScoreText.text = score.ToString();
    }

    private void SetComputerScore(int score)
    {
        computerScore = score;
        computerScoreText.text = score.ToString();
    }

    public void AddSurvivalScore(int points)
    {
        playerScore += points;
        playerScoreText.text = playerScore.ToString();
    }
}
