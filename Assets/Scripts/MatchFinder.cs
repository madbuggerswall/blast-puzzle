using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour {
	[SerializeField] int matchCount;
	int layerMask;

	void Awake() {
		layerMask = LayerMask.GetMask("Block");
	}

	void Start() {
		checkBlockGroups();
		Events.getInstance().fillingDone.AddListener(checkBlockGroups);
		Events.getInstance().filling.AddListener(clearBlockGroups);
	}

	// Check for matching blocks/matching groups
	void checkBlockGroups() {
		ColorBlock[] colorBlocks = LevelManager.getInstance().getBlockSpawner().getColorBlocks();

		foreach (ColorBlock colorBlock in colorBlocks) {
			if (colorBlock.getBlockGroup() != null)
				continue;

			ColorMatch colorMatch = new ColorMatch();
			checkBlockNeighbors(colorBlock, ref colorMatch);
			if (colorMatch.getColorBlocks().Count > 0) matchCount++;
		}
	}

	// Check for matching neighbors
	void checkBlockNeighbors(ColorBlock colorBlock, ref ColorMatch colorMatch) {
		Vector2[] directions = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

		foreach (Vector2 direction in directions) {
			// Multiply direction with grid size for correctness.
			Collider2D collider = Physics2D.OverlapPoint((Vector2) colorBlock.transform.position + direction, layerMask);
			Block neighbor = collider?.GetComponent<Block>();

			if (neighbor is BlastAffected) {
				if (!colorMatch.contains((BlastAffected) neighbor))
					colorMatch.addBlock((BlastAffected) neighbor);
			} else if (neighbor is ColorBlock) {
				if (!checkMatch(colorBlock, (ColorBlock) neighbor, colorMatch))
					continue;
				
				if (colorMatch.isEmpty())
					colorMatch.addBlock(colorBlock);

				colorMatch.addBlock((ColorBlock) neighbor);
				checkBlockNeighbors((ColorBlock) neighbor, ref colorMatch);
			}
		}
	}

	bool checkMatch(ColorBlock block, ColorBlock neighbor, ColorMatch colorMatch) {
		bool colorsMatch = block.getColor() == neighbor.getColor();
		bool alreadyInMatch = colorMatch.contains(neighbor);
		return colorsMatch && !alreadyInMatch;
	}

	void clearBlockGroups() {
		matchCount = 0;

		ColorBlock[] colorBlocks = LevelManager.getInstance().getBlockSpawner().getColorBlocks();
		foreach (ColorBlock colorBlock in colorBlocks)
			colorBlock.setBlockGroup(null);
	}
}
