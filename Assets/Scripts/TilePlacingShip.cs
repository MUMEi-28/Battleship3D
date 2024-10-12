using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacingShip : Tiles
{
	public LayerMask shipMask;
	private void FixedUpdate()
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
