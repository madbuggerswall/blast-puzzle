using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Rename this to BlockEffects/BlockFX
public class GoalEffectUI : MonoBehaviour {
	[SerializeField] GameObject spritePrefab;
	ObjectPool objectPool;

	void Awake() {
		objectPool = GetComponentInChildren<ObjectPool>();
	}

	void Start() {
		Events.getInstance().matchInGoals.AddListener(moveMatchTowardsGoalIcon);
		Events.getInstance().blockInGoals.AddListener(moveBlockTowardsGoalIcon);
	}

	// Move every colored block in a color match towards the corresponding goal icon in the UI.
	void moveMatchTowardsGoalIcon(ColorMatch colorMatch, Goal goal) {
		foreach (ColorBlock colorBlock in colorMatch.getColorBlocks()) {
			moveBlockTowardsGoalIcon(colorBlock, goal);
		}
	}


	// Move a colored block in towards the corresponding goal icon in the UI.
	void moveBlockTowardsGoalIcon(Block block, Goal goal) {
		GameObject blockSprite = objectPool.spawn(spritePrefab, block.transform.position);
		Sprite goalIcon = block.GetComponent<SpriteRenderer>().sprite;
		blockSprite.GetComponent<SpriteRenderer>().sprite = goalIcon;

		StartCoroutine(moveSpriteTowards(blockSprite, goal));
	}

	// Move transform towards corresponding goal panel. Update the panel and notify Events when it reaches target.
	IEnumerator moveSpriteTowards(GameObject sprite, Goal goal) {
		const float maxDelta = 12;
		GoalEntryPanelUI goalPanel = getPanel(goal);
		Vector3 target = goalPanel.transform.position;

		while (sprite.transform.position != target) {
			sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, target, maxDelta * Time.deltaTime);
			yield return null;
		}

		goalPanel.updateAmount(goal);
		sprite.SetActive(false);
		Events.getInstance().blockHitGoal.Invoke(goalPanel);
	}

	// Get the goal panel for corresponding goal
	GoalEntryPanelUI getPanel(Goal goal) {
		GoalEntryPanelUI[] panels = transform.parent.GetComponentsInChildren<GoalEntryPanelUI>();
		foreach (GoalEntryPanelUI panel in panels)
			if (panel.getSprite() == goal.getBlock().GetComponent<SpriteRenderer>().sprite)
				return panel;

		throw new System.Exception("Targeted panel could not be found");
	}
}
