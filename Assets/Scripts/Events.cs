using UnityEngine.Events;

public class Events {
	static Events instance;

	// Blast
	public UnityEvent<BlockGroup> matchBlasted;
	public UnityEvent<Duck> duckBlasted;
	public UnityEvent<Balloon> balloonBlasted;

	// Goal
	public UnityEvent<BlockGroup, Goal> goalMatch;
	public UnityEvent<GoalEntryPanelUI> blockHitGoal;
	public UnityEvent<Goal> goalAccomplished;

	// Fill
	public UnityEvent filling;
	public UnityEvent fillingDone;

	Events() {
		// Blast
		matchBlasted = new UnityEvent<BlockGroup>();
		duckBlasted = new UnityEvent<Duck>();
		balloonBlasted = new UnityEvent<Balloon>();

		// Goal
		goalMatch = new UnityEvent<BlockGroup, Goal>();
		blockHitGoal = new UnityEvent<GoalEntryPanelUI>();
		goalAccomplished = new UnityEvent<Goal>();

		// Fill
		filling = new UnityEvent();
		fillingDone = new UnityEvent();
	}

	public static Events getInstance() {
		if (instance == null)
			instance = new Events();
		return instance;
	}
}