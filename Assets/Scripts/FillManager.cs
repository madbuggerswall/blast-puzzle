using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillManager : MonoBehaviour {
	void Start() {
		Events.getInstance().matchBlasted.AddListener(fill);
	}

	// TODO: Break down this method. Also filling could be migrated to FillController or something.
	void fill(BlockGroup blockGroup) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();

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
