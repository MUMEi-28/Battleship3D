using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDragAndDrop : MonoBehaviour
{
	public Vector3 mousePosition;
	public Camera mainCam;

	public bool isSelected;
	public LayerMask shipMask;
	private void Start()
	{
		mainCam = Camera.main;
	}

	private void Update()
	{
		RaycastHit hit;
		if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, shipMask))
		{
			GameObject hitObject = hit.collider.gameObject;

			if (Input.GetMouseButtonDown(0))
			{
				hitObject.GetComponent<ShipController>().isSelected = true;
			}
		}
	}

}
