using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public AudioSource audioSource;

    public void PlaySFX(AudioClip audioClip, float volume=1f) {
        audioSource.clip = audioClip;
        audioSource.volume = volume;

        audioSource.Play();
    }

    public bool IsPlaying() {
        return audioSource.isPlaying;
    }
}
