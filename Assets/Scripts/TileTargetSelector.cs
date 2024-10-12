using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTargetSelector : Tiles
{
	[Header("Selecting Target")]
	public bool isPlayerTurn;
	public LayerMask tileTargetMask;
	private GameObject prevHit;
	[Header("Bomb")]
	public Bomb bomb;
	private void Update()
	{
		if (isPlayerTurn)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileTargetMask))
			{
				// If we hit a new tile, update its material and reset the previous one
				GameObject currentHit = hit.collider.gameObject;
				if (prevHit != currentHit)
				{
					// Reset the previous tile's material if it exists
					if (prevHit != null)
					{
						SetUnocupiedMaterial(prevHit);
					}

					// Set the material of the currently hit tile
					SetOccupiedMaterial(currentHit);

					// Update prevHit to the current tile
					prevHit = currentHit;
				}

				// Select the tile as the target
				if(Input.GetMouseButtonDown(0))
				{
					bomb.targetPosition = currentHit.transform;
				}
			}
			else
			{
				// If no tile is hit, reset the previously highlighted tile
				if (prevHit != null)
				{
					SetUnocupiedMaterial(prevHit);
					prevHit = null;
				}
			}
		}
	}
}
