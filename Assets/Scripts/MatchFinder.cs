using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bad practice only for testing
[DefaultExecutionOrder(4)]
public class MatchFinder : MonoBehaviour {
	int layerMask;

	void Awake() {
		layerMask = LayerMask.GetMask("Block");
	}

	void Start() {
		checkBlockGroups();
	}

	// Check for matching blocks/matching groups
	void checkBlockGroups() {
		foreach (Block block in getBlocks()) {
			if (block.getBlockGroup() != null)
				continue;

			BlockGroup blockGroup = new BlockGroup();
			checkBlockNeighbors(block, ref blockGroup);
			return;
		}
	}

	// Check for matching neighbors
	void checkBlockNeighbors(Block block, ref BlockGroup blockGroup) {
		Vector2[] directions = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

		foreach (Vector2 direction in directions) {
			RaycastHit2D hit = Physics2D.Raycast(block.transform.position, direction, 1f, layerMask);
			Debug.Log("Hit " + hit.collider.name);
			Block nieghbor = hit.collider.GetComponent<Block>();

			if (block.getColor() == nieghbor.getColor() && !blockGroup.contains(nieghbor)) {
				if (blockGroup.isEmpty())
					blockGroup.addBlock(block);

				blockGroup.addBlock(nieghbor);
				// checkBlockNeighbors(nieghbor, ref blockGroup);
			}
		}

	}

	Block[] getBlocks() {
		return LevelManager.getInstance().getBlockSpawner().GetComponentsInChildren<Block>();
	}
}