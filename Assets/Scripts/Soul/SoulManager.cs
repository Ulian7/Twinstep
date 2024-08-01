using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    public InputHandler inputHandler;
    private SoulLocomotion soulLocomotion;

    void Start()
    {
        soulLocomotion = GetComponent<SoulLocomotion>();
    }

    private void FixedUpdate()
    {
        inputHandler.FixTickInput();
        soulLocomotion.Tick();
    }
}
