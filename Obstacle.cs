using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (EventHandler.PLAYER_TAG == other.tag)
        {
            EventHandler.FinishGame?.Invoke(false);
        }
    }
}
