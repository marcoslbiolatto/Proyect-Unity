using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceMusica;
    [SerializeField] private AudioClip menuMusic;

    void Start()
    {
        if (audioSourceMusica != null && menuMusic != null)
        {
            audioSourceMusica.clip = menuMusic;
            audioSourceMusica.loop = true;
            audioSourceMusica.playOnAwake = false;
            audioSourceMusica.volume = 0.5f;
            audioSourceMusica.Play();
        }
    }
}
