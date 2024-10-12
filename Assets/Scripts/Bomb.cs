using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public Transform targetPosition;
	public float bombSpeed;


	public float acceleration = 0.1f; // Adjust this value for faster/slower acceleration
	public float maxSpeed = 5f; // The maximum speed

	private float currentSpeed = 0f;

	private void Update()
	{
		if (targetPosition != null)
		{
			Vector3 direction = (targetPosition.position - transform.position).normalized;

			// Increase the speed over time up to max speed
			currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

			// Move the object
			transform.position += direction * currentSpeed * Time.deltaTime;
		}
	}
}
