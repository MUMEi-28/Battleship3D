using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacingShip : Tiles
{
	public LayerMask shipMask;
	public GameManager manager;
	private void FixedUpdate()
	{
		if (manager.gameState == GameState.placingShip)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.up, out hit, 2f, shipMask))
			{
				SetOccupiedMaterial();
			}
			else
			{
				SetUnocupiedMaterial();
			}
		}
	}
}
