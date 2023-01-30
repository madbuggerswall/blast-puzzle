using UnityEngine;

public enum BlockColor {
	blue,
	green,
	purple,
	red,
	yellow
}

public class ColorBlock : Block, IFallable, IFillable {
	[SerializeField] BlockColor color;
	BlockGroup blockGroup;

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
		if (blockGroup?.getColorBlocks().Count >= 5) {
			BlockSpawner blockSpawner = LevelManager.getInstance().getBlockSpawner();
			Rocket rocketPrefab = Prefabs.getInstance().getRocket();

			blockSpawner.spawnBlock(rocketPrefab, transform.position);
		}

		blockGroup?.blast();
	}

	public override void blast() {
		gameObject.SetActive(false);
	}

	// IFallable Fall from top of the screen
	public void fall(int rowCount) {
		Vector2 target = getFallingTarget(rowCount);
		setSortingOrder(target);
		StartCoroutine(moveTowardsTarget(target));
	}

	// IFillable Fill empty spaces below
	public void fill(int rowCount) {
		Vector2 target = getFillingTarget(rowCount);
		setSortingOrder(target);
		StartCoroutine(moveTowardsTarget(target));
	}

	// To avoid undesired overlappings
	public void setSortingOrder(Vector2 blockPosition) {
		BlockGrid blockGrid = LevelManager.getInstance().getBlockGrid();
		int sortingOrder = blockGrid.getSize().y / 2 + blockGrid.worldToCell(blockPosition).y;
		spriteRenderer.sortingOrder = sortingOrder;
	}

	// Getters & Setters
	public BlockColor getColor() { return color; }
	public BlockGroup getBlockGroup() { return blockGroup; }

	public void setBlockGroup(BlockGroup blockGroup) { this.blockGroup = blockGroup; }
}