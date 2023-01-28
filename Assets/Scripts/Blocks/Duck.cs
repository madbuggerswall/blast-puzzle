using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : Block {
	
	void Start() {
		Events.getInstance().fillingDone.AddListener(blastAtBottom);
	}

	void blastAtBottom() {
		if (isAtTheBottom() && gameObject.activeInHierarchy) {
			gameObject.SetActive(false);
			Events.getInstance().blockBlasted.Invoke(this);
		}
	}

	bool isAtTheBottom() {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		Vector2Int minCell = blockGrid.getCellBounds().min;
		Vector2Int currentCell = blockGrid.worldToCell(transform.position);
		return (currentCell.y == minCell.y);
	}
}
