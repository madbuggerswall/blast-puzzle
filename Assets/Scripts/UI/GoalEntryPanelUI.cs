using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalEntryPanelUI : MonoBehaviour {
	Image blockImage;
	TMPro.TextMeshProUGUI amountText;

	int amount;

	void Awake() {
		blockImage = GetComponent<Image>();
		amountText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
	}

	// Initialize the panel
	public void initialize(Goal goal) {
		gameObject.SetActive(true);
		blockImage.sprite = goal.getBlock().GetComponent<SpriteRenderer>().sprite;
		amount = goal.getAmount();
		amountText.text = amount.ToString();
	}

	// Update the remaining goal amount gradually.
	public void updateAmount(Goal goal) {
		amount = Mathf.Clamp(amount - 1, goal.getAmount(), int.MaxValue);
		amountText.text = amount.ToString();
	}

	// Getters
	public Sprite getSprite() { return blockImage.sprite; }
}
