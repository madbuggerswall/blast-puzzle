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

			// TODO Refactor this block
			if (collider?.GetComponent<Block>() is BlastAffected) {
				BlastAffected neighbor = collider?.GetComponent<BlastAffected>();
				if (!colorMatch.contains(neighbor))
					colorMatch.addBlock(neighbor);
			} else if (collider?.GetComponent<Block>() is ColorBlock) {
				ColorBlock neighbor = collider?.GetComponent<ColorBlock>();
				if (colorBlock.getColor() == neighbor?.getColor() && !colorMatch.contains(neighbor)) {
					if (colorMatch.isEmpty())
						colorMatch.addBlock(colorBlock);

					colorMatch.addBlock(neighbor);
					checkBlockNeighbors(neighbor, ref colorMatch);
				}
			}
		}
	}

	void clearBlockGroups() {
		matchCount = 0;

		ColorBlock[] colorBlocks = LevelManager.getInstance().getBlockSpawner().getColorBlocks();
		foreach (ColorBlock colorBlock in colorBlocks)
			colorBlock.setBlockGroup(null);
	}
}
