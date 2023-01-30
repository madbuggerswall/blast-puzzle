using System.Collections.Generic;

public class ColorMatch {
	List<ColorBlock> colorBlocks;
	List<BlastAffected> blastAffectedBlocks;

	public ColorMatch() {
		colorBlocks = new List<ColorBlock>();
		blastAffectedBlocks = new List<BlastAffected>();
	}

	public void blast() {
		// Blast the color matched blocks
		foreach (ColorBlock block in colorBlocks) {
			block.blast();
		}

		// Blast the neighbor triggered blocks (balloons etc.)
		foreach (Block block in blastAffectedBlocks) {
			block.blast();
			Events.getInstance().blockBlasted.Invoke(block);
		}

		Events.getInstance().matchBlasted.Invoke(this);
	}

	public void addBlock(ColorBlock block) {
		colorBlocks.Add(block);
		block.setBlockGroup(this);
	}

	public void addBlock(BlastAffected block) {
		blastAffectedBlocks.Add(block);
	}

	public bool isEmpty() { return colorBlocks.Count == 0; }
	public bool contains(ColorBlock block) { return colorBlocks.Contains(block); }
	public bool contains(BlastAffected block) { return blastAffectedBlocks.Contains(block); }

	// Getters
	public List<ColorBlock> getColorBlocks() { return colorBlocks; }
	public List<Block> getAllBlocks() {
		List<Block> blocks = new List<Block>(colorBlocks);
		blocks.AddRange(blastAffectedBlocks);
		return blocks;
	}
}