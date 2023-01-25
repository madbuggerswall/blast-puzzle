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

		Events.getInstance().matchBlasted.AddListener(fill);
	}

	// Fills BlockGrid with random colored blocks
	void initializeGrid() {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();

		(Vector2Int firstCell, Vector2Int lastCell) = blockGrid.getCellBounds();

		for (int column = firstCell.x; column < lastCell.x; column++) {
			for (int row = firstCell.y; row < lastCell.y; row++) {
				Vector2 position = blockGrid.cellToWorld(new Vector2Int(column, row));
				int sortingOrder = blockGrid.getSize().y + row;

				spawnRandomBlock(position, sortingOrder);
			}
		}
	}

	// Spawn a random block at position
	void spawnRandomBlock(Vector3 position, int sortingOrder) {
		Block blockPrefab = Prefabs.getInstance().getBlock(Random.Range(0, colorCount));

		Block spawnedBlock = objectPool.spawn(blockPrefab.gameObject, position).GetComponent<Block>();
		spawnedBlock.setSortingOrder(sortingOrder);
		spawnedBlock.gameObject.SetActive(true);
	}

	// TODO
	void fill(BlockGroup blockGroup) {
		int layerMask = LayerMask.GetMask("Block");

		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		List<int> columnsAffected = new List<int>();

		foreach (Block blastedBlock in blockGroup.getBlocks()) {
			Vector2Int cellIndex = blockGrid.worldToCell(blastedBlock.transform.position);
			if (!columnsAffected.Contains(cellIndex.x))
				columnsAffected.Add(cellIndex.x);
		}

		int minRow = blockGrid.getCellBounds().min.y;
		int maxRow = blockGrid.getCellBounds().max.y;

		foreach (int column in columnsAffected) {
			int emptyCellCount = 0;
			for (int row = minRow; row < maxRow; row++) {
				Vector2 pointPos = blockGrid.cellToWorld(new Vector2Int(column, row));
				Collider2D collider = Physics2D.OverlapPoint(pointPos, layerMask);

				if (collider == null)
					emptyCellCount++;
				else if (emptyCellCount > 0)
					collider.GetComponent<Block>().fill(emptyCellCount);
			}
		}
	}

	// Getters
	public Block[] getBlocks() { return GetComponentsInChildren<Block>(); }
}
