using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockGrid : MonoBehaviour {
	readonly Vector2Int minSize = new Vector2Int(4, 4);
	readonly Vector2Int maxSize = new Vector2Int(28, 12);
	readonly Vector2 margin = new Vector2(0.24f, 0.4f);

	[SerializeField] Vector2Int size;

	SpriteRenderer spriteRenderer;
	Grid grid;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		grid = GetComponentInParent<Grid>();
	}

	void Start() {
		setGridSize();
	}

	// TODO: Doesn't work with odd numbers
	void setGridSize() {
		spriteRenderer.size = size + margin;
	}


	// TODO Camera center on grid
	void adjustPosition() {
		Vector3 offset = new Vector3();
		if (size.x % 2 != 0)
			offset.x = 0.5f;
		if (size.y % 2 != 0)
			offset.y = transform.position.y + 0.5f;

		transform.position = offset;
	}

	// Grid 
	public Vector3Int worldToCell(Vector3 position) { return grid.WorldToCell(position); }
	public Vector3 cellToWorld(Vector3Int cellIndex) { return grid.GetCellCenterLocal(cellIndex); }

	// Getters
	public Vector2Int getSize() { return size; }
}
