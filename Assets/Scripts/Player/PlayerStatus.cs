using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public enum Status
    {
        Grounded,
        InAir,
        Land
    }

    public Status status;
}
