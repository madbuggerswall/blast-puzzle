using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mediator
public class LevelManager : MonoBehaviour {
	static LevelManager instance;

	BlockGrid blockGrid;
	BlockSpawner blockSpawner;
	FillManager fillManager;
	GoalManager goalManager;

	void Awake() {
		// GameManager
		Application.targetFrameRate = 60;

		assertSingleton();

		// Find mediated objects
		blockGrid = FindObjectOfType<BlockGrid>();
		blockSpawner = FindObjectOfType<BlockSpawner>();
		fillManager = FindObjectOfType<FillManager>();
		goalManager = FindObjectOfType<GoalManager>();
	}

	// Singleton
	public static LevelManager getInstance() { return instance; }
	void assertSingleton() { if (instance == null) { instance = this; } else { Destroy(gameObject); } }

	// Getters
	public BlockGrid getBlockGrid() { return blockGrid; }
	public BlockSpawner getBlockSpawner() { return blockSpawner; }
	public FillManager getFillManager() { return fillManager; }
	public GoalManager getGoalManager() { return goalManager; }
}
