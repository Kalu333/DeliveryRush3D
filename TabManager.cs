using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    private enum SettingsState { Movement = 1, Difficulty, Sound, Camera }

    [Header("Movement")]
    [SerializeField]
    private Button movement_tab;
    [SerializeField]
    private GameObject movement_area;
    [SerializeField]
    private Slider side_movement_slider;
    [SerializeField]
    private Slider forward_movement_slider;


    [Header("Difficulty")]
    [SerializeField]
    private Button difficulty_tab;
    [SerializeField]
    private GameObject difficulty_area;
    [SerializeField]
    private Slider fix_difficulty_slider;
    [SerializeField]
    private Slider manual_obsticle_slider;

    [Header("Sound")]
    [SerializeField]
    private Button sound_tab;
    [SerializeField]
    private GameObject sound_area;
    [SerializeField]
    private Slider sound_slider;

    [Header("Camera")]
    [SerializeField]
    private Button camera_tab;
    [SerializeField]
    private GameObject camera_area;
    [SerializeField]
    private Slider forward_camera_slider;
    [SerializeField]
    private Slider height_camera_slider;
    [SerializeField]
    private Slider rotation_camera_slider;
    [SerializeField]
    private Slider fov_camera_slider;
    [SerializeField]
    private Slider smoothness_camera_slider;

    private SettingsState settings_state;

    private bool initalized;

    private void Awake()
    {
        initalized = false;
    }

    private void OnEnable()
    {
        if (!initalized)
            init_tab_ui();

        set_active_tab(SettingsState.Movement);

    }

    private void init_tab_ui()
    {
        //movement init
        movement_tab.onClick.AddListener(()=> set_active_tab(SettingsState.Movement));
        //Side
        side_movement_slider.onValueChanged.AddListener((value) => EventHandler.ChangeSideMovementSpeed?.Invoke(value));
        side_movement_slider.value = EventHandler.GetSideMovementSpeed.Invoke();
        //Forward
        forward_movement_slider.onValueChanged.AddListener((value) => EventHandler.ChangeForwardMovementSpeed?.Invoke(value));
        forward_movement_slider.value = EventHandler.GetForwardMovementSpeed.Invoke();

        //difficulty init
        difficulty_tab.onClick.AddListener(() => set_active_tab(SettingsState.Difficulty));
        manual_obsticle_slider.minValue = 0;
        manual_obsticle_slider.maxValue = GameManager.instance.Obstacles.Length;
        manual_obsticle_slider.onValueChanged.AddListener((value) => GameManager.instance.ObsticleNumber = (int)value);
        fix_difficulty_slider.onValueChanged.AddListener((value)=> {
            EventHandler.FixDifficulty((Difficulty)value);
            manual_obsticle_slider.value = GameManager.instance.ObsticleNumber;
        });
        //Set Easy difficulty by default
        fix_difficulty_slider.value = 0;
        EventHandler.FixDifficulty(Difficulty.Easy);
        manual_obsticle_slider.value = GameManager.instance.ObsticleNumber;

        //sound init
        sound_tab.onClick.AddListener(() => set_active_tab(SettingsState.Sound));
        //Sound slider
        sound_slider.minValue = -80f;
        sound_slider.maxValue = 0f;
        sound_slider.onValueChanged.AddListener((value) => EventHandler.GetMainMixer.Invoke().SetFloat("volume", value));
        float volume = 0f;
        EventHandler.GetMainMixer.Invoke().GetFloat("volume",  out volume);
        sound_slider.value = volume;

        //camera init
        camera_tab.onClick.AddListener(() => set_active_tab(SettingsState.Camera));
        //Forward
        forward_camera_slider.onValueChanged.AddListener((value) => EventHandler.ChangeCameraForward?.Invoke(value));
        forward_camera_slider.value = EventHandler.GetCameraOffset.Invoke().z;
        //Height
        height_camera_slider.onValueChanged.AddListener((value) => EventHandler.ChangeCameraHeight?.Invoke(value));
        height_camera_slider.value = EventHandler.GetCameraOffset.Invoke().y;
        //Rotation
        rotation_camera_slider.onValueChanged.AddListener((value) => EventHandler.ChangeCameraRotation?.Invoke(value));
        rotation_camera_slider.value = EventHandler.GetCameraRotation.Invoke();
        //FOV
        fov_camera_slider.onValueChanged.AddListener((value) => EventHandler.SetCameraFov?.Invoke(value));
        fov_camera_slider.value = EventHandler.GetCameraFov.Invoke();
        //Smoothness
        smoothness_camera_slider.onValueChanged.AddListener((value) => EventHandler.SetCameraSmoothness?.Invoke(value));
        smoothness_camera_slider.value = EventHandler.GetCameraSmoothness.Invoke();

        initalized = true;
    }
    
    private void set_active_tab(SettingsState _activate_state)
    {
        if (settings_state == _activate_state)
            return;
        else
            settings_state = _activate_state;

        movement_area.SetActive(false);
        difficulty_area.SetActive(false);
        sound_area.SetActive(false);
        camera_area.SetActive(false);

        switch (_activate_state)
        {
            case SettingsState.Movement:
                movement_area.SetActive(true);
                break;
            case SettingsState.Difficulty:
                difficulty_area.SetActive(true);
                break;
            case SettingsState.Sound:
                sound_area.SetActive(true);
                break;
            case SettingsState.Camera:
                camera_area.SetActive(true);
                break;
            default:
                movement_area.SetActive(true);
                break;
        }
    }

    private float scale_volume( float _to_scale )
    {
        const float SLIDER_MAX = 100f;
        const float SLIDER_MIN = 0f;
        const float MIXER_MAX = 0f;
        const float MIXER_MIN = -80f;

        return (((_to_scale - MIXER_MIN) * (SLIDER_MAX - SLIDER_MIN)) / (MIXER_MAX - MIXER_MIN)) + SLIDER_MIN;
    }



}
