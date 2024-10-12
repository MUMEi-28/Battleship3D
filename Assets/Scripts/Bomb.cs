using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public Transform targetPosition;
	public Vector3 defaultPosition;
	public float bombSpeed;


	public float acceleration = 0.1f; // Adjust this value for faster/slower acceleration
	public float maxSpeed = 5f; // The maximum speed

	private float currentSpeed = 0f;

	private void Start()
	{
		defaultPosition = transform.position;
	}

	private void Update()
	{
		if (targetPosition != null)
		{
			Vector3 direction = (targetPosition.position - transform.position).normalized;

			// Increase the speed over time up to max speed
			currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

			// Move the object
			transform.position += direction * currentSpeed * Time.deltaTime;

			// Return to orignal position if near the target position
			if (Vector3.Distance(transform.position, targetPosition.position) <= 0.1f)
			{
				transform.position = defaultPosition;
				targetPosition = null;
				print("BOMB HIT A TILE");

			}
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		// Reposition the bomb when collided with the ship
		if (other.CompareTag("EnemyShip"))
		{
			transform.position = defaultPosition;
			targetPosition = null;
			print("BOMB HIT A SHIP");
		}

	}
}
