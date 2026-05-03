using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class ScoringZone : MonoBehaviour
{
    public UnityEvent scoreTrigger;

    // ScoringZone.cs
    // ScoringZone.cs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball _))
        {
            if (SceneChanger.SelectedMode == "Survival Mode" && gameObject.name == "SurvivalWall")
            {
                Debug.Log("return");
                return;

            }
            else
            {
                if (SceneChanger.SelectedMode == "Survival Mode")
                {
                    FindObjectOfType<GameManager>().NewGame();
                    Debug.Log("你輸了!");
                }
                Debug.Log("goal");
                scoreTrigger.Invoke();
            }
        }
    }

}