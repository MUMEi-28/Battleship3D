using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
	public bool isSelected;
	public int rotateShipIndex = 0;
	public float rotationSpeed;


	private void Update()
	{
		if (isSelected)
		{
			MoveWithMouse();

			// Can only rotate if holding a ship
			RotateShip();

		}

	}
	private void MoveWithMouse()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
		transform.position = new Vector3(worldPos.x, 1, worldPos.z);
	}
	private void RotateShip()
	{
		float scrollInput = Input.GetAxis("Mouse ScrollWheel");

		if (scrollInput >= 0.1f)
		{
			rotateShipIndex++;
		}
		if (scrollInput <= -0.1f)
		{
			rotateShipIndex--;
		}

		
		// Redo the rotation
		if (rotateShipIndex > 3)
		{
			rotateShipIndex = 0;
		}
		else if(rotateShipIndex < 0)
		{
			rotateShipIndex = 3;
		}


		// Rotate the ship
		switch (rotateShipIndex)
		{
			case 0:
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), rotationSpeed * Time.deltaTime);
				break;
			case 1:
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 90, 0)), rotationSpeed * Time.deltaTime);
				break;
			case 2:
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 180, 0)), rotationSpeed * Time.deltaTime);
				break;
			case 3:
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 270, 0)), rotationSpeed * Time.deltaTime);
				break;

			default:
				break;
		}

	}
}