using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {
	[SerializeField] int colorCount = 5;

	ObjectPool objectPool;

	void Awake() {
		objectPool = GetComponentInChildren<ObjectPool>();
	}

	void Start() {
		fillGrid();
	}

	// Fills BlockGrid with random colored blocks
	void fillGrid() {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();

		Vector3 gridSize = new Vector3(blockGrid.getSize().x, blockGrid.getSize().y, 0f);
		Vector3Int firstCell = blockGrid.worldToCell(blockGrid.transform.position - gridSize / 2);
		Vector3Int lastCell = blockGrid.worldToCell(blockGrid.transform.position + gridSize / 2);

		for (int posX = firstCell.x; posX < lastCell.x; posX++) {
			for (int posY = firstCell.y; posY < lastCell.y; posY++) {
				Vector3 position = blockGrid.cellToWorld(new Vector3Int(posX, posY, 0));
				int sortingOrder = blockGrid.getSize().y + posY;
				
				spawnRandomBlock(position, sortingOrder);
			}
		}
	}

	// Spawn a random block at position
	public void spawnRandomBlock(Vector3 position, int sortingOrder) {
		Block blockPrefab = Prefabs.getInstance().getBlock(Random.Range(0, colorCount));

		Block spawnedBlock = objectPool.spawn(blockPrefab.gameObject, position).GetComponent<Block>();
		spawnedBlock.setSortingOrder(sortingOrder);
		spawnedBlock.gameObject.SetActive(true);
	}
}
