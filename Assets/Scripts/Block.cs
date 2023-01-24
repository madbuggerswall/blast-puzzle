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

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnMouseDown() {
		Debug.Log("Clicked on " + gameObject.name);
		blockGroup?.blast();
	}

	void fall() {

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
