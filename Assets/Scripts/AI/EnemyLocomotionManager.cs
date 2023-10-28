using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyLocomotionManager : MonoBehaviour
{
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;
    public Rigidbody enemyRigidBody;
    [SerializeField]
    LayerMask detectionLayer;


    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        enemyRigidBody.isKinematic = false;
    }


    public void HandleMoveToTarget()
    {



    }

    private void HandleRotateTowardsTarget()
    {

    }
}
