using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void Onclick()
    {
        EnemyTurnGA enemyTurnGA = new EnemyTurnGA();
        ActionSystem.Instance.Perform(enemyTurnGA);
    }
}
