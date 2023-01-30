using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : Block, IFillable, IFallable {
	void Start() {
		Events.getInstance().fillingDone.AddListener(blastAtBottom);
	}

	public override void blast() {
		gameObject.SetActive(false);
		Events.getInstance().blockBlasted.Invoke(this);
	}

	// IFallable Fall from top of the screen
	public void fall(int rowCount) {
		Vector2 target = getFallingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}

	// IFillable Fill empty spaces below
	public void fill(int rowCount) {
		Vector2 target = getFillingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}

	void blastAtBottom() {
		if (isAtTheBottom() && gameObject.activeInHierarchy) {
			blast();
		}
	}

	bool isAtTheBottom() {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		Vector2Int minCell = blockGrid.getCellBounds().min;
		Vector2Int currentCell = blockGrid.worldToCell(transform.position);
		return (currentCell.y == minCell.y);
	}
}
