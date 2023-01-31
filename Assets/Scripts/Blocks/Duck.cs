using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For other blocks that blast at bottom. Rename this class.
public abstract class BottomBlasted : Block {
	// Blast the block at the bottom of the grid
	protected void blastAtBottom() {
		if (isAtTheBottom() && gameObject.activeInHierarchy) {
			blast();
		}
	}

	// Check if block is at the bottom of the grid
	bool isAtTheBottom() {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		Vector2Int minCell = blockGrid.getCellBounds().min;
		Vector2Int currentCell = blockGrid.worldToCell(transform.position);
		return (currentCell.y == minCell.y);
	}
}

public class Duck : BottomBlasted, IFillable, IFallable {
	void Start() {
		Events.getInstance().fillingDone.AddListener(blastAtBottom);
	}

	public override void blast() {
		gameObject.SetActive(false);
		Events.getInstance().blockBlasted.Invoke(this);
	}

	//  Fill empty spaces below
	void IFillable.fill(int rowCount) {
		Vector2 target = getFillingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}

	//  Fall from top of the screen
	void IFallable.fall(int rowCount) {
		Vector2 target = getFallingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}


}
