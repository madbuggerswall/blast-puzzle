using System.Collections.Generic;

public class ColorMatch {
	List<ColorBlock> colorBlocks;
	List<Block> blastAffectedBlocks;

	public ColorMatch() {
		colorBlocks = new List<ColorBlock>();
		blastAffectedBlocks = new List<Block>();
	}

	public void blast() {
		// Blast the color matching group
		foreach (ColorBlock block in colorBlocks) {
			block.blast();
		}

		// Blast the neighbor triggered group (balloons etc.)
		foreach (Block block in blastAffectedBlocks) {
			block.blast();
		}
		
		Events.getInstance().matchBlasted.Invoke(this);
	}

	public void addBlock(ColorBlock block) {
		colorBlocks.Add(block);
		block.setBlockGroup(this);
	}

	public void addBlock(Block block) {
		blastAffectedBlocks.Add(block);
	}

	public bool isEmpty() { return colorBlocks.Count == 0; }
	public bool contains(ColorBlock block) { return colorBlocks.Contains(block); }
	public bool contains(Block block) { return blastAffectedBlocks.Contains(block); }

	// Getters
	public List<ColorBlock> getColorBlocks() { return colorBlocks; }
	public List<Block> getAllBlocks() {
		List<Block> blocks = new List<Block>(colorBlocks);
		blocks.AddRange(blastAffectedBlocks);
		return blocks;
	}
}