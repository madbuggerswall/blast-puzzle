using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalEntryPanelUI : MonoBehaviour {
	Image blockImage;
	TMPro.TextMeshProUGUI amountText;

	void Awake() {
		blockImage = GetComponent<Image>();
		amountText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
	}

	public void initialize(Goal goal) {
		gameObject.SetActive(true);
		blockImage.sprite = goal.getBlock().GetComponent<SpriteRenderer>().sprite;
		amountText.text = goal.getAmount().ToString();
	}
}
