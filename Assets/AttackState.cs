using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        //Select one of our many attacks based on attack scores
        //If the selected attack isnot able to be used because of bad angle or distance, select a new attack
        //if the attack is viable, stop our movement and attack our target
        //Reset recovery timer to the attacks recovery timer
        //Return the combat stance state

        return this;
    }


}
