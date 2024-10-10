using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{

	public Material occupiedMaterial;
	public Material extraSpaceMaterial;
	public Material unOcuppiedMaterial;

	public LayerMask shipMask;

	public bool isHoldingShip;


	private void FixedUpdate()
	{
		ChangeMaterial();
	}

	private void ChangeMaterial()
	{

		// Makes sure that it only runs when the player is holding a ship
		// it should not run all the time for performance issues
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
	public void SetUnocupiedMaterial()
	{
		gameObject.GetComponent<Renderer>().material = unOcuppiedMaterial;
	}
	public void SetOccupiedMaterial()
	{
		gameObject.GetComponent<Renderer>().material = occupiedMaterial;
	}
	public void SetExtraSpaceMaterial()
	{
		gameObject.GetComponent <Renderer>().material = extraSpaceMaterial;
	}
}
