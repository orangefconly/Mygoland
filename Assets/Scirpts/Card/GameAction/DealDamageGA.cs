using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DealDamageGA : GameAction
{
    public int Amount {  get; private set; }

    public List<CombatantView> Targets {  get; set; }
    public DealDamageGA (int amount, List<CombatantView> targets)
    {
        Amount = amount;
        Targets = targets;
    }
}
