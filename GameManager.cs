using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public enum GameStatus { MainMenu, Settings, Play, Pause, Finish }
public enum Difficulty { Easy = 0, Medium, Hard }


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private AudioMixer main_mixer = null;
    [SerializeField]
    private Inventory inventory = null;
    [SerializeField]
    private GameObject obstacles_parent = null;

    private GameStatus game_status;
    private Vector3 starting_position;
    private int obstacle_number;
    private Obstacle[] obstacles = null;
    private bool audio_mute = false;

    private readonly float EASY_DIFFICULTY_PRECENTAGE = 25f;
    private readonly float MEDIUM_DIFFICULTY_PRECENTAGE = 50f;
    private readonly float HARD_DIFFICULTY_PRECENTAGE = 100f;

    public GameStatus GameStatus { get => game_status; set => game_status = value; }
    public int ObsticleNumber { get => obstacle_number; set => obstacle_number = value; }
    public bool AudioMute { get => audio_mute; set => audio_mute = value; }
    public Inventory Inventory { get => inventory; }
    public Obstacle[] Obstacles { get => obstacles; }

    private void Awake()
    {
        //Make sure that you have only one instance of GameManager
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(instance.gameObject);
                instance = this;
            }
        }
        else
        {
            instance = this;
        }

        obstacles = obstacles_parent.GetComponentsInChildren<Obstacle>();

        EventHandler.ResetGoals += inventory.reset_goals;

        EventHandler.StartGame += start_game;
        EventHandler.RestartGame += restart_game;
        EventHandler.MainMenu += menu;

        EventHandler.FixDifficulty += (value) => difficulty_setter(value);
        audio_mute = false;
    }

    private void Start()
    {
        Time.timeScale = 0;
        game_status = GameStatus.MainMenu;
        EventHandler.GetMainMixer = () => main_mixer;
        starting_position = EventHandler.GetPlayerTransform.Invoke().position;
        difficulty_setter(Difficulty.Easy);
    }

    public void start_game()
    {
        game_status = GameStatus.Play;
        obsticle_spawner();
    }

    public void restart_game()
    {
        EventHandler.SetPlayerPosition?.Invoke(starting_position);
        game_status = GameStatus.Play;
        Time.timeScale = 1;
        EventHandler.ResetGoals?.Invoke();
        obsticle_spawner();
    }

    public void menu()
    {
        EventHandler.SetPlayerPosition?.Invoke(starting_position);
        game_status = GameStatus.MainMenu;
    }

    private void obsticle_spawner()
    {
        foreach (Obstacle o in obstacles) o.gameObject.SetActive(false);

        for (int enable = 0; enable < obstacle_number; enable++)
        {
            int obstacle_index;
            do 
            {
                obstacle_index = Random.Range(0, obstacles.Length);
            } while (obstacles[obstacle_index].gameObject.activeInHierarchy);
            obstacles[obstacle_index].gameObject.SetActive(true);
        }
    }

    private void difficulty_setter(Difficulty _difficulty)
    {
        switch (_difficulty)
        {
            case Difficulty.Easy:
                obstacle_number = (int)(obstacles.Length * EASY_DIFFICULTY_PRECENTAGE / 100f);
                break;
            case Difficulty.Medium:
                obstacle_number = (int)(obstacles.Length * MEDIUM_DIFFICULTY_PRECENTAGE / 100f);
                break;
            case Difficulty.Hard:
                obstacle_number = (int)(obstacles.Length * HARD_DIFFICULTY_PRECENTAGE / 100f);
                break;
            default:
                Debug.LogError("Difficulty setter!");
                break;
        }
    }

}
