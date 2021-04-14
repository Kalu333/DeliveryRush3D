using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent (typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    [SerializeField]
    [Range(1f,20f)]
    private float side_movement_speed = 5f;
    [SerializeField]
    [Range(1f, 20f)]
    private float forward_movement_speed = 5f;
    [SerializeField]
    [Range(-20f, 0f)]
    private float gravity_value = -1f;
    [SerializeField]
    [Range(0, 1f)]
    private float gravity_check_distance = 0.4f;
    [SerializeField]
    private LayerMask ground_mask;

    //Touch movement input
    private Vector2 touch_point;
    private Vector2 touch_point_start;
    private bool touch_detect;
    private Touch move_touch;
    private Vector2 direction;

    //CharacterController helper
    private CharacterController player_controller;

    //Gravity velocity
    private Vector3 gravity_velocity;

    //Animation
    private Animator player_animator;
    /// <summary>
    /// -1  -> LEFT
    ///  0  -> FORWARD
    ///  1  -> RIGHT
    /// </summary>
    private readonly string ANIMATION_STEERING = "steering";
    private readonly float ANIMATION_FORWARD = 0f;

    public float GetSideMovementSpeed { get => side_movement_speed; }

    private void Awake()
    {
        player_controller = GetComponent<CharacterController>();
        player_animator = GetComponent<Animator>();
        player_animator.SetFloat(ANIMATION_STEERING, ANIMATION_FORWARD);

        EventHandler.ChangeSideMovementSpeed = (value) => { side_movement_speed = value; };
        EventHandler.GetSideMovementSpeed = () => side_movement_speed;
        EventHandler.ChangeForwardMovementSpeed = (value) => { forward_movement_speed = value; };
        EventHandler.GetForwardMovementSpeed = () => forward_movement_speed;

        EventHandler.GetPlayerTransform = () => transform;
        EventHandler.SetPlayerPosition = (value) =>
        {
            player_controller.enabled = false;
            transform.position = value;
            player_controller.enabled = true;
        };
    }

    private void Update()
    {
        check_touch();

        if (touch_detect)
        {
            foreach (Touch t in Input.touches)
            {
                if (t.fingerId == move_touch.fingerId)
                {
                    touch_point = t.position;
                }
            }
            move(touch_point);
        }
        else
        {
            //Stop movement
            move(Vector2.zero);
        }

        gravity();
    }

    private void move(Vector2 _drag)
    {
        Vector3 player_movement;

        if (Vector2.zero != _drag)
        {
            direction = _drag - touch_point_start;
            direction = Vector2.ClampMagnitude(direction, 100f);
            direction /= 100;
            player_movement = Vector3.right * direction.x * side_movement_speed;

            player_animator.SetFloat(ANIMATION_STEERING, direction.x);

            player_movement += Vector3.forward * forward_movement_speed;
        }
        else
        {
            player_animator.SetFloat(ANIMATION_STEERING, 0);
            player_movement = Vector3.forward * forward_movement_speed;
        }

        player_movement *= Time.deltaTime;
        player_controller.Move(player_movement);
    }

    private void check_touch()
    {
        if (Input.touchCount > 0 && !touch_detect)
        {
            for (int index = 0; index < Input.touchCount; index++)
            {
                move_touch = Input.touches[index];
                if (!EventSystem.current.IsPointerOverGameObject(move_touch.fingerId))
                {
                    touch_point_start = move_touch.position;
                    touch_detect = true;
                }
            }
        }
        else if (Input.touchCount <= 0)
        {
            touch_detect = false;
        }
        else if (touch_detect)
        {
            touch_detect = false;
            foreach (Touch t in Input.touches)
            {
                if (t.fingerId == move_touch.fingerId)
                {
                    touch_detect = true;
                    break;
                }
            }
        }
    }

    private void gravity()
    {
        if (Physics.CheckSphere(transform.position - (Vector3.up), gravity_check_distance, ground_mask) && gravity_velocity.y < 0)
        {
            gravity_velocity.y = -.1f;
        }

        gravity_velocity.y += gravity_value * Time.deltaTime * Time.deltaTime;

        player_controller.Move(gravity_velocity);
    }
}
