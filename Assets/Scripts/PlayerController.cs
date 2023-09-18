using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //Character Controller
    CharacterController characterController;
    //Animator
    [Header("Animator")]
    Animator animator;
    public bool isUsingRootMotion;
    public bool allowMovement = true;
    public bool allowRotation = true;

    //INPUT VARIABLES
    [Header("INPUTS")]
    [SerializeField] public float verticalMovement;
    [SerializeField] public float horizontalMovement;
    [SerializeField] float mouseX;
    [SerializeField] float mouseY; 

    //PLAYER VARIABLES
    [Header("Player")]
    [SerializeField] bool isDualWielding;
    [SerializeField] bool isBlocking;
    [SerializeField] bool isStrafing;
    [SerializeField] bool isSprinting;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isWalking;
    [SerializeField] bool isJumping;
    [SerializeField] float rotationSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float walkingSpeed;
    [SerializeField] float jumpHeight = 5;

    [Header("Gravity")]
    [SerializeField] Vector3 yVelocity;
    [SerializeField] float groundedYVelocity = -20;
    [SerializeField] float fallStartYVelocity = -5;
    [SerializeField] float gravityForce = -9.81f;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] LayerMask groundLayer;
    private bool fallingVelocitySet = false;
    private float inAirTimer = 0;

    [Header("Player Debug")]
    [SerializeField] public float moveAmount;
    [SerializeField] bool isPerformingAction;
    private Vector3 moveDirection;
    private Vector3 jumpDirection;

    //CAMERA VARIABLES
    [Header("Camera")]
    [SerializeField] float leftAndRightLookSpeed;
    [SerializeField] float upAndDownLookSpeed;
    [SerializeField] float minimumPivot;
    [SerializeField] float maximumPivot;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject playerCameraPivot;
    [SerializeField] Camera cameraObject;


    [Header("Camera Debug")]
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float UpAndDownLookAngle;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    //ATTACK VARIABLES
    [SerializeField] bool isLockedOn;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cameraObject = Camera.main;
    }

    #region
    private void Update()
    {
        HandleInputs();
        UpdateAnimationParameters();
        HandleAllPlayerLocomotion();

    }
    private void LateUpdate()
    {
        HandleCameraActions();
    }
    #endregion
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 moveVector = ctx.ReadValue<Vector2>();
        verticalMovement = moveVector.y;
        horizontalMovement = moveVector.x;
    }

    public void OnMouseMove(InputAction.CallbackContext ctx)
    {
        Vector2 mouseVector = ctx.ReadValue<Vector2>();
        mouseX = mouseVector.x;
        mouseY = mouseVector.y;

    }

    public void OnLightAttack(InputAction.CallbackContext ctx)
    {

    }


    private void HandleAllPlayerLocomotion()
    {
        HandleGroundCheck();
        HandlePlayerMovement();
        HandlePlayerRotation();
    }


    private void HandleWalkOrRun()
    {
        if (isSprinting || isPerformingAction)
            return;


    }
    private void HandleInputs()
    {
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalMovement) + Mathf.Abs(verticalMovement));
    }
    private void HandlePlayerRotation()
    {
        if (!allowRotation)
            return;

        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.transform.forward * verticalMovement;
        targetDirection = targetDirection + cameraObject.transform.right * horizontalMovement;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion turnRotation = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, turnRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
    private void HandlePlayerMovement()
    {
        if (!allowMovement)
            return;
        /*if (!isGrounded)
            return;
*/
        moveDirection = cameraObject.transform.forward * verticalMovement;
        moveDirection = moveDirection + cameraObject.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            moveDirection = moveDirection * sprintSpeed * Time.deltaTime;
        }

        moveDirection = moveDirection * walkingSpeed * Time.deltaTime;
        characterController.Move(moveDirection);
    }
    #region CAMERA
    private void HandleCameraActions()
    {
        HandleCameraFollowPlayer();
        HandleCameraRotate();
    }

    private void HandleCameraFollowPlayer()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(playerCamera.transform.position, transform.position, ref cameraFollowVelocity, 0.1f);
        playerCamera.transform.position = targetPosition;
    }

    private void HandleCameraRotate()
    {
        Vector3 cameraRotation;
        Quaternion targetCameraRotation;


        leftAndRightLookAngle += (mouseX * leftAndRightLookSpeed) * Time.deltaTime;
        UpAndDownLookAngle -= (mouseY * upAndDownLookSpeed) * Time.deltaTime;
        UpAndDownLookAngle = Mathf.Clamp(UpAndDownLookAngle, minimumPivot, maximumPivot);

        cameraRotation = Vector3.zero;
        cameraRotation.y = leftAndRightLookAngle;
        targetCameraRotation = Quaternion.Euler(cameraRotation);
        playerCamera.transform.rotation = targetCameraRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.x = UpAndDownLookAngle;
        targetCameraRotation = Quaternion.Euler(cameraRotation);
        playerCameraPivot.transform.localRotation = targetCameraRotation;

    }

    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckSphereRadius, groundLayer);
        if (isGrounded)
        {
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocitySet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if (!isJumping && !fallingVelocitySet)
            {
                fallingVelocitySet = true;
                yVelocity.y = fallStartYVelocity;
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            yVelocity.y += gravityForce * Time.deltaTime;
        }
        characterController.Move(yVelocity * Time.deltaTime);
    }
    #endregion
    #region ANIMATOR
    private void PlayActionAnimation(string animation, bool isPerformingAction, bool isUsingRootMotion = true, bool allowMovement = false, bool allowRotation = false)
    {
        this.isUsingRootMotion = isUsingRootMotion;
        this.allowMovement = allowMovement;
        this.allowRotation = allowRotation;
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.CrossFade(animation, 0.2f);
    }

    private void UpdateAnimationParameters()
    {
        float snappedVertical;
        float snappedHorizontal;

        #region Horizontal
        if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.5f)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.5f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion
        #region Vertical
        if (verticalMovement > 0 && verticalMovement <= 0.5f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.5f)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0 && verticalMovement >= -0.5f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.5f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if (isSprinting)
        {
            isStrafing = false;
            animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            animator.SetFloat("Vertical", 2, 0.2f, Time.deltaTime);
        }
        else
        {
            if (isStrafing)
            {

            }
            else
            {
                if (isWalking)
                {
                    animator.SetFloat("Vertical", moveAmount / 2, 0.2f, Time.deltaTime);
                    animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("Vertical", moveAmount, 0.2f, Time.deltaTime);
                    animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                }
            }
        }
        if (moveAmount == 0)
        {
            animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
        }
    }
    #endregion

}
