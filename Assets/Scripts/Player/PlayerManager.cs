using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputHandler inputHandler;
    private PlayerLocomotion playerLocomotion;
    private PlayerStatus playerStatus;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isGrounded;
    public bool isLanding;

    public float AirTimer;
    
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        inputHandler.TickInput(delta);
        playerLocomotion.Tick(delta) ;
        UpdateAirTimer(delta);
    }
    
    private void LateUpdate()
    {
        inputHandler.Refresh();
        if (inputHandler.sprintTimer > 0.3f)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = inputHandler.b_Input;
        }
    }

    private void UpdateAirTimer(float delta)
    {
        if (isGrounded)
        {
            AirTimer = 0;
        }
        else
        {
            AirTimer += delta;
        }
    }
}
