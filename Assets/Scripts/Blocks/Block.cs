using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make Block abstract, and colored blocks Colored Block or MatchBlock : Block
// Duck and Balloon would be inheriting from Block in that way
// All movement related
// Fake the goal movement

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public abstract class Block : MonoBehaviour {
	SpriteRenderer spriteRenderer;
	Rigidbody2D rigidBody;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigidBody = GetComponent<Rigidbody2D>();
	}

	public abstract void blast();

	// Fall from top of the screen
	public void fall(int rowCount) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();

		int maxRow = blockGrid.getCellBounds().max.y;
		Vector2Int targetCell = blockGrid.worldToCell(new Vector2(transform.position.x, maxRow - rowCount));
		Vector2 target = blockGrid.cellToWorld(targetCell);
		int sortingOrder = blockGrid.getSize().y / 2 + maxRow - rowCount;
		spriteRenderer.sortingOrder = sortingOrder;

		StartCoroutine(moveTowardsTarget(target));
	}

	// Fill empty spaces below
	public void fill(int rowCount) {
		// Guaranteed snapping
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();

		Vector2Int targetCell = blockGrid.worldToCell(transform.position + Vector3.down * rowCount);
		Vector2 target = blockGrid.cellToWorld(targetCell);
		spriteRenderer.sortingOrder -= rowCount;

		StartCoroutine(moveTowardsTarget(target));
	}

	IEnumerator moveTowardsTarget(Vector2 target) {
		const float maxDelta = 12;

		while (rigidBody.position != target) {
			Vector3 towards = Vector3.MoveTowards(rigidBody.position, target, maxDelta * Time.deltaTime);
			rigidBody.MovePosition(towards);
			yield return new WaitForFixedUpdate();
		}

		LevelManager.getInstance().getFillManager().decrementShiftingBlocks();
	}

	// To avoid undesired overlappings
	public void setSortingOrder(int sortingOrder) {
		spriteRenderer.sortingOrder = sortingOrder;
	}

}
