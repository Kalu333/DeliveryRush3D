using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void Start()
    {
        EventHandler.ResetGoals += () => gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EventHandler.PLAYER_TAG == other.tag)
        {
            EventHandler.CoinUpdate?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
