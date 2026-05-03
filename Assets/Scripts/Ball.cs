using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public float baseSpeed = 5f;
    public float maxSpeed = 30f; // 建議設一個上限，以免球速無限快到穿牆
    public float currentSpeed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ResetPosition()
    {
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

    // Ball.cs 中的 OnCollisionEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
{
    // 1. 取得碰撞法線並計算反射向量 (確保一定會反彈)
    Vector2 normal = collision.GetContact(0).normal;
    Vector2 reflectDir = Vector2.Reflect(rb.velocity.normalized, normal);
    currentSpeed *= 1.05f;

    //
    //survival mode
    //

    if (SceneChanger.SelectedMode == "Survival Mode") 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().AddSurvivalScore(1);
            Debug.Log("接到球！加 1 分");
        }
    }

    // 3. 統一的反彈速度處理[cite: 1]
    currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    
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