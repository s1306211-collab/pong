using UnityEngine;

public class ComputerPaddle : Paddle
{
    private Rigidbody2D targetBall;
    private Vector2 direction;

    private void Update()
    {
        if (SceneChanger.SelectedMode == "Duo")
        {
            HandleDuoInput();
        }
    }

    private void FixedUpdate()
    {
        if (SceneChanger.SelectedMode == "Duo")
        {
            HandleDuoMovement();
        }
        else
        {
            HandleAIBehavior();
        }
    }

//
// Duo
//
    private void HandleDuoInput()
    {
        if (Input.GetKey(KeyCode.UpArrow)) {
            direction = Vector2.up;
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            direction = Vector2.down;
        } else {
            direction = Vector2.zero;
        }
    }

    private void HandleDuoMovement()
    {
        if (direction.sqrMagnitude != 0) {
            rb.AddForce(direction * speed);
        }
    }


//
// AI
//
    private void HandleAIBehavior()
    {
        if (targetBall == null) {
            FindClosestBall();
            if (targetBall == null) return;
        }

        // AI 追球邏輯
        if (targetBall.velocity.x > 0f) {
            if (targetBall.position.y > rb.position.y) {
                rb.AddForce(Vector2.up * speed);
            } else {
                rb.AddForce(Vector2.down * speed);
            }
        } else {
            // 回到中間
            MoveToOrigin();
        }
    }

    private void MoveToOrigin()
    {
        if (Mathf.Abs(rb.position.y) > 0.1f) {
            Vector2 returnDir = rb.position.y > 0 ? Vector2.down : Vector2.up;
            rb.AddForce(returnDir * speed);
        }
    }

    private void FindClosestBall()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        float closestDistance = Mathf.Infinity;
        Rigidbody2D closestBall = null;

        foreach (GameObject ballObj in balls) {
            float distance = Vector2.Distance(transform.position, ballObj.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestBall = ballObj.GetComponent<Rigidbody2D>();
            }
        }
        targetBall = closestBall;
    }
}