using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillManager : MonoBehaviour {
	// Shifting block count
	[SerializeField] int shiftingBlocks;

	void Start() {
		Events.getInstance().matchBlasted.AddListener(fill);
	}

	void fill(BlockGroup blockGroup) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		Events.getInstance().filling.Invoke();

		int minRow = blockGrid.getCellBounds().min.y;
		int maxRow = blockGrid.getCellBounds().max.y;

		foreach (int column in getAffectedColumns(blockGroup)) {
			fillColumn(column, minRow, maxRow);
		}
	}

	List<int> getAffectedColumns(BlockGroup blockGroup) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		List<int> affectedColumns = new List<int>();

		foreach (Block blastedBlock in blockGroup.getBlocks()) {
			Vector2Int cellIndex = blockGrid.worldToCell(blastedBlock.transform.position);
			if (!affectedColumns.Contains(cellIndex.x))
				affectedColumns.Add(cellIndex.x);
		}

		return affectedColumns;
	}

	void fillColumn(int column, int minRow, int maxRow) {
		int emptyCellCount = 0;
		int layerMask = LayerMask.GetMask("Block");

		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		BlockSpawner blockSpawner = LevelManager.getInstance().getBlockSpawner();

		for (int row = minRow; row < maxRow; row++) {
			Vector2 pointPos = blockGrid.cellToWorld(new Vector2Int(column, row));
			Collider2D collider = Physics2D.OverlapPoint(pointPos, layerMask);

			if (collider == null)
				emptyCellCount++;
			else if (emptyCellCount > 0) {
				collider.GetComponent<Block>().fill(emptyCellCount);
				shiftingBlocks++;
			}
		}

		// Falling blocks
		const int fallingRow = 12;
		for (int targetRow = emptyCellCount; targetRow > 0; targetRow--) {
			Vector2 spawnPosition = blockGrid.cellToWorld(new Vector2Int(column, fallingRow-targetRow));
			Block block = blockSpawner.spawnRandomBlock(spawnPosition);
			block.fall(targetRow);
			shiftingBlocks++;
		}

	}

	public void decrementShiftingBlocks() {
		shiftingBlocks--;
		if (shiftingBlocks == 0) {
			Events.getInstance().fillingDone.Invoke();
		}
	}
}
