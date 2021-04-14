using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CompleteDelivery : MonoBehaviour
{
    [SerializeField]
    private DeliveryType required_type;

    private void Awake()
    {
        EventHandler.ResetGoals += () => gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EventHandler.PLAYER_TAG == other.tag)
        {
            EventHandler.DeliveryCompleted?.Invoke(required_type);
        }
    }
}
