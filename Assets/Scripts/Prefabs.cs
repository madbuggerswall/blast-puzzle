using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Prototype/Clone Pattern
public class Prefabs : MonoBehaviour {
	static Prefabs instance;

	[Header("Color Blocks")]
	[SerializeField] Block blueBlock;
	[SerializeField] Block greenBlock;
	[SerializeField] Block purpleBlock;
	[SerializeField] Block redBlock;
	[SerializeField] Block yellowBlock;

	[Header("Other Blocks")]
	[SerializeField] Block duck;
	[SerializeField] Block balloon;


	void Awake() {
		assertSingleton();
	}

	public Block getBlock(BlockColor color) {
		switch (color) {
			case BlockColor.blue:
				return blueBlock;
			case BlockColor.green:
				return greenBlock;
			case BlockColor.purple:
				return purpleBlock;
			case BlockColor.red:
				return redBlock;
			case BlockColor.yellow:
				return yellowBlock;
			default:
				return blueBlock;
		}
	}

	public Block getBlock(int blockColor) {
		return getBlock((BlockColor) blockColor);
	}

	public Block getDuck() { return duck; }
	public Block getBalloon() { return balloon; }

	// Singleton
	public static Prefabs getInstance() { return instance; }
	void assertSingleton() { if (instance == null) { instance = this; } else { Destroy(gameObject); } }
}
