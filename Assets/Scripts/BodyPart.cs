using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour {
    public List<AudioClip> audioClips;
    AudioSource audioSource;
    float audioTimeout;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        audioTimeout -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        if (audioTimeout <= 0) {
            audioSource.clip = audioClips[Random.Range(0, 3)];
            audioSource.Play();
            audioTimeout = 2.0f;
        }
    }
}
