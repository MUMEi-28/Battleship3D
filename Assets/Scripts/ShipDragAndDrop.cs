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

	[Header("Dropping Ships")]
	public LayerMask tileMask;
	public GameObject currentTile;
	public float yOffset;

	public Material testMaterial;

	public Tiles[] tiles;
	private void Start()
	{
		mainCam = Camera.main;
		tiles = FindObjectsOfType<Tiles>();
	}

	private void Update()
	{
		ray = mainCam.ScreenPointToRay(Input.mousePosition); // Move raycasting here to ensure it's always updated
		DragShip();
		DropShip();
	}

	private void DropShip()
	{
		// Get tile position
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileMask))
		{
			if (Input.GetMouseButtonDown(0) && currentShip != null)
			{
				// Make sure that the current tile is not holding any ship
				foreach (Tiles item in tiles)
				{
					if (item.isHoldingShip)
					{
						Debug.Log("ALREADY HAVE A SHIP");
					}
					else
					{
						item.UpdateIfHoldingASHip();

						// Adjust ship offset to position on the tile
						currentShip.transform.position = new Vector3(hit.transform.position.x, yOffset, hit.transform.position.z);
						currentShip.GetComponent<ShipController>().isSelected = false;
					}

				}
				Debug.Log("DROPPED A SHIP");
				currentShip = null;
			}
		}
	}

	private void DragShip()
	{
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, shipMask))
		{
			if (Input.GetMouseButtonDown(0))
			{
				GameObject hitObject = hit.collider.gameObject;

				hitObject.GetComponent<ShipController>().isSelected = true;
				currentShip = hitObject;

			}
		}
	}
}
