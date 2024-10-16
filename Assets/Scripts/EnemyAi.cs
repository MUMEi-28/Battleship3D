using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public enum CurrentPhase
{
	guessing,
	shipHit,
	vertical,
	horizontal
}

public class EnemyAi : MonoBehaviour
{
	public CurrentPhase currentPhase;
	public List<GameObject> playerTiles;
	public Transform bombTargetHeight;

	public EnemyBomb enemyBomb;
	public GameObject guessTile;
	public bool isTileEmpty;

	[Header("Potential hit")]
	public List<GameObject> potentialHits = new List<GameObject>();
	public GameObject potentialTile;
	public int currentHitCount;

	[Header("Vertical | Horizontal Item")]
	public GameObject initialHitObject;
	public GameObject nextGuessTarget; // This is the guess if either forward or backward to target
	public List<GameObject> verticalList = new List<GameObject>();

	public bool hitTest;
	private void Update()
	{
		if (hitTest)
		{
				
			GuessTile();
			//	ChoosePotentialHit();

			hitTest = false;
		}
	}

	// Returns the most likely tile that have a ship
	public void GuessTile()
	{

		// Find a tile 
		if (currentPhase == CurrentPhase.guessing)
		{
			// Choose a random tile index
			int randomTileIndex = Random.Range(0, playerTiles.Count);

			// Get the tile at the random index
			guessTile = playerTiles[randomTileIndex];

			// Set the target for the bomb
			enemyBomb.targetPosition = guessTile.transform;

			// Position the bomb target right above the target
			bombTargetHeight.position = new Vector3(guessTile.transform.position.x, 10, guessTile.transform.position.z);

			// Remove the chosen tile from the list to prevent guessing it again
			playerTiles.RemoveAt(randomTileIndex);

			// If the enemy hit a ship then Get the Surrounding Tiles on that hit to guess the next potential hit
		}
		// Find out weather the ship is horizontal or vertical
		else if (currentPhase == CurrentPhase.shipHit)
		{
			ChoosePotentialHit();
		}
		// Continue hitting vertically
		else if (currentPhase == CurrentPhase.vertical)
		{
			Debug.LogWarning("TILES CLEARED ON UPDATE VERTICAL");

			GetVerticalTiles();

			ChoosePotentialHit();
		}
		// Continue hitting horizontally
		else if (currentPhase == CurrentPhase.horizontal)
		{
			print("Horizontal");
		}
		else
		{
			print("OTHER OUTPUT");
		}

	}



	// Find the four surrounding tiles of the hit tile (Enemy Bomb)
	public List<GameObject> GuessPotentialHits(GameObject hitTile)
	{
		// Define directions: Up, Down, Left, Right
		Vector3[] directions = new Vector3[]
		{
			Vector3.forward, // Up (North)
            Vector3.back,    // Down (South)
            Vector3.left,    // Left (West)
            Vector3.right    // Right (East)
        };

		// Loop through each direction and check for tiles
		foreach (Vector3 direction in directions)
		{
			RaycastHit hit;
			Vector3 startPosition = hitTile.transform.position + Vector3.down;

			// Draw the ray in the Scene view for debugging (yellow color)
			Debug.DrawRay(startPosition, direction, Color.blue, 9999f);

			// Raycast to find the adjacent tile in the given direction
			if (Physics.Raycast(startPosition, direction, out hit, 1f, LayerMask.GetMask("PlayerTiles")))
			{
				GameObject adjacentTile = hit.collider.gameObject;

				// Check if the tile is in the list of available player tiles
				if (playerTiles.Contains(adjacentTile))
				{
					potentialHits.Add(adjacentTile);
				}
			}

		}

		// Print all potential hit game object names
		/*		string potentialHitsNames = "POTENTIAL HITS ARE: ";
				foreach (GameObject potentialHit in potentialHits)
				{
					potentialHitsNames += potentialHit.name + ", ";
				}
				Debug.Log(potentialHitsNames);*/


		return potentialHits;
	}

	// Choose a tile on the four surrounding hit tile
	public void ChoosePotentialHit()
	{
		if (potentialHits.Count == 0)
		{
			currentPhase = CurrentPhase.guessing;
			print("NO MORE POTENTIAL TILES");

			return;
		}


		// Choose a random tile on the four list
		int randomPotentialTile = Random.Range(0, potentialHits.Count);
		potentialTile = potentialHits[randomPotentialTile];


		// Home the bomb to the target
		enemyBomb.targetPosition = potentialTile.transform;
		bombTargetHeight.position = new Vector3(potentialTile.transform.position.x, 10, potentialTile.transform.position.z);


		// Remove the tile missed tile
		potentialHits.RemoveAt(randomPotentialTile);

		// If the selected tile exists in the vertical list, remove it from there as well
		if (verticalList.Contains(potentialTile))
		{
			verticalList.Remove(potentialTile);
			Debug.Log($"Removed {potentialTile.name} from vertical list.");
		}


		if (potentialHits.Count == 0)
		{
		//	currentPhase = CurrentPhase.guessing;
			print("LAST POTENTIAL TILES IS DESTROYED");
		}

		// If you hit more than 2 tiles consecutively in the same direction then continue in that direction
	}

	// Method to call when a hit is confirmed (from EnemyBomb)
	public void RegisterHit()
	{
		currentHitCount++;

		// If hit count is 2 or more, lock the direction based on the last hit
		if (currentHitCount >= 2)
		{
			LockDirection();
		}
	}

	// Method to call when a miss occurs (from EnemyBomb)
	public void RegisterMiss()
	{
		currentHitCount = 0; // Reset the hit count on a miss
		currentPhase = CurrentPhase.guessing; // Return to random guessing mode
		potentialHits.Clear(); // Clear potential hits since we are guessing again
	}
	// Lock direction based on the hits
	private void LockDirection()
	{
		Vector3 directionDifference = potentialTile.transform.position - guessTile.transform.position;

		// Determine if the locked direction should be vertical or horizontal
		if (Mathf.Abs(directionDifference.x) > Mathf.Abs(directionDifference.z))
		{
			currentPhase = CurrentPhase.horizontal;
		}
		else
		{
			currentPhase = CurrentPhase.vertical;
			Debug.LogWarning("TILES CLEARED ON LOCK DIRECTION");

	//		GetInitialVerticalTiles();
			GetVerticalTiles();
		}
	}

	// ENEMY BOMB METHOD
	// Reset to guessing if there are no more potential tiles for example if the tile was on edge
	public void ResetGuessOnEmptyTile()
	{
		if (potentialHits.Count == 0)
		{
			currentPhase = CurrentPhase.guessing;
			potentialHits.Clear();
			Debug.LogWarning("TILES CLEARED ON ON POTENTIAL HIT = 0");
		}
	}
	public void GetInitialVerticalTiles()
	{

		// Clear the potential tiles when locked
		potentialHits.Clear();

		// Define directions: Up, Down
		Vector3[] directions = new Vector3[]
		{
			Vector3.forward, // Up (North)
            Vector3.back,    // Down (South)
        };

		// Get the forward and backward tiles
		foreach (Vector3 direction in directions)
		{
			RaycastHit hit;
			Vector3 startPosition = initialHitObject.transform.position;

			Debug.DrawRay(startPosition, direction, Color.green, 9999f);

			if (Physics.Raycast(startPosition, direction, out hit, 1f, LayerMask.GetMask("PlayerTiles")))
			{
				GameObject verticalTiles = hit.collider.gameObject;

				// Check if the tile above or below still exist on the board
				if (playerTiles.Contains(verticalTiles))
				{
					verticalList.Add(verticalTiles);
				}


			}
		}
	}
	private void GetVerticalTiles()
	{
		// Clear the potential tiles when locked
		potentialHits.Clear();

		// Define directions: Up, Down
		Vector3[] directions = new Vector3[]
		{
			Vector3.forward, // Up (North)
            Vector3.back,    // Down (South)
        };

		// Get the forward and backward tiles
		foreach (Vector3 direction in directions)
		{
			RaycastHit hit;
			Vector3 startPosition = nextGuessTarget.transform.position;

			Debug.DrawRay(startPosition, direction, Color.green, 9999f);

			if (Physics.Raycast(startPosition, direction, out hit, 1f, LayerMask.GetMask("PlayerTiles")))
			{
				GameObject verticalTiles = hit.collider.gameObject;

				// Check if the tile above or below still exist on the board
				if (playerTiles.Contains(verticalTiles))
				{

					potentialHits.Add(verticalTiles);
					verticalList.Add(verticalTiles);
				}

				// If the selected tile exists in the vertical list, remove it from there as well
				if (verticalList.Contains(potentialTile))
				{
					verticalList.Remove(potentialTile);
					Debug.Log($"Removed {potentialTile.name} from vertical list.");
				}
			}
		}
	}

	private void HitHorizontal()
	{

	}
}




