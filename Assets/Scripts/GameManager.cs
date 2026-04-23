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
            // 請確保這裡填入的場景名稱與你「選單場景」的名稱完全一致（例如 "Menu"）
            SceneManager.LoadScene("Menu");
        }
    }

    public void NewGame()
    {
        SetPlayerScore(0);
        SetComputerScore(0);
        NewRound();
        if (SceneChanger.SelectedMode == "Survival Mode")
        {
            computerPaddle.transform.localScale = new Vector3(2f, 200f, 1f);
        }
        
    }

    public void NewRound()
    {
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();

        // 1. 先清除場景中殘留的球 (避免上一局的球還在跑)
        GameObject[] existingBalls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject b in existingBalls)
        {
            Destroy(b);
        }

        // 2. 延遲一秒後生成新球
        CancelInvoke();
        Invoke(nameof(StartRound), 1f);
    }

    private void StartRound()
    {
        // 呼叫你的 Spawner 根據模式生成球
        // 這裡我們需要微調一下 BallSpawner，讓它能被外部呼叫
        ballSpawner.SpawnByMode();
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

}
