using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {
	[SerializeField] GameObject blockParticlePrefab;
	[SerializeField] GameObject goalParticlePrefab;

	[Header("Colors")]
	[SerializeField] Color blue;
	[SerializeField] Color green;
	[SerializeField] Color purple;
	[SerializeField] Color red;
	[SerializeField] Color yellow;

	ObjectPool objectPool;

	void Awake() {
		objectPool = GetComponentInChildren<ObjectPool>();
	}

	void Start() {
		Events.getInstance().matchBlasted.AddListener(blastParticles);
		Events.getInstance().blockHitGoal.AddListener(blastParticles);
		Events.getInstance().blockBlasted.AddListener(blastParticles);
	}

	// Spawn particles for each ColorBlock in a color match
	void blastParticles(ColorMatch colorMatch) {
		foreach (ColorBlock colorBlock in colorMatch.getColorBlocks()) {
			spawnParticlesAtBlock(colorBlock);
		}
	}

	// Spawn particles for block
	void blastParticles(Block block) {
		if (block is ColorBlock)
			spawnParticlesAtBlock((ColorBlock) block);
	}

	// Spawn particles for goal progress
	void blastParticles(GoalEntryPanelUI goalPanel) {
		GameObject spawnedParticles = objectPool.spawn(goalParticlePrefab, goalPanel.transform.position);
		ParticleSystem.MainModule mainModule = spawnedParticles.GetComponent<ParticleSystem>().main;
		mainModule.startColor = Color.white;
	}

	// Spawn particles for block
	void spawnParticlesAtBlock(ColorBlock colorBlock) {
		GameObject spawnedParticles = objectPool.spawn(blockParticlePrefab, colorBlock.transform.position);
		ParticleSystem.MainModule mainModule = spawnedParticles.GetComponent<ParticleSystem>().main;
		mainModule.startColor = getColor(colorBlock);
	}

	// Enum to Color
	Color getColor(ColorBlock colorBlock) {
		switch (colorBlock.getColor()) {
			case BlockColor.blue:
				return blue;
			case BlockColor.green:
				return green;
			case BlockColor.purple:
				return purple;
			case BlockColor.red:
				return red;
			case BlockColor.yellow:
				return yellow;
			default:
				return blue;
		}
	}
}
