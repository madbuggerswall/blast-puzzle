using System.Collections.Generic;

// Rename this class to Match
public class BlockGroup {
	List<Block> blocks;

	public BlockGroup() {
		blocks = new List<Block>();
	}

	public void blast() {
		foreach (Block block in blocks) {
			block.gameObject.SetActive(false);
		}
	}

	public void addBlock(Block block) {
		blocks.Add(block);
		block.setBlockGroup(this);

		// Retrieve these values from LevelManager or Board.
		// groupIcons(Board.getInstance().getIconThresholds());
	}
	
	public bool isEmpty() { return blocks.Count == 0; }
	public bool contains(Block block) { return blocks.Contains(block); }
}