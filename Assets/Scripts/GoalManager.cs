using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[SerializeField] List<Goal> goals;
	[SerializeField] int movesLeft;

	void Start() {
		Events.getInstance().matchBlasted.AddListener(checkGoals);
		Events.getInstance().matchBlasted.AddListener(delegate { decrementMovesLeft(); });
		Events.getInstance().blockBlasted.AddListener(checkGoals);
	}

	void checkGoals(ColorMatch colorMatch) {
		foreach (Goal goal in goals) {
			// Gateway for color block goals
			if (goal.getBlock() is not ColorBlock)
				continue;

			ColorBlock goalBlock = goal.getBlock() as ColorBlock;
			bool colorsMatch = colorMatch.getColorBlocks()[0].getColor() == goalBlock.getColor();
			bool goalCompleted = goal.getAmount() <= 0;

			if (colorsMatch && !goalCompleted) {
				goal.decrementAmount(colorMatch.getColorBlocks().Count);
				Events.getInstance().matchInGoals.Invoke(colorMatch, goal);
			}
		}
	}

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

	void decrementMovesLeft() {
		movesLeft--;
		if (movesLeft == 0)
			Events.getInstance().noMovesLeft.Invoke();
	}

	public List<Goal> getGoals() { return goals; }
	public int getMovesLeft() { return movesLeft; }
}
