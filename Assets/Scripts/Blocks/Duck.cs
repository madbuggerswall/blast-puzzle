using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : Block {
	void Start() {
		Events.getInstance().fillingDone.AddListener(blastAtBottom);
	}

	public override void blast() {
		gameObject.SetActive(false);
		Events.getInstance().blockBlasted.Invoke(this);
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
