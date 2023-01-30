using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketHead : MonoBehaviour {
	int layerMask;
	Vector3 initialLocalPosition;
	public bool isVisible;

	public enum Direction { up, right, down, left }
	[SerializeField] Direction direction;

	void Awake() {
		initialLocalPosition = transform.localPosition;
		layerMask = LayerMask.GetMask("Block");
	}

	void OnEnable() {
		isVisible = true;
		transform.localPosition = initialLocalPosition;
	}

	void OnBecameInvisible() {
		if (!gameObject.activeInHierarchy)
			return;
		
		isVisible = false;
		GetComponentInParent<Rocket>().checkRocketHeads();
		LevelManager.getInstance().getPowerUpManager().decrementRocketHeadsFired();
	}

	public void fire() {
		LevelManager.getInstance().getPowerUpManager().incrementRocketHeadsFired();
		StartCoroutine(moveTowardsTarget());
	}

	IEnumerator moveTowardsTarget() {
		const float maxDelta = 12;
		const int targetCellDistance = 20;

		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		Vector2 target = transform.position + getDirection(direction) * targetCellDistance;
		Vector2Int currentCell = blockGrid.worldToCell(transform.position);

		while ((Vector2) transform.position != target) {
			transform.position = Vector3.MoveTowards(transform.position, target, maxDelta * Time.deltaTime);
			Vector2Int nextCell = blockGrid.worldToCell(transform.position);
			if (currentCell != nextCell && blockGrid.isCellInBound(nextCell)) {
				currentCell = blockGrid.worldToCell(transform.position);
				checkCell(currentCell);
			}
			yield return null;
		}
	}

	void checkCell(Vector2Int cell) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		PowerUpManager powerUpManager = LevelManager.getInstance().getPowerUpManager();

		Collider2D collider = Physics2D.OverlapPoint(blockGrid.cellToWorld(cell), layerMask);
		Block block = collider?.GetComponent<Block>();

		if (block is not BottomBlasted && block is not null && !powerUpManager.contains(block)) {
			powerUpManager.addBlock(block);
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
