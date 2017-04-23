using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	Transform audioListener;
	Transform playerTransform;
	SoundLibrary soundLibrary;

	void Awake() {
		if(instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);

			soundLibrary = GetComponent<SoundLibrary>();
			audioListener = FindObjectOfType<AudioListener>().transform;
			playerTransform = FindObjectOfType<Player>().transform;
		}
	}

	void Update() {
		if (playerTransform != null) {
			audioListener.position = playerTransform.position;
		}
	}

	public void PlaySound(AudioClip clip, Vector3 position) {
		if(clip != null) {
			AudioSource.PlayClipAtPoint(clip, position); 
		}
	}

	public void PlaySound(string clipName , Vector3 position) {
		PlaySound(soundLibrary.GetClipFromName(clipName), position);
	}
}
