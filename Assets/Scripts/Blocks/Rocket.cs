using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Block, IFillable {
	[SerializeField] RocketHead rocketFirst;
	[SerializeField] RocketHead rocketSecond;
	[SerializeField] List<Block> blocks;

	(bool first, bool second) rocketHeadLeft;

	void OnEnable() {
		rocketHeadLeft = (false, false);
		blocks.Clear();
	}

	void OnMouseDown() {
		blast();
	}

	public override void blast() {
		Events.getInstance().filling.Invoke();
		StartCoroutine(rocketFirst.moveTowardsTarget());
		StartCoroutine(rocketSecond.moveTowardsTarget());
	}

	//  Fill empty spaces below
	void IFillable.fill(int rowCount) {
		Vector2 target = getFillingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}

	public void onRocketHeadLeft(RocketHead.Direction direction) {
		if (direction == RocketHead.Direction.up || direction == RocketHead.Direction.left)
			rocketHeadLeft.first = true;
		if (direction == RocketHead.Direction.down || direction == RocketHead.Direction.right)
			rocketHeadLeft.second = true;

		if (rocketHeadLeft.first && rocketHeadLeft.second) {
			gameObject.SetActive(false);
			addBlock(this);
			Events.getInstance().powerUpBlasted.Invoke(blocks);
		}
	}

	public void addBlock(Block block) { blocks.Add(block); }
}
