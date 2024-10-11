using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDragAndDrop : MonoBehaviour
{
	RaycastHit hit;
	Ray ray;
	public Camera mainCam;
	[Header("Dragging Ships")]
	public LayerMask shipMask;
	public GameObject currentShip;
	public bool isDragging;

	[Header("Dropping Ships")]
	public LayerMask tileMask;
	public GameObject currentTile;
	public float yOffset;

	[Header("Snapping")]
	public float snappingSpeed;

	public Material testMaterial;

	public Tiles[] tiles;
	public Collider currentShipCollider;
	private void Start()
	{
		mainCam = Camera.main;
		tiles = FindObjectsOfType<Tiles>();
	}

	private void Update()
	{
		ray = mainCam.ScreenPointToRay(Input.mousePosition);

		ray = mainCam.ScreenPointToRay(Input.mousePosition);

		// Check if we're currently dragging a ship
		if (isDragging)
		{
			// Use the tile mask for placement while dragging
			DropShip();
		}
		else if (Physics.Raycast(ray, out hit, Mathf.Infinity, shipMask))
		{
			// If not dragging, allow selecting a new ship
			DragShip();
		}
	}
	private void DropShip()
	{
		// Get tile position
		if (Input.GetMouseButtonDown(0) && currentShip != null)
		{
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileMask))
			{

				// Make sure that the current tile is not holding any ship
				if (IsCollidingWithOtherShip())
				{
					SnapShipToOriginalPositon();
					Debug.Log("ALREADY HAVE A SHIP");
				}
				else
				{
					// Adjust ship offset to position on the tile
					currentShip.transform.position = new Vector3(hit.transform.position.x, yOffset, hit.transform.position.z);
					currentShip.GetComponent<ShipController>().isSelected = false;
					isDragging = false;
				}
				Debug.Log("DROPPED A SHIP");
				currentShip = null;
				isDragging = false;

			}
			else
			{
				SnapShipToOriginalPositon();
				Debug.Log("Not within tile map");
			}
		}
	}


	private void DragShip()
	{
		// Start dragging on mouse click
		if (Input.GetMouseButtonDown(0))
		{
			GameObject hitObject = hit.collider.gameObject;

			hitObject.GetComponent<ShipController>().isSelected = true;
			currentShip = hitObject;

			currentShipCollider = currentShip.GetComponent<Collider>(); // Get the collider of the current ship
			isDragging = true;

			Debug.Log("STARTED DRAGGING SHIP");
		}
	}
	private bool IsCollidingWithOtherShip()
	{
		Collider[] hitColliders = Physics.OverlapBox(currentShipCollider.bounds.center, currentShipCollider.bounds.extents, currentShip.transform.rotation, shipMask);

		// Check if there are any colliders within the ship's space (excluding itself)
		foreach (Collider col in hitColliders)
		{
			if (col.gameObject != currentShip)
			{
				return true; // There's a collision with another ship
			}
		}
		return false;
	}
	private void SnapShipToOriginalPositon()
	{
		// Reset the ship data
		ShipController ship = currentShip.GetComponent<ShipController>();
		currentShip.transform.position = ship.originalPosition;
		currentShip.transform.rotation = ship.originalRotation;
		ship.rotateShipIndex = 0;

		ship.isSelected = false;

		currentShip = null;
		isDragging = false;

		Debug.Log("SNAPPED SHIP");
	}
}

