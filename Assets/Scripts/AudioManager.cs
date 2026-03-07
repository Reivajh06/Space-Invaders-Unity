using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioSource gameOverSource;
    
    public void Awake() {
        gameOverSource.Stop();
        if (Instance == null) Instance = this;
        
        else Destroy(gameObject);
    }

    public void PlaySFX(AudioClip audioClip, float volume=1f) {
        sfxSource.clip = audioClip;
        sfxSource.volume = volume;
        
        sfxSource.PlayOneShot(audioClip);
    }

    public void SetMusicPitch(float pitch) {
        musicSource.pitch = pitch;
    }

    public void SetGameOverMusic() {
        musicSource.Stop();
        gameOverSource.Play();
    }
}
