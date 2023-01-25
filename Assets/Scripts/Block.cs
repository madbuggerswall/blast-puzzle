using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockColor {
	blue,
	green,
	pink,
	purple,
	red,
	yellow
}

// Make Block abstract, and colored blocks Colored Block or MatchBlock : Block
// Duck and Balloon would be inheriting from Block in that way
public class Block : MonoBehaviour {
	[SerializeField] BlockColor color;

	BlockGroup blockGroup;

	SpriteRenderer spriteRenderer;
	Rigidbody2D rigidBody;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigidBody = GetComponent<Rigidbody2D>();
	}

	void Start() {
	}

	void OnMouseDown() {
		blockGroup?.blast();
	}

	public void fill(int rowCount) {
		Vector2 target = transform.position + Vector3.down * rowCount;
		StartCoroutine(moveTowardsTarget(target));
	}

	// For smooth shifting
	IEnumerator moveTowardsTarget(Vector2 target) {
		const float maxDelta = 12;

		while (rigidBody.position != target) {
			Vector3 towards = Vector3.MoveTowards(rigidBody.position, target, maxDelta * Time.deltaTime);
			rigidBody.MovePosition(towards);
			yield return new WaitForFixedUpdate();
		}
	}


	// To avoid undesired overlappings
	public void setSortingOrder(int sortingOrder) {
		spriteRenderer ??= GetComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = sortingOrder;
	}

	// Getters & Setters
	public BlockColor getColor() { return color; }
	public BlockGroup getBlockGroup() { return blockGroup; }

	public void setBlockGroup(BlockGroup blockGroup) { this.blockGroup = blockGroup; }
}
