using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        this.transform.position = player.position;
    }
}
