using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play(); // Joue la musique au démarrage
    }

    public void StopMusic()
    {
        audioSource.Stop(); // Arrête la musique
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play(); // Joue la musique si elle n'est pas déjà en cours
        }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        audioSource.clip = newClip; // Change le clip audio
        audioSource.Play(); // Joue le nouveau clip
    }
} 