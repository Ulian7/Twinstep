using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public PlayerControls inputActions;
    public CameraHandler cameraHandler;
    
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;
    public float lsX;
    public float lsY;
    
    public bool sprintFlag;
    public float sprintTimer;
    
    public bool b_Input;
    public bool jump_Input;
    public bool sink_Input;
    public bool float_Input;
    private Vector2 moveInput;
    private Vector2 ls_Input;
    
    private void LateUpdate()
    {
        if (cameraHandler != null)
        {
            cameraHandler.HandleCameraRotation(Time.deltaTime, mouseX, mouseY);
        }
    }
    private void OnMove(InputAction.CallbackContext i)
    {
        moveInput = i.ReadValue<Vector2>();
    }
    
    private void OnRoll(InputAction.CallbackContext i)
    {
        b_Input = true;
    }
    
    private void OnMouseX(InputAction.CallbackContext i)
    {
        mouseX = i.ReadValue<Vector2>().x;
    }
        
    private void OnMouseY(InputAction.CallbackContext i)
    {
        mouseY = i.ReadValue<Vector2>().y;
    }
    
    private void OnLS(InputAction.CallbackContext i)
    {
        ls_Input = i.ReadValue<Vector2>();
    }
    

    private void OnJump(InputAction.CallbackContext i)
    {
        jump_Input = true;
    }
    
    private void OnSink(InputAction.CallbackContext i)
    {
        sink_Input = true;
    }
    
    private void OnFloat(InputAction.CallbackContext i)
    {
        float_Input = true;
    }
    
    private void QuitSink(InputAction.CallbackContext i)
    {
        sink_Input = false;
    }
    
    private void QuitFloat(InputAction.CallbackContext i)
    {
        float_Input = false;
    }
    
    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Move.performed += OnMove;
            inputActions.PlayerActions.Roll.performed += OnRoll;
            inputActions.PlayerActions.Mouse.performed += OnMouseX;
            inputActions.PlayerActions.Mouse.performed += OnMouseY;
            inputActions.PlayerActions.Jump.performed += OnJump;
            inputActions.SoulMovement.Move.performed += OnLS;
            inputActions.SoulMovement.Sink.performed += OnSink;
            inputActions.SoulMovement.Float.performed += OnFloat;
            inputActions.SoulMovement.Sink.canceled += QuitSink;
            inputActions.SoulMovement.Float.canceled += QuitFloat;
        }

        inputActions.Enable();
    }
        
    private void OnDisable()
    {
        inputActions.Disable();
    }
    
    /*private void ClearDelegation()
    {
        inputActions.PlayerMovement.Move.performed -= OnMove;

    }
    
    private void RecoverDelegation()
    {
        inputActions.PlayerMovement.Move.performed += OnMove;
    }*/
    
    public void TickInput(float delta)
    {
        HandleMoveInput(delta);
        HandleRollInput(delta);
    }

    public void FixTickInput()
    {
        HandleFixMoveInput();
    }
    
    private void HandleMoveInput(float delta)
    {
        horizontal = moveInput.x;
        vertical = moveInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }

    private void HandleRollInput(float delta)
    {
        b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        if (b_Input)
        {
            sprintTimer += delta;
            sprintFlag = true;
        }
        else
        {
            if (moveAmount == 0)
            {
                sprintTimer = 0;
            }

            if (sprintTimer > 0.3)
            {
                sprintFlag = true;
            }
        }
    }

    private void HandleFixMoveInput()
    {
        lsX = ls_Input.x;
        lsY = ls_Input.y;
    }
    
    public void Refresh()
    {
        sprintFlag = false;
        jump_Input = false;
    }
}
