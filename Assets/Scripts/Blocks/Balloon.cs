using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For other blast triggered blocks (Crate)
public abstract class BlastAffected : Block { }

public class Balloon : BlastAffected, IFallable, IFillable {
	// Block
	public override void blast() {
		gameObject.SetActive(false);
		Events.getInstance().blockBlasted.Invoke(this);
	}

	// IFallable Fall from top of the screen
	public void fall(int rowCount) {
		Vector2 target = getFallingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}

	// IFillable Fill empty spaces below
	public void fill(int rowCount) {
		Vector2 target = getFillingTarget(rowCount);
		StartCoroutine(moveTowardsTarget(target));
	}
}
