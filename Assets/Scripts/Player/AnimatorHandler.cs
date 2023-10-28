using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : AnimatorManager
{
    PlayerManager playerManager;
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;

    int vertical;
    int horizontal;
    public bool canRotate;


    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");

    }

    public void UpdateAnimatorValue(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;
        if (verticalMovement > 0 && verticalMovement <= 0.5f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.5f && verticalMovement <= 1f)
        {
            v = 1;
        }
        else if (verticalMovement < 0f && verticalMovement > -0.5f)
        {
            v = -0.5f;
        }
        else if (verticalMovement <= -0.5f)
        {
            v = -1;
        }
        #endregion
        #region Horizontal
        float h = 0;
        if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.5f && horizontalMovement <= 1f)
        {
            h = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.5f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement <= -0.5f)
        {
            h = -1;
        }
        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }


    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }
    private void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false)
            return;
        float delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;

    }
}