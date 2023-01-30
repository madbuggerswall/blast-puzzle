using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {
	[SerializeField] List<Block> blocks;
	[SerializeField] int rocketHeadsFired;


	public void decrementRocketHeadsFired() {
		rocketHeadsFired--;
		if (rocketHeadsFired == 0) {
			Events.getInstance().powerUpBlasted.Invoke(blocks);
			blocks.Clear();
		}
	}
	public void incrementRocketHeadsFired() { rocketHeadsFired++; }
	public void addBlock(Block block) { blocks.Add(block); }
	public bool contains(Block block) { return blocks.Contains(block); }
}
