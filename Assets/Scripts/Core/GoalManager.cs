using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Because Editor can't serialize (int , Block)
[System.Serializable]
public class Goal {
	[SerializeField] int amount;
	[SerializeField] Block block;

	// Decrement remaining goal amount. Notify Events if goal is acheived
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
	[SerializeField] List<Goal> goals;
	[SerializeField] int movesLeft;

	void Start() {
		Events.getInstance().matchBlasted.AddListener(checkGoals);
		Events.getInstance().matchBlasted.AddListener(delegate { decrementMovesLeft(); });
		Events.getInstance().blockBlasted.AddListener(checkGoals);
	}

	// Check if ColorMatch counts for any goal
	void checkGoals(ColorMatch colorMatch) {
		foreach (Goal goal in goals) {
			// Gateway for color block goals
			if (goal.getBlock() is not ColorBlock)
				continue;

			// Check if colors are matched and goal isn't completed yet
			ColorBlock goalBlock = goal.getBlock() as ColorBlock;
			bool colorsMatch = colorMatch.getColorBlocks()[0].getColor() == goalBlock.getColor();
			bool goalCompleted = goal.getAmount() <= 0;

			// If ColorMatch is counted for a Goal notify Events (for effects mostlyF)
			if (colorsMatch && !goalCompleted) {
				goal.decrementAmount(colorMatch.getColorBlocks().Count);
				Events.getInstance().matchInGoals.Invoke(colorMatch, goal);
			}
		}
	}

	// Check if block counts for any goal
	void checkGoals(Block block) {
		foreach (Goal goal in goals) {
			if (goal.getBlock().GetType() != block.GetType())
				continue;

			if (block is ColorBlock)
				if ((block as ColorBlock).getColor() != (goal.getBlock() as ColorBlock).getColor())
					continue;

			bool goalCompleted = goal.getAmount() <= 0;
			if (!goalCompleted) {
				goal.decrementAmount(1);
				Events.getInstance().blockInGoals.Invoke(block, goal);
			}
		}
	}

	// If there are no moves left notify Events. Nothing will happen but it's there.
	void decrementMovesLeft() {
		movesLeft--;
		if (movesLeft == 0)
			Events.getInstance().noMovesLeft.Invoke();
	}

	// Getters
	public List<Goal> getGoals() { return goals; }
	public int getMovesLeft() { return movesLeft; }
}
