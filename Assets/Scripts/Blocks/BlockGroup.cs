using System.Collections.Generic;

// Rename this class to Match/ColorMatch/ColorGroup
public class BlockGroup {
	List<ColorBlock> colorBlocks;

	public BlockGroup() {
		colorBlocks = new List<ColorBlock>();
	}

	public void blast() {
		foreach (ColorBlock block in colorBlocks) {
			block.gameObject.SetActive(false);
		}

		Events.getInstance().matchBlasted.Invoke(this);
	}

	public void addBlock(ColorBlock block) {
		colorBlocks.Add(block);
		block.setBlockGroup(this);

		// Retrieve these values from LevelManager or Board.
		// groupIcons(Board.getInstance().getIconThresholds());
	}

	public bool isEmpty() { return colorBlocks.Count == 0; }
	public bool contains(ColorBlock block) { return colorBlocks.Contains(block); }

	// Getters
	public List<ColorBlock> getColorBlocks() { return colorBlocks; }
}