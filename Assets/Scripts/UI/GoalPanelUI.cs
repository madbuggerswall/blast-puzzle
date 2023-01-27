using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPanelUI : MonoBehaviour {
	[SerializeField] GoalEntryPanelUI goalEntryPanelPrefab;
	[SerializeField] GameObject goalPanel;

	void Start() {
		initializeGoalEntryPanels();
	}

	void initializeGoalEntryPanels() {
		GoalManager goalManager = LevelManager.getInstance().getGoalManager();

		foreach (Goal goal in goalManager.getGoals()) {
			GoalEntryPanelUI goalEntryPanel = Instantiate(goalEntryPanelPrefab, goalPanel.transform);
			goalEntryPanel.initialize(goal);
		}
	}
}
