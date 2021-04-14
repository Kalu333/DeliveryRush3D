using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    private int coins = 0;
    [SerializeField]
    private DeliveryGoal[] delivery_goals;
    [SerializeField]
    private int delivery_goal_number = 3;

    public int Coins { get => coins; set => coins = value; }
    public DeliveryGoal[] DeliveryGoals { get => delivery_goals; }

    private void OnEnable()
    {
        reset_goals();
        EventHandler.DeliveryCollect = add_delivery_goal;
        EventHandler.DeliveryCompleted += (value) => 
        {
            DeliveryGoal dg = get_delivery_goal(value);
            if (null != dg)
            {
                dg.Completed = true;
                EventHandler.DeliveryUpdate?.Invoke();
            }
        };
        EventHandler.CoinUpdate += () => coins++;
    }

    public void reset_goals()
    {
        delivery_goals = new DeliveryGoal[delivery_goal_number];
    }

    public DeliveryGoal get_delivery_goal(DeliveryType _type)
    {
        foreach (DeliveryGoal dg in delivery_goals)
        {
            if (null != dg)
                if (_type == dg.DiliveryType)
                    return dg;
        }
        return null;
    }

    private void add_delivery_goal(DeliveryGoal _goal)
    {
        for (int index = 0; index < delivery_goals.Length; index++)
        {
            if (null == delivery_goals[index])
            {
                delivery_goals[index] = _goal;
                EventHandler.DeliveryUpdate?.Invoke();
                return;
            }
        }
    }
}
