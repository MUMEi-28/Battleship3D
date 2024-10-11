using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{

	public Material occupiedMaterial;
	public Material extraSpaceMaterial;
	public Material unOcuppiedMaterial;

	public LayerMask shipMask;


	private void FixedUpdate()
	{
		ChangeMaterial();
	}

	#region Changing Materials
	private void ChangeMaterial()
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

	private void SetUnocupiedMaterial()
	{
		gameObject.GetComponent<Renderer>().material = unOcuppiedMaterial;
	}
	private void SetOccupiedMaterial()
	{
		gameObject.GetComponent<Renderer>().material = occupiedMaterial;
	}
	private void SetExtraSpaceMaterial()
	{
		gameObject.GetComponent <Renderer>().material = extraSpaceMaterial;
	}
	#endregion Changing Materials
}
