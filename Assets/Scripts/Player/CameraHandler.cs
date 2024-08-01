using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    public Transform cameraPivotTransform;
    private InputHandler inputHandler;
    
    public static CameraHandler singleton;

    public float lookSpeed = 0.1f;
    public float pivotSpeed = 0.03f;
        
    private float lookAngle;
    private float pivotAngle;
    public float minPivot = -35;
    public float maxPivot = 35;
    
    private void Awake()
    {
        singleton = this;
        inputHandler = GameObject.Find("Player").GetComponent<InputHandler>();
        lookAngle = transform.parent.localEulerAngles.y;
        cinemachineBrain = FindObjectOfType<CinemachineBrain>().GetComponent<CinemachineBrain>();
    }
    
    public void HandleCameraRotation(float delta, float RSXInput, float RSYInput)
    {
        lookAngle += (RSXInput * lookSpeed) * delta;
        pivotAngle -= (RSYInput * pivotSpeed) * delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        Vector3 rotation = Vector3.zero; 
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation; 
    }
}
