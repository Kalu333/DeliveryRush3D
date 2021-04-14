using UnityEngine;

public enum DeliveryType { BURGER = 1, TACO, PIZZA }

[RequireComponent(typeof(Collider))]
public class DeliveryGoal : MonoBehaviour
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private DeliveryType dilivery_type;
    private bool completed;

    public Sprite Icon { get => icon; }
    public DeliveryType DiliveryType { get => dilivery_type; set => dilivery_type = value; }
    public bool Completed { get => completed; set => completed = value; }

    private void Start()
    {
        completed = false;
        EventHandler.ResetGoals += () => {
            gameObject.SetActive(true);
            completed = false;
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EventHandler.PLAYER_TAG == other.tag)
        {
            EventHandler.DeliveryCollect?.Invoke(this);
            EventHandler.DeliveryUpdate?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
