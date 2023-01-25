using UnityEngine.Events;

public class Events {
	static Events instance;

	// Possibly UnityEvent<BlockGroup/Match>
	public UnityEvent<BlockGroup> matchBlasted;

	// Rename these, or gridReady(bool)
	public UnityEvent filling;
	public UnityEvent fillingDone;

	Events() {
		matchBlasted = new UnityEvent<BlockGroup>();

		filling = new UnityEvent();		
		fillingDone = new UnityEvent();		
	}

	public static Events getInstance() {
		if (instance == null)
			instance = new Events();
		return instance;
	}
}