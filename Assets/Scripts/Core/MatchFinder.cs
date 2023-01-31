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
		findColorMatches();
		Events.getInstance().fillingDone.AddListener(findColorMatches);
		Events.getInstance().filling.AddListener(clearColorMatches);
	}

	// Find if there's a match for each ColorBlock
	void findColorMatches() {
		ColorBlock[] colorBlocks = LevelManager.getInstance().getBlockSpawner().getColorBlocks();

		foreach (ColorBlock colorBlock in colorBlocks) {
			if (colorBlock.getBlockGroup() != null)
				continue;

			ColorMatch colorMatch = new ColorMatch();
			checkBlockNeighbors(colorBlock, ref colorMatch);
			if (colorMatch.getColorBlocks().Count > 0) matchCount++;
		}
	}

	// Check for color matching neighbors recursively
	void checkBlockNeighbors(ColorBlock colorBlock, ref ColorMatch colorMatch) {
		Vector2[] directions = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

		foreach (Vector2 direction in directions) {
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

	// Check if colors match and neighbor isn't already in match
	bool checkMatch(ColorBlock block, ColorBlock neighbor, ColorMatch colorMatch) {
		bool colorsMatch = block.getColor() == neighbor.getColor();
		bool alreadyInMatch = colorMatch.contains(neighbor);
		return colorsMatch && !alreadyInMatch;
	}

	// Clear all matches after each blast
	void clearColorMatches() {
		matchCount = 0;

		ColorBlock[] colorBlocks = LevelManager.getInstance().getBlockSpawner().getColorBlocks();
		foreach (ColorBlock colorBlock in colorBlocks)
			colorBlock.setBlockGroup(null);
	}
}
