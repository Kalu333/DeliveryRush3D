using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip coin_collect;
    [SerializeField]
    private AudioClip delivery_collected;
    [SerializeField]
    private AudioClip delivery_completed;
    [SerializeField]
    private AudioClip lvl_completed;
    [SerializeField]
    private AudioClip crash;
    [SerializeField]
    private AudioSource[] audio_source;

    private void Start()
    {
        EventHandler.CoinUpdate += () => play_audio_clip(coin_collect);
        EventHandler.DeliveryCollect += (value) => play_audio_clip(delivery_collected);
        EventHandler.DeliveryCompleted += (value) => play_audio_clip(delivery_completed);

        EventHandler.FinishGame += (value) => {
            stop_audio();
            if (value)
            {
                play_audio_clip(lvl_completed);
            }
            else
            {
                play_audio_clip(crash);
            }
        };

        EventHandler.PauseGame += stop_audio;
        EventHandler.ResumeGame += play_audio;
        EventHandler.RestartGame += play_audio;
        EventHandler.StartGame += play_audio;

        stop_audio();

        EventHandler.MuteAudio += (value) => {
            foreach (AudioSource audios in audio_source)
                audios.mute = value;
        };
    }

    private void play_audio_clip(AudioClip _ac)
    {
        audio_source[0].PlayOneShot(_ac);
    }

    private void stop_audio()
    {
        foreach (AudioSource audios in audio_source)
        {
            audios.Stop();
        }
    }

    private void play_audio()
    {
        foreach (AudioSource audios in audio_source)
        {
            audios.Play();
        }
    }
}
