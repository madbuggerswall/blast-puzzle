using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillManager : MonoBehaviour {
	// Shifting block count
	[SerializeField] int shiftingBlocks;

	void Start() {
		Events.getInstance().matchBlasted.AddListener(fill);
		Events.getInstance().blockBlasted.AddListener(fill);
	}

	void fill(Block block) {
		Events.getInstance().filling.Invoke();
		fillColumn(getAffectedColumn(block));
	}

	void fill(BlockGroup blockGroup) {
		Events.getInstance().filling.Invoke();
		foreach (int column in getAffectedColumns(blockGroup)) {
			fillColumn(column);
		}
	}

	List<int> getAffectedColumns(BlockGroup blockGroup) {
		List<int> affectedColumns = new List<int>();

		foreach (Block block in blockGroup.getColorBlocks()) {
			int affectedColumn = getAffectedColumn(block);
			if (!affectedColumns.Contains(affectedColumn))
				affectedColumns.Add(affectedColumn);
		}

		return affectedColumns;
	}

	int getAffectedColumn(Block block) {
		return LevelManager.getInstance().getBlockGrid().worldToCell(block.transform.position).x;
	}

	void fillColumn(int column) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		BlockSpawner blockSpawner = LevelManager.getInstance().getBlockSpawner();

		int emptyCellCount = 0;
		int layerMask = LayerMask.GetMask("Block");

		int minRow = blockGrid.getCellBounds().min.y;
		int maxRow = blockGrid.getCellBounds().max.y;

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
			Vector2 spawnPosition = blockGrid.cellToWorld(new Vector2Int(column, fallingRow - targetRow));
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
