using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {
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
	}

	void blastParticles(BlockGroup blockGroup) {
		foreach (Block block in blockGroup.getBlocks()) {
			spawnParticlesAtBlock(block);
		}
	}

	// TODO
	void spawnParticlesAtBlock(Block block) {
		GameObject particlesPrefab = Prefabs.getInstance().getBlockParticles();
		GameObject spawnedParticles = objectPool.spawn(particlesPrefab, block.transform.position);
		ParticleSystem.MainModule mainModule = spawnedParticles.GetComponent<ParticleSystem>().main;
		mainModule.startColor = getColor(block);
	}

	Color getColor(Block block) {
		switch (block.getColor()) {
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
