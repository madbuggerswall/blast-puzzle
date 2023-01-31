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
		Events.getInstance().blockBlasted.AddListener(playBlockSound);
	}

	// Randomize pitch slightly in every play
	void playAudioSource(AudioSource audioSource) {
		audioSource.pitch = Random.Range(0.96f, 1.04f);
		audioSource.Play();
	}

	// Play the corresponding sound for each block
	void playBlockSound(Block block) {
		if (block is Duck)
			playAudioSource(duck);
		else if (block is Balloon)
			playAudioSource(balloon);
		else if (block is ColorBlock)
			playAudioSource(cubeExplode);
	}
}
