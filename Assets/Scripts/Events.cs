using UnityEngine.Events;

public class Events {
	static Events instance;

	// Blast
	public UnityEvent<ColorMatch> matchBlasted;
	public UnityEvent<Block> blockBlasted;

	// Goal
	public UnityEvent<ColorMatch, Goal> goalMatch;	//Rename this
	public UnityEvent<Block, Goal> goalBlock;	//Rename this
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
		goalMatch = new UnityEvent<ColorMatch, Goal>();
		goalBlock = new UnityEvent<Block, Goal>();
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