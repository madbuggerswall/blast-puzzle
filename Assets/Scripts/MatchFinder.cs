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

			BlockGroup blockGroup = new BlockGroup();
			checkBlockNeighbors(colorBlock, ref blockGroup);
			if (blockGroup.getColorBlocks().Count > 0) matchCount++;
		}
	}

	// Check for matching neighbors
	void checkBlockNeighbors(ColorBlock colorBlock, ref BlockGroup blockGroup) {
		Vector2[] directions = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

		foreach (Vector2 direction in directions) {
			// Multiply direction with grid size for correctness.
			Collider2D collider = Physics2D.OverlapPoint((Vector2) colorBlock.transform.position + direction, layerMask);
			ColorBlock nieghbor = collider?.GetComponent<ColorBlock>();

			if (colorBlock.getColor() == nieghbor?.getColor() && !blockGroup.contains(nieghbor)) {
				if (blockGroup.isEmpty())
					blockGroup.addBlock(colorBlock);

				blockGroup.addBlock(nieghbor);
				checkBlockNeighbors(nieghbor, ref blockGroup);
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
