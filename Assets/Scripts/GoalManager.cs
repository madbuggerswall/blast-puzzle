using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Fire an event when a goal is accomplished
// Because Editor can't serialize (int , Block)
[System.Serializable]
public class Goal {
	[SerializeField] int amount;
	[SerializeField] Block block;

	public void decrementAmount(int count) {
		amount = Mathf.Clamp(amount - count, 0, int.MaxValue);
		if (amount == 0)
			Events.getInstance().goalAccomplished.Invoke(this);
	}

	// Getters
	public int getAmount() { return amount; }
	public Block getBlock() { return block; }
}

public class GoalManager : MonoBehaviour {
	// Maybe a list of goals instead of this, would ditch GoalEntry class that way
	[SerializeField] List<Goal> goals;
	[SerializeField] int movesLeft;


	void Awake() {
	}

	void Start() {
		Events.getInstance().matchBlasted.AddListener(checkForGoals);
	}

	// Rename this 
	void checkForGoals(BlockGroup blockGroup) {
		foreach (Goal goal in goals) {
			// Gateway for color block goals
			ColorBlock goalBlock = goal.getBlock() as ColorBlock;
			if (goalBlock == null)
				continue;

			bool colorsMatch = blockGroup.getColorBlocks()[0].getColor() == goalBlock.getColor();
			bool goalCompleted = goal.getAmount() <= 0;

			if (colorsMatch && !goalCompleted) {
				goal.decrementAmount(blockGroup.getColorBlocks().Count);
				Events.getInstance().goalMatch.Invoke(blockGroup, goal);
			}
		}
	}

	// void checkForGoals(Block block) { }

	public List<Goal> getGoals() { return goals; }
}
