using UnityEngine.Events;

public class Events {
	static Events instance;

	public UnityEvent<BlockGroup> matchBlasted;
	public UnityEvent<BlockGroup, Goal> goalMatch;
	public UnityEvent<GoalEntryPanelUI> blockHitGoal;

	// Rename these, or gridReady(bool)
	public UnityEvent filling;
	public UnityEvent fillingDone;

	Events() {
		matchBlasted = new UnityEvent<BlockGroup>();
		goalMatch = new UnityEvent<BlockGroup, Goal>();
		blockHitGoal = new UnityEvent<GoalEntryPanelUI>();


		filling = new UnityEvent();
		fillingDone = new UnityEvent();
	}

	public static Events getInstance() {
		if (instance == null)
			instance = new Events();
		return instance;
	}
}