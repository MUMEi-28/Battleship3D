using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[Header("Bomb Ascending|Descending")]
	public Transform targetPosition;
	public Vector3 defaultPosition;
	public Transform heightTargetPosition;
	public float acceleration = 0.1f;
	public float maxSpeed = 5f;
	private float currentSpeed = 0f;
	public bool isAscending = false;
	public float waitTimeBeforeDescending = 1.0f;
	public TileTargetSelector targetSelector;

	[Header("Bomb Hit")]
	public Material tileHitMaterial;
	public Material shipHitMaterial;
	public GameObject fireParticles;
	public GameObject waterSplashParticle;


	private void Start()
	{
		defaultPosition = transform.position;
	}

	private void Update()
	{
		if (targetPosition != null)
		{
			// Increase the speed over time up to max speed
			currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

			if (isAscending)
			{
				// Disable mouse to prevent changing target when the bomb has flown
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				Vector3 direction = (heightTargetPosition.position - transform.position).normalized;

				// Move the bomb upwards until it reaches the specified height
				transform.position += direction * currentSpeed * Time.deltaTime;

				// Check if the bomb has reached the specified height
				if (Vector3.Distance(transform.position, heightTargetPosition.position) <= 0.1f)
				{
					isAscending = false; // Stop the ascending phase
					StartCoroutine(StartDescendingAfterDelay());
				}
			}
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

		}

		if (isAscending && targetPosition != null)
		{
			// Look towards the height target while ascending
			transform.LookAt(heightTargetPosition.position);
		}
		else if (!isAscending && targetPosition != null)
		{
			// Look towards the final target while descending
			transform.LookAt(targetPosition.position);
		}
		else
		{
			// Reset rotation when there's no target
			transform.rotation = Quaternion.Euler(Vector3.zero);
		}
	}

	private IEnumerator StartDescendingAfterDelay()
	{
		// Wait for the specified time before starting the descent
		yield return new WaitForSeconds(waitTimeBeforeDescending);

		// Begin the descent towards the target position
		while (targetPosition != null)
		{
			Vector3 direction = (targetPosition.position - transform.position).normalized;

			// Move the object towards the target
			transform.position += direction * currentSpeed * Time.deltaTime;

			yield return null; // Wait until the next frame
		}
	}

	

	// Helper method to get the tile below the ship if necessary
	public GameObject GetTileBelow(Transform shipTransform, string layerMask)
	{
		RaycastHit hit;
		Ray downRay = new Ray(shipTransform.position, Vector3.down);
		if (Physics.Raycast(downRay, out hit, Mathf.Infinity, LayerMask.GetMask(layerMask)))
		{
			return hit.collider.gameObject;
		}
		return null;
	}

	// Reposition the bomb when collided with the ship | tile
	public void ResetBomb()
	{
		targetPosition = null;
		isAscending = true; // Reset for the next shot
		currentSpeed = 0f; // Reset speed
		transform.position = defaultPosition;
	}
}

