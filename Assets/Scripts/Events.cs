using UnityEngine.Events;

public class Events {
	static Events instance;

	// Possibly UnityEvent<BlockGroup/Match>
	public UnityEvent<BlockGroup> matchBlasted;

	public UnityEvent noMovesLeft;

	Events() {
		matchBlasted = new UnityEvent<BlockGroup>();
		noMovesLeft = new UnityEvent();
	}

	public static Events getInstance() {
		if (instance == null)
			instance = new Events();
		return instance;
	}
}