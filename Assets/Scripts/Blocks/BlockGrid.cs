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

		setGridSize();
		adjustPosition();
	}

	void Start() {
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
			offset.y = 0.5f;

		transform.position += offset;
		Camera.main.transform.position = new Vector3(offset.x, offset.y, Camera.main.transform.position.z);
	}

	// Grid 
	public Vector2Int worldToCell(Vector2 position) { return (Vector2Int) grid.WorldToCell(position); }
	public Vector2 cellToWorld(Vector2Int cellIndex) { return grid.GetCellCenterLocal((Vector3Int) cellIndex); }

	// Getters
	public Vector2Int getSize() { return size; }
	public (Vector2Int min, Vector2Int max) getCellBounds() {
		Vector2Int firstCell = worldToCell((Vector2) transform.position - size / 2);
		Vector2Int lastCell = worldToCell((Vector2) transform.position + Vector2Int.CeilToInt((Vector2) size / 2));
		return (firstCell, lastCell);
	}
}
