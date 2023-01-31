using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockGrid : MonoBehaviour {
	readonly Vector2Int minSize = new Vector2Int(4, 4);
	readonly Vector2Int maxSize = new Vector2Int(10, 16);
	readonly Vector2 margin = new Vector2(0.24f, 0.4f);

	[SerializeField] Vector2Int size;

	SpriteRenderer spriteRenderer;
	Grid grid;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		grid = GetComponentInParent<Grid>();

		clampSize();
		setGridSize();
		adjustPosition();
	}

	// Clamps the size between minimum and maximum size values.
	void clampSize() {
		size.x = Mathf.Clamp(size.x, minSize.x, maxSize.x);
		size.y = Mathf.Clamp(size.y, minSize.y, maxSize.y);
	}

	// Sets grid size accounting for the border margins
	void setGridSize() {
		spriteRenderer.size = size + margin;
	}

	// If grid size contains odd numbers, shift the grid and camera to align with UnityEngine.Grid
	void adjustPosition() {
		Vector3 offset = new Vector3();
		if (size.x % 2 != 0)
			offset.x = 0.5f;
		if (size.y % 2 != 0)
			offset.y = 0.5f;

		transform.position += offset;
		Camera.main.transform.position = new Vector3(offset.x, offset.y, Camera.main.transform.position.z);
	}

	// Grid Utilities
	public Vector2Int worldToCell(Vector2 position) { return (Vector2Int) grid.WorldToCell(position); }
	public Vector2 cellToWorld(Vector2Int cellIndex) { return grid.GetCellCenterLocal((Vector3Int) cellIndex); }
	public bool isCellInBound(Vector2Int cell) {
		(Vector2Int min, Vector2Int max) bounds = getCellBounds();
		bool horizontal = cell.x >= bounds.min.x && cell.x < bounds.max.x;
		bool vertical = cell.y >= bounds.min.y && cell.y < bounds.max.y;
		return horizontal && vertical;
	}

	// Getters
	public Vector2Int getSize() { return size; }
	public (Vector2Int min, Vector2Int max) getCellBounds() {
		Vector2Int firstCell = worldToCell((Vector2) transform.position - size / 2);
		Vector2Int lastCell = worldToCell((Vector2) transform.position + Vector2Int.CeilToInt((Vector2) size / 2));
		return (firstCell, lastCell);
	}
}
