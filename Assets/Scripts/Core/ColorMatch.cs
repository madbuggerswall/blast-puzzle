using System.Collections.Generic;

public class ColorMatch {
	List<ColorBlock> colorBlocks;
	List<BlastAffected> blastAffectedBlocks;

	public ColorMatch() {
		colorBlocks = new List<ColorBlock>();
		blastAffectedBlocks = new List<BlastAffected>();
	}

	// Blast the color matched blocks and neighbor triggered blocks.
	public void blast() {
		foreach (ColorBlock block in colorBlocks) {
			block.blast();
		}

		foreach (Block block in blastAffectedBlocks) {
			block.blast();
			Events.getInstance().blockBlasted.Invoke(block);
		}

		Events.getInstance().matchBlasted.Invoke(this);
	}


	// ColorMatch Utilities
	public bool isEmpty() { return colorBlocks.Count == 0; }
	public bool contains(ColorBlock block) { return colorBlocks.Contains(block); }
	public bool contains(BlastAffected block) { return blastAffectedBlocks.Contains(block); }
	public void addBlock(BlastAffected block) { blastAffectedBlocks.Add(block); }
	public void addBlock(ColorBlock block) {
		colorBlocks.Add(block);
		block.setBlockGroup(this);
	}


	// Getters
	public List<ColorBlock> getColorBlocks() { return colorBlocks; }
	public List<Block> getAllBlocks() {
		List<Block> blocks = new List<Block>(colorBlocks);
		blocks.AddRange(blastAffectedBlocks);
		return blocks;
	}
}