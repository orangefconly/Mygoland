using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView;
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<EnemyAttackGA>(EnemyAttackPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<EnemyAttackGA>();
    }
    public void Setup(List<EnemyData>enemyDatas)
    {
        foreach (var enemyData in enemyDatas)
        {
            enemyBoardView.AddEnemy(enemyData);
        }
    }
    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {   Debug.Log("Enemy Turn");
        foreach(var enemy in enemyBoardView.EnemyViews)
        {
            EnemyAttackGA enemyAttackGA = new(enemy);
            ActionSystem.Instance.AddRection(enemyAttackGA);
        }
        Debug.Log("End Enemy Turn");
        yield return null;
    }
    private IEnumerator EnemyAttackPerformer(EnemyAttackGA enemyAttackGA)
    {
        EnemyView attacker = enemyAttackGA.Attacker;
        Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1f, 0.2f);
        yield return tween.WaitForCompletion();
        attacker.transform.DOMoveX(attacker.transform.position.x + 1f, 0.25f);
        DealDamageGA dealDamageGA = new(attacker.AttackPower, new() { HeroSystem.Instance.HeroView });
        ActionSystem.Instance.AddRection(dealDamageGA);
    }
}
