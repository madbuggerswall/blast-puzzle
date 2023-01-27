using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Because Editor can't serialize (int , Block)
[System.Serializable]
public class Goal {
	[SerializeField] int amount;
	[SerializeField] Block block;

	public int getAmount() { return amount; }
	public void decrementAmount() { amount--; }
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

	void checkForGoals(BlockGroup blockGroup) {
		foreach (Goal goal in goals) {
			// Gateway for color block goals
			ColorBlock goalBlock = goal.getBlock() as ColorBlock;
			if (goalBlock == null)
				continue;

			foreach (ColorBlock colorBlock in blockGroup.getColorBlocks()) {
				// Gateway for wrong colors and completed goals
				bool colorsMatch = colorBlock.getColor() == goalBlock.getColor();
				bool goalCompleted = goal.getAmount() == 0;

				if (!colorsMatch || goalCompleted)
					break;

				goal.decrementAmount();
				FindObjectOfType<GoalEffectUI>().flyToGoal(colorBlock);
			}
		}
	}

	// void checkForGoals(Block block) { }

	public List<Goal> getGoals() { return goals; }
}
