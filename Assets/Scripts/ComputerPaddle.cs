using UnityEngine;

public class ComputerPaddle : Paddle
{
    // 改成 GameObject 方便搜尋，且不需要在 Inspector 拖拽
    private Rigidbody2D targetBall;

    private void FixedUpdate()
    {
        // 1. 如果目前沒有追蹤的球，或是原本的球被銷毀了，就去找一顆新的
        if (targetBall == null)
        {
            FindClosestBall();
            if (targetBall == null) return; // 如果場景還是沒球，就先不動
        }

        // 2. 使用 targetBall 取代原本的 ball
        if (targetBall.velocity.x > 0f)
        {
            if (targetBall.position.y > rb.position.y)
            {
                rb.AddForce(Vector2.up * speed);
            }
            else if (targetBall.position.y < rb.position.y)
            {
                rb.AddForce(Vector2.down * speed);
            }
        }
        else
        {
            // 回到中間的邏輯保持不變
            if (rb.position.y > 0f)
            {
                rb.AddForce(Vector2.down * speed);
            }
            else if (rb.position.y < 0f)
            {
                rb.AddForce(Vector2.up * speed);
            }
        }
    }

    // 尋找場景中離電腦最近的一顆球
    private void FindClosestBall()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        float closestDistance = Mathf.Infinity;
        Rigidbody2D closestBall = null;

        foreach (GameObject ballObj in balls)
        {
            float distance = Vector2.Distance(transform.position, ballObj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBall = ballObj.GetComponent<Rigidbody2D>();
            }
        }
        targetBall = closestBall;
    }
}