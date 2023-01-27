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

	public void flyToGoal(ColorBlock colorBlock) {
		GameObject blockSprite = objectPool.spawn(spritePrefab, colorBlock.transform.position);
		Sprite goalSprite = colorBlock.GetComponent<SpriteRenderer>().sprite;
		blockSprite.GetComponent<SpriteRenderer>().sprite = goalSprite;

		StartCoroutine(moveTowards(blockSprite, getTarget(goalSprite)));
	}

	IEnumerator moveTowards(GameObject sprite, Vector2 target) {
		const float maxDelta = 12;
		while ((Vector2) sprite.transform.position != target) {
			sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, target, maxDelta * Time.deltaTime);
			yield return null;
		}

		sprite.SetActive(false);
	}

	Vector2 getTarget(Sprite goalSprite) {
		GoalEntryPanelUI[] panels = transform.parent.GetComponentsInChildren<GoalEntryPanelUI>();
		foreach (GoalEntryPanelUI panel in panels)
			if (panel.getSprite() == goalSprite)
				return panel.transform.position;

		return Vector2.zero;
	}
}
