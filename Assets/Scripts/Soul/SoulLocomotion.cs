using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulLocomotion : MonoBehaviour
{
    public Transform cameraObject;
    public Vector3 moveDirection;
    
    public InputHandler inputHandler;
    private Rigidbody rigidbody;
    private SoulManager soulManager;

    [Header("Movement Stats")] 
    [SerializeField]
    float horizontalForce = 100;
    [SerializeField]
    float verticalForce = 100;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        soulManager = GetComponent<SoulManager>();
    }

    public void Tick()
    {
        HandleMovement();
        HandleVerticalMovement();
    }

    public void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputHandler.lsY;
        moveDirection += cameraObject.right * inputHandler.lsX;
        moveDirection.y = 0;
        moveDirection.Normalize();

        rigidbody.AddForce(moveDirection * horizontalForce, ForceMode.Force);
    }

    public void HandleVerticalMovement()
    {
        if (inputHandler.sink_Input)
        {
            rigidbody.AddForce(Vector3.down  * verticalForce, ForceMode.Force);
        }

        if (inputHandler.float_Input)
        {
            rigidbody.AddForce(Vector3.up  * verticalForce, ForceMode.Force);
        }
    }
}
