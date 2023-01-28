using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : Block {
	public override void blast() {
		gameObject.SetActive(false);
		Events.getInstance().blockBlasted.Invoke(this);
	}
}
