using UnityEngine;
using System.Collections; // 必須引入這個才能使用 Coroutine

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public float baseSpeed = 5f;
    public float maxSpeed = 30f; // 建議設一個上限，以免球速無限快到穿牆
    public float currentSpeed { get; set; }

    [Header("隨機變大事件設定")]
    [Tooltip("觸發變大事件的目標模式名稱")]
    public string targetEventMode = "Random Event";
    [Range(0f, 1f)] public float powerUpChance = 0.25f; // 25% 機率觸發
    public float eventDuration = 8f;                  // 變大持續 4 秒
    public float bigScaleMultiplier = 10f;            // 變大為原來的幾倍

    private Vector3 originalScale;                     // 紀錄原始大小
    private bool isEventActive = false;                // 避免重複觸發導致尺寸出錯

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;          // 一開始就記住原本的大小
    }

    public void ResetPosition()
    {
        // 安全機制：被銷毀或重置時，確保協程停止並恢復大小
        StopAllCoroutines();
        transform.localScale = originalScale;
        isEventActive = false;

        rb.velocity = Vector2.zero;
        rb.position = Vector2.zero;
    }

    public void AddStartingForce()
    {
        float x = Random.value < 0.5f ? -1f : 1f;
        float y = Random.value < 0.5f ? Random.Range(-1f, -0.5f) : Random.Range(0.5f, 1f);

        Vector2 direction = new Vector2(x, y).normalized;
        currentSpeed = baseSpeed;
        rb.velocity = direction * currentSpeed; // 直接給速度
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. 取得碰撞法線並計算反射向量 (確保一定會反彈)
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 reflectDir = Vector2.Reflect(rb.velocity.normalized, normal);
        currentSpeed *= 1.05f;

        // ==========================================
        // 隨機事件模式 (Random Event Mode)
        // ==========================================
        if (SceneChanger.SelectedMode == "Random Event")
        {
            Debug.Log("aaaaaaaaaaaa");
            // 只有撞擊到玩家(或電腦)擋板，且目前不在事件狀態中，才計算機率
            if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Computer")) && !isEventActive)
            {
                if (Random.value < powerUpChance)
                {
                    StartCoroutine(BigBallRoutine());
                }
            }
        }

        // ==========================================
        // 生存模式 (Survival Mode)
        // ==========================================
        if (SceneChanger.SelectedMode == "Survival Mode")
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                FindObjectOfType<GameManager>().AddSurvivalScore(1);
                Debug.Log("接到球！加 1 分");
            }
        }

        // 3. 統一的反彈速度處理
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    }

    // 控制球變大與縮小的協程
    private IEnumerator BigBallRoutine()
    {
        isEventActive = true;
        Debug.Log($"【隨機事件】球變大了！持續 {eventDuration} 秒");

        // 變大
        transform.localScale = originalScale * bigScaleMultiplier;

        // 等待指定秒數
        yield return new WaitForSeconds(eventDuration);

        // 變回原本的大小
        transform.localScale = originalScale;
        isEventActive = false;
        Debug.Log("【隨機事件】球恢復原狀");
    }

    private void FixedUpdate()
    {
        // 確保球速始終保持在 currentSpeed，不會因為物理摩擦力變慢
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity = rb.velocity.normalized * currentSpeed;
        }
    }
}