using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO BlockSpawner fires an event whenever grid is ready after a falling fill or an initial fill
// TODO BlockSpawner is responisble for "fall" so it should be responsible for "fill"
public class BlockSpawner : MonoBehaviour {
	[SerializeField] int colorCount = 5;

	ObjectPool objectPool;

	void Awake() {
		objectPool = GetComponentInChildren<ObjectPool>();
	}

	void Start() {
		initializeGrid();
	}

	// Fills BlockGrid with random colored blocks
	void initializeGrid() {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		(Vector2Int firstCell, Vector2Int lastCell) = blockGrid.getCellBounds();

		for (int column = firstCell.x; column < lastCell.x; column++) {
			for (int row = firstCell.y; row < lastCell.y; row++) {
				Vector2 position = blockGrid.cellToWorld(new Vector2Int(column, row));
				int sortingOrder = blockGrid.getSize().y / 2 + row;

				Block block = spawnRandomBlock(position);
				block.setSortingOrder(sortingOrder);
			}
		}
	}

	// Spawn a random block at position
	public Block spawnRandomBlock(Vector3 position) {
		Block blockPrefab = Prefabs.getInstance().getBlock(Random.Range(0, colorCount));

		Block spawnedBlock = objectPool.spawn(blockPrefab.gameObject, position).GetComponent<Block>();
		spawnedBlock.gameObject.SetActive(true);
		return spawnedBlock;
	}

	// Getters
	public ColorBlock[] getColorBlocks() { return GetComponentsInChildren<ColorBlock>(false); }
}
