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

	void Start() {
		Events.getInstance().matchBlasted.AddListener(checkForGoals);
		Events.getInstance().blockBlasted.AddListener(checkForGoals);
	}

	// Rename this 
	void checkForGoals(ColorMatch colorMatch) {
		foreach (Goal goal in goals) {
			// Gateway for color block goals
			if (goal.getBlock() is not ColorBlock)
				continue;

			ColorBlock goalBlock = goal.getBlock() as ColorBlock;
			bool colorsMatch = colorMatch.getColorBlocks()[0].getColor() == goalBlock.getColor();
			bool goalCompleted = goal.getAmount() <= 0;

			if (colorsMatch && !goalCompleted) {
				goal.decrementAmount(colorMatch.getColorBlocks().Count);
				Events.getInstance().goalMatch.Invoke(colorMatch, goal);
			}
		}
	}

	// TODO Goals should work with Duck and Balloon blocks
	void checkForGoals(Block block) {
		foreach (Goal goal in goals) {
			if (goal.getBlock().GetType() != block.GetType())
				continue;

			bool goalCompleted = goal.getAmount() <= 0;
			if (!goalCompleted) {
				goal.decrementAmount(1);
				Events.getInstance().goalBlock.Invoke(block, goal);
			}
		}
	}

	public List<Goal> getGoals() { return goals; }
}
