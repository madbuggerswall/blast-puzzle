using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Using ColorMatch is a bad idea
// Instead adjust filling manager
public class Rocket : Block, IFillable {
	// ColorMatch colorMatch;

	void Start() {
		Events.getInstance().fillingDone.AddListener(getBlocksAtRow);
	}

	void OnMouseDown() {
		blast();
	}

	public override void blast() { }


	// IFillable Fill empty spaces below
	public void fill(int rowCount) {
		Vector2 target = getFillingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}

	void getBlocksAtRow() {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		(Vector2Int min, Vector2Int max) bounds = blockGrid.getCellBounds();

		int row = blockGrid.worldToCell(transform.position).y;
		int layerMask = LayerMask.GetMask("Block");

		for (int column = bounds.min.x; column < bounds.max.x; column++) {
			Collider2D collider = Physics2D.OverlapPoint(blockGrid.cellToWorld(new Vector2Int(column, row)), layerMask);
			Block block = collider.GetComponent<Block>();

			if (block is Duck || block is Rocket)
				continue;
		}

	}

}
