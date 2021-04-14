using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (EventHandler.PLAYER_TAG == other.tag)
        {
            EventHandler.FinishGame?.Invoke(true);
        }
    }
}
