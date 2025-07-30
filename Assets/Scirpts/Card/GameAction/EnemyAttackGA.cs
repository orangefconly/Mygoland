using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackGA : GameAction
{
    public EnemyView Attacker {  get; private set; }

    public EnemyAttackGA(EnemyView attacker)
    {
        Attacker = attacker;
    }
}
