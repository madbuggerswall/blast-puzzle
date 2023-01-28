using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	[SerializeField] AudioSource cubeExplode;
	[SerializeField] AudioSource cubeCollect;
	[SerializeField] AudioSource duck;
	[SerializeField] AudioSource balloon;

	void Start() {
		Events.getInstance().matchBlasted.AddListener(delegate { playAudioSource(cubeExplode); });
		Events.getInstance().blockHitGoal.AddListener(delegate { playAudioSource(cubeCollect); });
		Events.getInstance().duckBlasted.AddListener(delegate { playAudioSource(duck); });
		Events.getInstance().balloonBlasted.AddListener(delegate { playAudioSource(balloon); });
	}

	void playAudioSource(AudioSource audioSource) {
		audioSource.pitch = Random.Range(0.96f, 1.04f);
		audioSource.Play();
	}
}
