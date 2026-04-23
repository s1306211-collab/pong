using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    public float baseSpeed = 5f;
    public float maxSpeed = Mathf.Infinity;
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
        // Flip a coin to determine if the ball starts left or right
        float x = Random.value < 0.5f ? -1f : 1f;

        // Flip a coin to determine if the ball goes up or down. Set the range
        // between 0.5 -> 1.0 to ensure it does not move completely horizontal.
        float y = Random.value < 0.5f ? Random.Range(-1f, -0.5f)
                                      : Random.Range(0.5f, 1f);

        // Apply the initial force and set the current speed
        Vector2 direction = new Vector2(x, y).normalized;
        rb.AddForce(direction * baseSpeed, ForceMode2D.Impulse);
        currentSpeed = baseSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢查撞到的物件是否有 BouncySurface 組件，或者直接讓它撞到任何東西都加速
        // 這裡建議設定一個倍率，例如 1.1f 代表增加 10% 速度
        float speedMultiplier = 1.1f;

        // 更新目前的物理速度
        currentSpeed *= speedMultiplier;

        // 限制最大速度，避免球速快到穿牆
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        // 立即將新速度套用到物理引擎，確保反彈後速度立刻變快
        rb.velocity = rb.velocity.normalized * currentSpeed;
    }

    private void FixedUpdate()
    {
        // Clamp the velocity of the ball to the max speed
        Vector2 direction = rb.velocity.normalized;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        rb.velocity = direction * currentSpeed;
    }

}
