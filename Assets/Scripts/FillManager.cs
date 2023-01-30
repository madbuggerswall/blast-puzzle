using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillManager : MonoBehaviour {
	int layerMask;
	[SerializeField] int shiftingBlocks;

	void Awake() {
		layerMask = LayerMask.GetMask("Block");
	}

	void Start() {
		Events.getInstance().matchBlasted.AddListener(fill);
		Events.getInstance().blockBlasted.AddListener(fill);
		Events.getInstance().powerUpBlasted.AddListener(fill);
	}

	// For Duck and other BottomBlasted
	void fill(Block block) {
		if (block is not BottomBlasted)
			return;
		Events.getInstance().filling.Invoke();
		fillColumn(getAffectedColumn(block));
	}

	// For ColorMatch and BlastAffected
	void fill(ColorMatch colorMatch) {
		Events.getInstance().filling.Invoke();
		foreach (int column in getAffectedColumns(colorMatch)) {
			fillColumn(column);
		}
	}

	// For blocks blasted via Rocket or PowerUps
	void fill(List<Block> blocks) {
		Events.getInstance().filling.Invoke();
		foreach (int column in getAffectedColumns(blocks)) {
			fillColumn(column);
		}
	}

	List<int> getAffectedColumns(ColorMatch colorMatch) {
		return getAffectedColumns(colorMatch.getAllBlocks());
	}

	List<int> getAffectedColumns(List<Block> blocks) {
		List<int> affectedColumns = new List<int>();

		foreach (Block block in blocks) {
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
		int emptyCellCount;

		fillEmptyCells(column, out emptyCellCount);
		dropBlocks(column, emptyCellCount);
	}

	// Shifts blocks to fill blasted cells
	void fillEmptyCells(int column, out int emptyCellCount) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();

		emptyCellCount = 0;
		(Vector2Int min, Vector2Int max) = blockGrid.getCellBounds();

		for (int row = min.y; row < max.y; row++) {
			Vector2 pointPos = blockGrid.cellToWorld(new Vector2Int(column, row));
			Collider2D collider = Physics2D.OverlapPoint(pointPos, layerMask);

			if (collider == null)
				emptyCellCount++;
			else if (emptyCellCount > 0) {
				collider.GetComponent<IFillable>().fill(emptyCellCount);
				shiftingBlocks++;
			}
		}
	}

	// Drop falling blocks to fill empty spaces on the top
	void dropBlocks(int column, int emptyCellCount) {
		const int fallingRow = 12;
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		BlockSpawner blockSpawner = LevelManager.getInstance().getBlockSpawner();

		for (int targetRow = emptyCellCount; targetRow > 0; targetRow--) {
			Vector2 spawnPosition = blockGrid.cellToWorld(new Vector2Int(column, fallingRow + emptyCellCount - targetRow));
			IFallable fallableBlock = blockSpawner.spawnRandomFallable(spawnPosition);
			fallableBlock.fall(targetRow);
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
