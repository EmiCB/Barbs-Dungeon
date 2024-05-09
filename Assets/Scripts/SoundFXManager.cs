// Ref from: https://youtu.be/DU7cgVsU2rM?si=sB5BejqFq11x96HO

using UnityEngine;
using UnityEngine.Rendering;

public class SoundFXManager : MonoBehaviour {
    // make singleton -- can be called form anywhere
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake() {
        if (instance == null)  {
            instance = this;
        }
    }

    // TODO: integrate object pooling ? -- look into this
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume) {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume.Equals(volume);
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
