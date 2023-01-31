using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Block, IFillable {
	[SerializeField] RocketHead rocketFirst;
	[SerializeField] RocketHead rocketSecond;

	void OnMouseDown() {
		LevelManager.getInstance().getPowerUpManager().addBlock(this);
		blast();
	}

	// Fires the rocket heads when blasted
	public override void blast() {
		rocketFirst.fire();
		rocketSecond.fire();
	}

	//  Fill empty spaces below
	void IFillable.fill(int rowCount) {
		Vector2 target = getFillingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}

	// If fired rocket heads are out of screen, rocket is disabled
	public void checkRocketHeads() {
		if (!rocketFirst.isVisible && !rocketSecond.isVisible)
			gameObject.SetActive(false);
	}
}
