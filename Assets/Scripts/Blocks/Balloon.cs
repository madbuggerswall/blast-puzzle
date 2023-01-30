using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For other blast triggered blocks (Crate)
public abstract class BlastAffected : Block { }

public class Balloon : BlastAffected, IFillable, IFallable {
	// Block
	public override void blast() {
		gameObject.SetActive(false);
	}

	// Fill empty spaces below
	void IFillable.fill(int rowCount) {
		Vector2 target = getFillingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}

	// Fall from top of the screen
	void IFallable.fall(int rowCount) {
		Vector2 target = getFallingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}
}
