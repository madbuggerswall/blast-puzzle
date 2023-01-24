using UnityEngine.Events;

public class Events {
	static Events instance;

	public UnityEvent noMovesLeft;

	Events() {
		noMovesLeft = new UnityEvent();
	}

	public static Events getInstance() {
		if (instance == null)
			instance = new Events();
		return instance;
	}
}