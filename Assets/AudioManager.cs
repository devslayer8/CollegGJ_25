using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusicSource;
    public AudioSource gunShotSourceAttacker;
    public AudioSource gunShotSourceHealer;
    public AudioSource gameOverMusicSource;

    public AudioClip backgroundMusic;
    public AudioClip gunShotAttacker;
    public AudioClip gunShotHealer;
    public AudioClip gameOverMusic;

    void Start()
    {
        // Play background music on loop
        if (backgroundMusic != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    public void PlayGunShotSoundAttacker()
    {
        if (gunShotAttacker != null)
        {
            gunShotSourceAttacker.PlayOneShot(gunShotAttacker);
        }
    }

    public void PlayGunShotSoundHealer()
    {
        if (gunShotHealer != null)
        {
            gunShotSourceHealer.PlayOneShot(gunShotHealer);
        }
    }

    public void PlayGameOverMusic()
    {
        if (gameOverMusic != null)
        {
            gameOverMusicSource.clip = gameOverMusic;
            gameOverMusicSource.loop = false; // Play once
            gameOverMusicSource.Play();
        }
    }
}
