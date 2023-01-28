using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Could've just include the balloons when MatchFinder searches for matches.
public class Balloon : Block {
	void Start() {
		Events.getInstance().matchBlasted.AddListener(blastNearMatch);
	}

	void blastNearMatch(BlockGroup blockGroup) {
		Vector2Int[] neighborCells = getNeighborCells();
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();

		foreach (ColorBlock block in blockGroup.getColorBlocks()) {
			Vector2Int blockCellPosition = blockGrid.worldToCell(block.transform.position);
			foreach (Vector2Int neighborCell in neighborCells) {
				if (neighborCell == blockCellPosition) {
					gameObject.SetActive(false);
					Events.getInstance().blockBlasted.Invoke(this);
				}
			}
		}
	}

	Vector2Int[] getNeighborCells() {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();

		Vector3[] directions = new Vector3[] { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
		Vector2Int[] neighborCells = new Vector2Int[4];

		for (int i = 0; i < directions.Length; i++)
			neighborCells[i] = blockGrid.worldToCell(transform.position + directions[i]);

		return neighborCells;
	}
}
