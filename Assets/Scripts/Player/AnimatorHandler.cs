using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : AnimatorManager
{
    private PlayerManager playerManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerStatus playerStatus;

    private int vertical;
    private int horizontal;
    private int verticalSpeed;
    private int status;
    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        playerStatus = GetComponentInParent<PlayerStatus>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
        verticalSpeed = Animator.StringToHash("VerticalSpeed");
        status = Animator.StringToHash("Status");
    }
    
    public void UpdateMoveValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        float v = 0;

        if (verticalMovement > 0)
        {
            v = 0.5f;
        }
        else if (verticalMovement < 0)
        {
            v = -0.5f;
        }
            
        float h = 0;

        if (horizontalMovement > 0)
        {
            h = 0.5f;
        }
        else if (horizontalMovement < 0)
        {
            h = -0.5f;
        }

        if (isSprinting && verticalMovement != 0)
        {
            v = 1;
            h = horizontalMovement;
        }
        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void UpdateAirValue(float verticalSpeed)
    {
        anim.SetFloat(this.verticalSpeed, verticalSpeed);
    }

    public void UpdateStatusValue(PlayerStatus.Status status)
    {
        anim.SetInteger(this.status, (int)status);
        playerStatus.status = status;
    }
    
}
