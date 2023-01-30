using UnityEngine.Events;

public class Events {
	static Events instance;

	// Blast
	public UnityEvent<ColorMatch> matchBlasted;
	public UnityEvent<Block> blockBlasted;

	// Goal
	public UnityEvent<ColorMatch, Goal> matchInGoals;	
	public UnityEvent<Block, Goal> blockInGoals;	
	public UnityEvent<GoalEntryPanelUI> blockHitGoal;
	public UnityEvent<Goal> goalAccomplished;

	// Fill
	public UnityEvent filling;
	public UnityEvent fillingDone;

	Events() {
		// Blast
		matchBlasted = new UnityEvent<ColorMatch>();
		blockBlasted = new UnityEvent<Block>();

		// Goal
		matchInGoals = new UnityEvent<ColorMatch, Goal>();
		blockInGoals = new UnityEvent<Block, Goal>();
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