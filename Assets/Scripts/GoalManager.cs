using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Because Editor can't serialize (int , Block)

[System.Serializable]
public class GoalEntry {
	[SerializeField] int amount;
	[SerializeField] Block block;

	public int getAmount() { return amount; }
	public void decrementAmount() { amount--; }
	public Block getBlock() { return block; }
}

[System.Serializable]
public class Goal {
	[SerializeField] List<GoalEntry> goalEntries;
	[SerializeField] int movesLeft;


	public Goal() {
		goalEntries = new List<GoalEntry>();
	}

	public void setGoals(params GoalEntry[] entries) {
		foreach (GoalEntry goalEntry in entries) {
			this.goalEntries.Add(goalEntry);
		}
	}

	public List<GoalEntry> getGoalEntries() { return goalEntries; }
}

public class GoalManager : MonoBehaviour {
	// Maybe a list of goals instead of this, would ditch GoalEntry class that way
	[SerializeField] Goal goal;

	void Awake() {
	}

	void Start() {
		Events.getInstance().matchBlasted.AddListener(checkForGoals);
	}

	void checkForGoals(BlockGroup blockGroup) {
		foreach (ColorBlock colorBlock in blockGroup.getColorBlocks()) {
			Debug.Log(colorBlock.GetType() + "|" + colorBlock.getColor());
			foreach (GoalEntry goalEntry in goal.getGoalEntries()) {
				bool colorsMatch = ((ColorBlock) goalEntry.getBlock())?.getColor() == colorBlock.getColor();
				if (colorsMatch && goalEntry.getAmount() > 0) {
					goalEntry.decrementAmount();
					Debug.Log("Amount: " + goalEntry.getAmount());
				}
			}
		}
	}

	// void checkForGoals(Block block) { }
}
