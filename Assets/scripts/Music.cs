using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

    public AudioClip[] myMusic; // declare this as Object array
    private AudioSource _audio;

    void Awake() {
        _audio = GetComponent<AudioSource>();
//        Random.InitState(System.DateTime.Now.Second);
    }

    void Start() {
        playRandomMusic();
    }

    // Update is called once per frame
    void Update() {
        if (!_audio.isPlaying)
            playRandomMusic();
    }

    void playRandomMusic() {
        _audio.clip = myMusic[Random.Range(0, myMusic.Length)];
        _audio.Play();
    }
}