using UnityEngine;

public enum BlockColor { blue, green, purple, red, yellow }

public class ColorBlock : Block, IFillable, IFallable {
	[SerializeField] BlockColor color;
	ColorMatch colorMatch;

	SpriteRenderer spriteRenderer;

	protected override void Awake() {
		base.Awake();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// IPoolable.reset
	void OnEnable() {
		setBlockGroup(null);
	}

	void OnMouseDown() {
		checkForPowerUps(PowerUpManager.powerUpThreshold);
		colorMatch?.blast();
	}

	public override void blast() {
		gameObject.SetActive(false);
	}

	// Fill empty spaces below
	void IFillable.fill(int rowCount) {
		Vector2 target = getFillingTarget(rowCount);
		setSortingOrder(target);
		StartCoroutine(moveTowardsTarget(target));
	}

	// Fall from top of the screen
	void IFallable.fall(int rowCount) {
		Vector2 target = getFallingTarget(rowCount);
		setSortingOrder(target);
		StartCoroutine(moveTowardsTarget(target));
	}

	// To avoid undesired overlappings
	public void setSortingOrder(Vector2 blockPosition) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		int sortingOrder = blockGrid.getSize().y / 2 + blockGrid.worldToCell(blockPosition).y;
		spriteRenderer.sortingOrder = sortingOrder;
	}

	// If block match size is above threshold spawn a random rocket.
	void checkForPowerUps(int powerUpThreshold) {
		if (colorMatch?.getColorBlocks().Count >= powerUpThreshold) {
			BlockSpawner blockSpawner = LevelManager.getInstance().getBlockSpawner();
			Rocket rocketPrefab = Prefabs.getInstance().getRandomRocket();
			blockSpawner.spawnBlock(rocketPrefab, transform.position);
		}
	}

	// Getters & Setters
	public BlockColor getColor() { return color; }
	public ColorMatch getBlockGroup() { return colorMatch; }

	public void setBlockGroup(ColorMatch colorMatch) { this.colorMatch = colorMatch; }
}