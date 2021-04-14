using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public static class EventHandler
{
    public static readonly string PLAYER_TAG = "Player";

    //Movement
    public static Action<float> ChangeSideMovementSpeed;
    public static Func<float> GetSideMovementSpeed;
    public static Action<float> ChangeForwardMovementSpeed;
    public static Func<float> GetForwardMovementSpeed;
    //PlayerTransform
    public static Func<Transform> GetPlayerTransform;
    public static Action<Vector3> SetPlayerPosition;

    //Camera
    public static Action<float> ChangeCameraHeight;//Change camera Y position
    public static Action<float> ChangeCameraForward;//Change camera Z position
    public static Action<float> ChangeCameraRotation;//Change camera X rotation
    public static Action<float> SetCameraFov;// Change camera fov value
    public static Func<float> GetCameraFov;//Get current value of Camera fov value
    public static Func<Vector3> GetCameraOffset;//Get camera transform
    public static Func<float> GetCameraRotation;//Get camera transform
    public static Action<float> SetCameraSmoothness;
    public static Func<float> GetCameraSmoothness;
    
    //Sound
    public static Func<AudioMixer> GetMainMixer;
    public static Action<bool> MuteAudio;
    
    //Restart
    public static Action ResetParameters;

    //Inventory
    public static Action<DeliveryGoal> DeliveryCollect;
    public static Action<DeliveryType> DeliveryCompleted;
    public static Action DeliveryUpdate;
    public static Action CoinUpdate;
    public static Action ResetGoals;

    //Obstacle
    public static Action ObstacleHit;

    //ButtonControl
    public static Action StartGame;
    public static Action PauseGame;
    public static Action RestartGame;
    public static Action MainMenu;
    public static Action ResumeGame;

    //Difficulty
    public static Action<Difficulty> FixDifficulty;
    public static Action<int> ManualDifficulty;

    //GameState
    public static Action<bool> FinishGame;
}
