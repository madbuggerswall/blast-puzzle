using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Some blocks are not fillable (Crate)
public interface IFillable {
	public void fill(int rowCount);
}

// Some blocks are not fallable (Rocket)
public interface IFallable {
	public void fall(int rowCount);
}

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public abstract class Block : MonoBehaviour {
	Rigidbody2D rigidBody;

	protected virtual void Awake() {
		rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.isKinematic = true;
	}

	public abstract void blast();

	// Returns a target position for shifting cells given empty cell count
	protected Vector2 getFillingTarget(int emptyCellCount) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		Vector2Int targetCell = blockGrid.worldToCell(transform.position + Vector3.down * emptyCellCount);
		return blockGrid.cellToWorld(targetCell);
	}

	// Returns a target position for falling cells given empty cell count
	protected Vector2 getFallingTarget(int emptyCellCount) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		int maxRow = blockGrid.getCellBounds().max.y;
		Vector2Int targetCell = blockGrid.worldToCell(new Vector2(transform.position.x, maxRow - emptyCellCount));
		return blockGrid.cellToWorld(targetCell);
	}

	// Moves the rigidbody towards a target. Notifies FillManager when it reaches target
	protected IEnumerator moveTowardsTarget(Vector2 target) {
		const float maxDelta = 12;

		while (rigidBody.position != target) {
			Vector3 towards = Vector3.MoveTowards(rigidBody.position, target, maxDelta * Time.deltaTime);
			rigidBody.MovePosition(towards);
			yield return new WaitForFixedUpdate();
		}

		LevelManager.getInstance().getFillManager().decrementShiftingBlocks();
	}
}
