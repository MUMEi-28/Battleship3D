using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
	public Material occupiedMaterial;
	public Material extraSpaceMaterial;
	public Material unOcuppiedMaterial;

	#region Changing Materials
	public void SetUnocupiedMaterial(GameObject obj)
	{
		obj.GetComponent<Renderer>().material = unOcuppiedMaterial;
	}
	public void SetOccupiedMaterial(GameObject obj)
	{
		obj.GetComponent<Renderer>().material = occupiedMaterial;
	}
	public void SetExtraSpaceMaterial(GameObject obj)
	{
		obj.GetComponent<Renderer>().material = extraSpaceMaterial;
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
		gameObject.GetComponent<Renderer>().material = extraSpaceMaterial;
	}
	#endregion Changing Materials
}
