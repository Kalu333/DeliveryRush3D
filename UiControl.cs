using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    private GameObject settings_panel;
    [SerializeField]
    private GameObject stop_panel;
    [SerializeField]
    private GameObject game_panel;
    [SerializeField]
    private GameObject finish_panel;
    [Header("Game Panel")]
    [SerializeField]
    private Text coins_text;
    [SerializeField]
    private Image[] delivery_img;
    [SerializeField]
    private Sprite completed_delivery;
    [SerializeField]
    private Sprite empty_slot;
    [Header("Finish Panel")]
    [SerializeField]
    private Text finish_text;
    [SerializeField]
    private Image[] finish_img;
    [Header("Finish game texts")]
    [SerializeField]
    private string complete_lvl;
    [SerializeField]
    private string fail_lvl;
    [Header("Sound sprites")]
    [SerializeField]
    private Sprite sound_on;
    [SerializeField]
    private Sprite sound_mute;
    [Header("Control Buttons")]
    [SerializeField]
    private Button start_button;
    [SerializeField]
    private Button pause_button;
    [SerializeField]
    private Button[] settings_button;
    [SerializeField]
    private Button[] menu_button;
    [SerializeField]
    private Button[] restart_button;
    [SerializeField]
    private Button resume_button;
    [SerializeField]
    private Button mute_button;
    [SerializeField]
    private Button close_settings_button;


    private void Start()
    {
        EventHandler.CoinUpdate += () => coins_text.text = GameManager.instance.Inventory.Coins.ToString();
        EventHandler.DeliveryUpdate = update_delivery;
        coins_text.text = GameManager.instance.Inventory.Coins.ToString();
        EventHandler.ResetGoals += reset_delivery_goals;

        EventHandler.StartGame += start_game;
        EventHandler.RestartGame += restart;
        EventHandler.PauseGame += pause_function;
        EventHandler.MainMenu += menu;
        EventHandler.ResumeGame += resume_game;

        EventHandler.FinishGame += finish_game;

        start_button.onClick.AddListener(EventHandler.StartGame.Invoke);
        pause_button.onClick.AddListener(EventHandler.PauseGame.Invoke);
        resume_button.onClick.AddListener(EventHandler.ResumeGame.Invoke);
        mute_button.onClick.AddListener(toggle_sound);
        if (null != settings_button)
        {
            foreach (Button b in settings_button)
            {
                b.onClick.AddListener(settings);
            }
            close_settings_button.onClick.AddListener(settings);
        }
        if (null != menu_button)
        {
            foreach (Button b in menu_button)
            {
                b.onClick.AddListener(EventHandler.MainMenu.Invoke);
            }
        }
        if (null != restart_button)
        {
            foreach (Button b in restart_button)
            {
                b.onClick.AddListener(EventHandler.RestartGame.Invoke);
            }
        }
        game_panel.SetActive(false);
        settings_panel.SetActive(false);
        stop_panel.SetActive(false);
        finish_panel.SetActive(false);
    }

    private void start_game()
    {
        Time.timeScale = 1f;
        settings_panel.SetActive(false);
        stop_panel.SetActive(false);
        start_button.gameObject.SetActive(false);
        finish_panel.SetActive(false);
        EventHandler.ResetGoals?.Invoke();
        game_panel.SetActive(true);
    }

    private void settings()
    {
        bool active_setting = !settings_panel.activeInHierarchy;
        settings_panel.SetActive(active_setting);

        enable_panel(!settings_panel.activeInHierarchy);

        if (settings_panel.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
        else if (GameStatus.Play == GameManager.instance.GameStatus)
        {
            Time.timeScale = 1;
        }
    }

    private void pause_function()
    {
        if (((GameStatus.Play == GameManager.instance.GameStatus) ||
            (GameStatus.Pause == GameManager.instance.GameStatus)) && 
            (!settings_panel.activeInHierarchy))
        {
            stop_panel.SetActive(!stop_panel.activeInHierarchy);

            if (stop_panel.activeInHierarchy)
            {
                pause_game();
            }
            else
            {
                EventHandler.ResumeGame?.Invoke();
            }
        }
    }

    private void pause_game()
    {
        stop_panel.SetActive(true);
        Time.timeScale = 0;
        GameManager.instance.GameStatus = GameStatus.Pause;
    }

    private void resume_game()
    {
        stop_panel.SetActive(false);
        Time.timeScale = 1;
        GameManager.instance.GameStatus = GameStatus.Play;
    }

    private void restart()
    {
        stop_panel.SetActive(false);
        finish_panel.SetActive(false);
        EventHandler.ResetGoals?.Invoke();
    }

    private void menu()
    {
        start_button.gameObject.SetActive(true);
        stop_panel.SetActive(false);
        finish_panel.SetActive(false);
        game_panel.SetActive(false);
        Time.timeScale = 0;
    }

    private void update_delivery()
    {
        for (int index = 0; index < GameManager.instance.Inventory.DeliveryGoals.Length; index++)
        {
            if (null != GameManager.instance.Inventory.DeliveryGoals[index])
            {
                if (GameManager.instance.Inventory.DeliveryGoals[index].Completed)
                {
                    delivery_img[index].sprite = completed_delivery;
                }
                else
                {
                    delivery_img[index].sprite = GameManager.instance.Inventory.DeliveryGoals[index].Icon;
                }
            }
        }
    }

    private void reset_delivery_goals()
    {
        foreach (Image i in delivery_img)
        {
            i.sprite = empty_slot;
        }
    }

    private void toggle_sound()
    {
        GameManager.instance.AudioMute = !GameManager.instance.AudioMute;
        EventHandler.MuteAudio?.Invoke(GameManager.instance.AudioMute);

        if (GameManager.instance.AudioMute)
        {
            mute_button.image.sprite = sound_mute;
        }
        else
        {
            mute_button.image.sprite = sound_on;
        }
    }

    private void finish_game(bool _finished)
    {
        if (_finished)
        {
            finish_text.text = complete_lvl;
        }
        else
        {
            finish_text.text = fail_lvl;
        }

        for (int index = 0; index < delivery_img.Length; index++)
        {
            finish_img[index].sprite = delivery_img[index].sprite;
        }
        finish_panel.SetActive(true);
        GameManager.instance.GameStatus = GameStatus.Finish;
        Time.timeScale = 0f;
    }

    private void enable_panel(bool _enable)
    {
        switch (GameManager.instance.GameStatus)
        {
            case GameStatus.Finish:
                finish_panel.SetActive(_enable);
                break;
            case GameStatus.Pause:
                stop_panel.SetActive(_enable);
                break;
            default:
                break;
        }
    }
}
