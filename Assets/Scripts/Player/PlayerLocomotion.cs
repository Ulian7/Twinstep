using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    //[HideInInspector]
    //public AnimatorHandler animatorHandler;

    public Transform cameraObject;
    public Vector3 moveDirection;
    public float gravity = -9.8f;
    public float groundCheckOffset = 0.5f;
    public float jumpVelocity = 5f;
    public float landCheckOffset;

    private LayerMask ignoreLayer;
    private InputHandler inputHandler;
    private PlayerManager playerManager;
    private CharacterController characterController;
    private AnimatorHandler animatorHandler;
    
    public float verticalVelocity;
    public bool jumpBuffer = false;
    private float fallMultiplier = 1.5f;
    private int currentCacheIndex;
    private int CACHE_SIZE = 3;
    private float landTime = 0.15f;
    private Vector3[] velCache;
    private Vector3 averageVel;
    
    
    [Header("Movement Stats")]
    [SerializeField]
    float movementSpeed = 4;
    [SerializeField]
    float sprintSpeed = 7;
    [SerializeField]
    float rotationSpeed = 10;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerManager = GetComponent<PlayerManager>();
        characterController = GetComponent<CharacterController>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        currentCacheIndex = 0;
        velCache = new Vector3[3];
        ignoreLayer = ~(1 << 7);
    }

    Vector3 AverageVel(Vector3 newVel)
    {
        velCache[currentCacheIndex] = newVel;
        currentCacheIndex++;
        currentCacheIndex %= CACHE_SIZE;
        Vector3 average = Vector3.zero;
        foreach (Vector3 vel in velCache)
        {
            average += vel;
        }

        return average / CACHE_SIZE;
    }

    public void Tick(float delta)
    {
        CheckGround();
        HandleGravity(delta);
        HandleJump(delta);
        HandleMovement(delta);
    }

    private void CheckGround()
    {
        if (Physics.SphereCast(transform.position + (Vector3.up * groundCheckOffset), characterController.radius, Vector3.down, out RaycastHit hit, groundCheckOffset - characterController.radius + 2 * characterController.skinWidth, ignoreLayer))
        {
            playerManager.isGrounded = true;
            StartCoroutine("CoolDownJump");
        }
        else
        {
            if (verticalVelocity <= 0)
            {
                landCheckOffset =
                    -verticalVelocity * landTime + 0.5f * -gravity * fallMultiplier * landTime * landTime;
                if (Physics.SphereCast(transform.position + (Vector3.up * groundCheckOffset), characterController.radius, Vector3.down, out hit, groundCheckOffset - characterController.radius + 2 * characterController.skinWidth + landCheckOffset, ignoreLayer))
                {
                    if (playerManager.AirTimer > 0.2f)
                    {
                        playerManager.isLanding = true;
                    }
                    
                    return;
                }
            }
            playerManager.isGrounded = false;
        }
    }
    
    public void HandleGravity(float delta)
    {
        if (playerManager.isGrounded)
        {
            verticalVelocity = gravity;
        }
        else
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity += gravity * delta * fallMultiplier;
            }
            else
            {
                verticalVelocity += gravity * delta;
            }
        }
    }
    
    private void HandleRotation(float delta)
    {
        Vector3 targetDir = moveDirection;
        
        if (targetDir == Vector3.zero)
            targetDir = transform.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * delta);

        transform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.y = 0;
        moveDirection.Normalize();

        float speed = movementSpeed;

        if (playerManager.isGrounded)
        {
            if (inputHandler.sprintFlag)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;

            }
            else
            {
                moveDirection *= speed;
            }

            averageVel = AverageVel(moveDirection);
        }
        else
        {
            if (averageVel == Vector3.zero)
            {
                moveDirection *= speed;
            }
            else
            {
                moveDirection *= speed / 2f;
                moveDirection += averageVel;
            }
        }
        HandleRotation(delta);
        moveDirection.y = verticalVelocity * delta;
        characterController.Move(moveDirection);
        animatorHandler.UpdateMoveValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
    } 

    public void HandleJump(float delta)
    {
        if (playerManager.isGrounded)
        {
            animatorHandler.UpdateStatusValue(PlayerStatus.Status.Grounded);
            if (inputHandler.jump_Input || jumpBuffer)
            {
                if (playerManager.isLanding)
                {
                    jumpBuffer = true;
                }
                else
                {
                    verticalVelocity = jumpVelocity;
                    jumpBuffer = false;
                }
            }
        }
        else if (playerManager.isLanding)
        {
            animatorHandler.UpdateStatusValue(PlayerStatus.Status.Land);
        }
        else
        {
            animatorHandler.UpdateStatusValue(PlayerStatus.Status.InAir);
            animatorHandler.UpdateAirValue(verticalVelocity);
        }
    }

    IEnumerator CoolDownJump()
    {
        yield return new WaitForSeconds(0.1f);
        playerManager.isLanding = false;
    }
}
