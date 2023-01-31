using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPanelUI : MonoBehaviour {
	[SerializeField] GoalEntryPanelUI goalEntryPanelPrefab;
	[SerializeField] GameObject goalPanel;
	[SerializeField] TMPro.TextMeshProUGUI movesLeft;

	void Start() {
		initializeGoalEntryPanels();
		updateMovesLeft();
		
		Events.getInstance().matchBlasted.AddListener(delegate { updateMovesLeft(); });
	}

	// Initialize all goal panels
	void initializeGoalEntryPanels() {
		GoalManager goalManager = LevelManager.getInstance().getGoalManager();

		foreach (Goal goal in goalManager.getGoals()) {
			GoalEntryPanelUI goalEntryPanel = Instantiate(goalEntryPanelPrefab, goalPanel.transform);
			goalEntryPanel.initialize(goal);
		}
	}
	
	// Update moves left panel
	void updateMovesLeft() {
		movesLeft.text = LevelManager.getInstance().getGoalManager().getMovesLeft().ToString();
	}
}
