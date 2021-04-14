using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Range(0.1f, 50f)]
    [SerializeField]
    private float movement_speed = 1f;

    private bool should_move;
    private Vector3 starting_position;

    private readonly string MOVEMENT_TRIGGER_TAG = "ObstacleTrigger";
    private readonly string OBSTACLE_TAG = "Obstacle";

    private void Awake()
    {
        should_move = false;
        starting_position = transform.position;
        EventHandler.ResetGoals += () => {
            should_move = false;
            transform.position = starting_position;
        };
    }

    private void Update()
    {
        if (should_move)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * movement_speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (MOVEMENT_TRIGGER_TAG == other.tag)
        {
            should_move = true;
        }

        if (OBSTACLE_TAG == other.tag)
        {
            should_move = false;
        }
    }
}
