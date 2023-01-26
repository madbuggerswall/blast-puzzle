using UnityEngine;

public enum BlockColor {
	blue,
	green,
	purple,
	red,
	yellow
}

public class ColorBlock : Block {
	[SerializeField] BlockColor color;

	BlockGroup blockGroup;

	// IPoolable.reset
	void OnEnable() {
		setBlockGroup(null);
	}

	void OnMouseDown() {
		blockGroup?.blast();
	}


	// Getters & Setters
	public BlockColor getColor() { return color; }
	public BlockGroup getBlockGroup() { return blockGroup; }

	public void setBlockGroup(BlockGroup blockGroup) { this.blockGroup = blockGroup; }
}