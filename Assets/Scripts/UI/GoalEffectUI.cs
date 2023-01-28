using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Rename this to GoalAnimationUI/GoalBlockAnimationUI
public class GoalEffectUI : MonoBehaviour {
	[SerializeField] GameObject spritePrefab;
	ObjectPool objectPool;

	void Awake() {
		objectPool = GetComponentInChildren<ObjectPool>();
	}

	void Start() {
		Events.getInstance().goalMatch.AddListener(moveMatchTowardsGoalIcon);
	}

	void moveMatchTowardsGoalIcon(BlockGroup blockGroup, Goal goal) {
		foreach (ColorBlock colorBlock in blockGroup.getColorBlocks()) {
			moveSpriteTowardsGoalIcon(colorBlock, goal);
		}
	}

	void moveSpriteTowardsGoalIcon(ColorBlock colorBlock, Goal goal) {
		GameObject blockSprite = objectPool.spawn(spritePrefab, colorBlock.transform.position);
		Sprite goalIcon = colorBlock.GetComponent<SpriteRenderer>().sprite;
		blockSprite.GetComponent<SpriteRenderer>().sprite = goalIcon;

		StartCoroutine(moveSpriteTowards(blockSprite, goal));
	}

	IEnumerator moveSpriteTowards(GameObject sprite, Goal goal) {
		const float maxDelta = 12;
		GoalEntryPanelUI goalPanel = getPanel(goal);
		Vector3 target = goalPanel.transform.position;

		while (sprite.transform.position != target) {
			sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, target, maxDelta * Time.deltaTime);
			yield return null;
		}

		// Goal amount decremented here for visual correctness
		goalPanel.updateAmount(goal);
		sprite.SetActive(false);
		Events.getInstance().blockHitGoal.Invoke(goalPanel);
	}

	GoalEntryPanelUI getPanel(Goal goal) {
		GoalEntryPanelUI[] panels = transform.parent.GetComponentsInChildren<GoalEntryPanelUI>();
		foreach (GoalEntryPanelUI panel in panels)
			if (panel.getSprite() == goal.getBlock().GetComponent<SpriteRenderer>().sprite)
				return panel;

		throw new System.Exception("Targeted panel could not be found");
	}
}
