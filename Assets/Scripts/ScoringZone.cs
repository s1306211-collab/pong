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
            // 如果是生存模式，且這面牆的名字是 SurvivalWall，就直接返回，不重開
            if (SceneChanger.SelectedMode == "Surival Mode" && gameObject.name == "SurvivalWall")
            {
                Debug.Log("return");
                return;

            }
            else
            {
                // 否則正常重開 (玩家後方的牆會走這裡)
                Debug.Log("goal");
                scoreTrigger.Invoke();
            }
        }
    }

}