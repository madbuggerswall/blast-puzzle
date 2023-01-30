using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketHead : MonoBehaviour {
	int layerMask;
	Vector3 initialLocalPosition;

	public enum Direction { up, right, down, left }
	[SerializeField] Direction direction;

	void Awake() {
		initialLocalPosition = transform.localPosition;
		layerMask = LayerMask.GetMask("Block");
	}
	
	void OnEnable() {
		transform.localPosition = initialLocalPosition;
	}

	void OnBecameInvisible() {
		GetComponentInParent<Rocket>().onRocketHeadLeft(direction);
	}

	public IEnumerator moveTowardsTarget() {
		const float maxDelta = 12;
		const int targetCellDistance = 20;

		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		Vector2 target = transform.position + getDirection(direction) * targetCellDistance;
		Vector2Int currentCell = blockGrid.worldToCell(transform.position);

		while ((Vector2) transform.position != target) {
			transform.position = Vector3.MoveTowards(transform.position, target, maxDelta * Time.deltaTime);
			if (currentCell != blockGrid.worldToCell(transform.position)) {
				currentCell = blockGrid.worldToCell(transform.position);
				checkCell(currentCell);
			}
			yield return null;
		}
	}

	void checkCell(Vector2Int cell) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		Collider2D collider = Physics2D.OverlapPoint(blockGrid.cellToWorld(cell), layerMask);
		Block block = collider?.GetComponent<Block>();

		if (block is not BottomBlasted && block is not null) {
			GetComponentInParent<Rocket>().addBlock(block);
			block.blast();
			Events.getInstance().blockBlasted.Invoke(block);
		}
	}

	Vector3 getDirection(Direction direction) {
		switch (direction) {
			case Direction.up:
				return Vector3.up;
			case Direction.right:
				return Vector3.right;
			case Direction.down:
				return Vector3.down;
			case Direction.left:
				return Vector3.left;
			default:
				return Vector3.zero;
		}
	}
}
