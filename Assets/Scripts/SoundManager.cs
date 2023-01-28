using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	[SerializeField] AudioClip cubeExplode;
	[SerializeField] AudioClip cubeCollect;
	[SerializeField] AudioClip duck;
	[SerializeField] AudioClip balloon;

	AudioSource audioSource;

	void Awake() {
		audioSource = GetComponent<AudioSource>();
	}

	void Start() {
		Events.getInstance().matchBlasted.AddListener(delegate (BlockGroup blockGroup) {
			playAudioClip(cubeExplode);
		});

		Events.getInstance().blockHitGoal.AddListener(delegate (GoalEntryPanelUI goalPanel) {
			playAudioClip(cubeCollect);
		});
	}

	void playAudioClip(AudioClip audioClip) {
		audioSource.pitch = Random.Range(0.96f, 1.04f);
		audioSource.clip = audioClip;
		audioSource.Play();
	}
}
