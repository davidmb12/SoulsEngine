using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{

    public AttackState attackState;
    public PursueTargetState pursueTargetState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        //Check for attack range
        if(enemyManager.currentRecoveryTime <=0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return attackState;
        }else if(enemyManager.distanceFromTarget> enemyManager.maximumAttackRange)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
        //potentially circle player or walk around them
        //if in attack range return attack state
        //if we are in a cool down after attacking, return this state and continue circling player
        //if the player runs out of range return the pursue target state

    }
}
