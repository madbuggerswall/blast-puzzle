using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Because Editor can't serialize (int , Block)
[System.Serializable]
public class GoalEntry {
	int amount;
	// BlockType or Block
}

[System.Serializable]
public class Goal {
	[SerializeField] List<GoalEntry> goalEntries;

	public Goal() {
		goalEntries = new List<GoalEntry>();
	}

	public void setGoals(params GoalEntry[] entries) {
		foreach (GoalEntry goalEntry in entries) {
			this.goalEntries.Add(goalEntry);
		}
	}
}

public class GoalManager : MonoBehaviour {
	[SerializeField] Goal goal;
	[SerializeField] int movesLeft;

	[SerializeField] KeyValuePair<int, int> dictionaryEntry;

	void Awake() {
		Dictionary<int, int> stuff = new Dictionary<int, int>();
	}
}
