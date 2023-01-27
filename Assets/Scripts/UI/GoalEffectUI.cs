using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Rename this to GoalAnimationUI/GoalBlockAnimationUI
public class GoalEffectUI : MonoBehaviour {
	ObjectPool objectPool;

	void Awake() {
		objectPool = GetComponentInChildren<ObjectPool>();
	}

	public void flyToGoal(ColorBlock colorBlock) {
		GameObject spawnedSprite = objectPool.spawn(Prefabs.getInstance().getBlockSprite(), colorBlock.transform.position);
		Sprite goalSprite = colorBlock.GetComponent<SpriteRenderer>().sprite;
		spawnedSprite.GetComponent<SpriteRenderer>().sprite = goalSprite;

		StartCoroutine(moveTowards(spawnedSprite, getTarget(goalSprite)));
	}

	IEnumerator moveTowards(GameObject spawnedSprite, Vector2 target) {
		const float maxDelta = 12;
		while ((Vector2) spawnedSprite.transform.position != target) {
			spawnedSprite.transform.position = Vector3.MoveTowards(spawnedSprite.transform.position, target, maxDelta * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		spawnedSprite.gameObject.SetActive(false);
	}

	Vector2 getTarget(Sprite goalSprite) {
		GoalEntryPanelUI[] panels = transform.parent.GetComponentsInChildren<GoalEntryPanelUI>();
		foreach (GoalEntryPanelUI panel in panels)
			if (panel.getSprite() == goalSprite)
				return panel.transform.position;

		return Vector2.zero;
	}
}
