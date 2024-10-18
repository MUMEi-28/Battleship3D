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
	public List<GameObject> horizontalList = new List<GameObject>();



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
	#region RANDOM GUESSING
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


			print("GUESS RANDOM");
		}
		// Find out weather the ship is horizontal or vertical
		else if (currentPhase == CurrentPhase.shipHit)
		{
			ChoosePotentialSurroundingHit();
		}
		// Continue hitting vertically
		else if (currentPhase == CurrentPhase.vertical)
		{
			GetVerticalTiles();

			ChooseVerticalSurroundingHit();
		}
		// Continue hitting horizontally
		else if (currentPhase == CurrentPhase.horizontal)
		{
			GetHorizontalTiles();

			ChooseHorizontalSurroundingHit();
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
	public void ChoosePotentialSurroundingHit()
	{

		// Return to guessing and clear out the vertical and horizontal tiles
		if (potentialHits.Count == 0)
		{
			currentPhase = CurrentPhase.guessing;
			verticalList.Clear();
			horizontalList.Clear();
			return;
		}


		// Choose a random tile on the four list
		int randomPotentialTile = Random.Range(0, potentialHits.Count);
		potentialTile = potentialHits[randomPotentialTile];


		// Home the bomb to the target
		
			enemyBomb.targetPosition = potentialTile.transform;
			bombTargetHeight.position = new Vector3(potentialTile.transform.position.x, 10, potentialTile.transform.position.z);

			print("GUESS SURROUNDING");

		// Remove the tile missed tile
		potentialHits.RemoveAt(randomPotentialTile);

		// Remove the missed tiles on the whole player board too
		playerTiles.Remove(potentialTile);

		// If the selected tile exists in the vertical list, remove it from there as well
		if (verticalList.Contains(potentialTile))
		{
			verticalList.Remove(potentialTile);
		}

		// If the selected tile exists in the horizontal list, remove it from there as well
		if (horizontalList.Contains(potentialTile))
		{
			horizontalList.Remove(potentialTile);
		}

		// If you hit more than 2 tiles consecutively in the same direction then continue in that direction
	}
	#endregion RANDOM GUESSING

	#region METHODS
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
		print("MISSED GUESSING AGAIN");

		currentHitCount = 0; // Reset the hit count on a miss
		currentPhase = CurrentPhase.guessing; // Return to random guessing mode
		potentialHits.Clear(); // Clear potential hits since we are guessing again
		verticalList.Clear(); // Clear out the vertical tiles
		horizontalList.Clear();

		// Reset the initial hits too
		initialHitObject = null;
		nextGuessTarget = null;
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
			verticalList.Clear();
			horizontalList.Clear();

			Debug.LogWarning("TILES CLEARED RESET EMPTY");
		}
	}
	#endregion METHODS

	#region VERTICAL GUESSING
	public void ChooseVerticalSurroundingHit()
	{
		// Return to guessing and clear out the vertical tiles
		if (verticalList.Count == 0)
		{
			currentPhase = CurrentPhase.guessing;
			verticalList.Clear();
			return;
		}


		// Choose a random tile on the four list
		int randomPotentialTile = Random.Range(0, verticalList.Count);
		potentialTile = verticalList[randomPotentialTile];

		// Home the bomb to the target

		enemyBomb.targetPosition = potentialTile.transform;
		bombTargetHeight.position = new Vector3(potentialTile.transform.position.x, 10, potentialTile.transform.position.z);



		// Remove the missed tiles on the whole player board too
		playerTiles.Remove(potentialTile);


		print("GUESS VERTICAL Removed:	" + verticalList[randomPotentialTile]);
		// Remove the tile missed tile	
		verticalList.RemoveAt(randomPotentialTile);

	}

	public void GetInitialVerticalTiles()
	{

		// Clear the potential tiles when locked
	//	potentialHits.Clear();

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

				// Check if the tile above or below still exist on the board and make sure that the list doesn't containt that object already
				if (playerTiles.Contains(verticalTiles) && !verticalList.Contains(verticalTiles))
				{
					potentialHits.Add(verticalTiles);
					verticalList.Add(verticalTiles);
				}

				// If the selected tile exists in the vertical list, remove it from there as well
				if (verticalList.Contains(potentialTile))
				{
					verticalList.Remove(potentialTile);
				}
			}
		}
	}
	#endregion VERTICAL GUESSING



	#region HORIZONTAL GUESSING
	public void ChooseHorizontalSurroundingHit()
	{
		// Return to guessing and clear out the horizontal tiles
		if (horizontalList.Count == 0)
		{
			currentPhase = CurrentPhase.guessing;
			horizontalList.Clear();
			return;
		}


		// Choose a random tile on the four list
		int randomPotentialTile = Random.Range(0, horizontalList.Count);
		potentialTile = horizontalList[randomPotentialTile];

		// Home the bomb to the target

		enemyBomb.targetPosition = potentialTile.transform;
		bombTargetHeight.position = new Vector3(potentialTile.transform.position.x, 10, potentialTile.transform.position.z);


		// Remove the missed tiles on the whole player board too
		playerTiles.Remove(potentialTile);

		print("GUESS HORIZONTAL Removed:	" + horizontalList[randomPotentialTile]);
		// Remove the tile missed tile
		horizontalList.RemoveAt(randomPotentialTile);

	}
	public void GetInitialHorizontalTiles()
	{

		// Define directions: Up, Down
		Vector3[] directions = new Vector3[]
		{
			Vector3.left, 
            Vector3.right,    
        };

		// Get the left and right tiles
		foreach (Vector3 direction in directions)
		{
			RaycastHit hit;
			Vector3 startPosition = initialHitObject.transform.position;

			Debug.DrawRay(startPosition, direction, Color.green, 9999f);

			if (Physics.Raycast(startPosition, direction, out hit, 1f, LayerMask.GetMask("PlayerTiles")))
			{
				GameObject horizontalTiles = hit.collider.gameObject;

				// Check if the tile above or below still exist on the board
				if (playerTiles.Contains(horizontalTiles))
				{
					horizontalList.Add(horizontalTiles);
				}
			}
		}
	}
	private void GetHorizontalTiles()
	{
		// Clear the potential tiles when locked
		potentialHits.Clear();

		// Define directions: Left, Right for horizontal checking
		Vector3[] directions = new Vector3[]
		{
			Vector3.left, // West
			Vector3.right // East
		};

		// Get the forward and backward tiles
		foreach (Vector3 direction in directions)
		{
			RaycastHit hit;
			Vector3 startPosition = nextGuessTarget.transform.position;

			Debug.DrawRay(startPosition, direction, Color.white, 9999f);

			if (Physics.Raycast(startPosition, direction, out hit, 1f, LayerMask.GetMask("PlayerTiles")))
			{
				GameObject horizontalTiles = hit.collider.gameObject;

				// Check if the tile above or below still exist on the board and make sure that the list doesn't containt that object already
				if (playerTiles.Contains(horizontalTiles) && !horizontalList.Contains(horizontalTiles))
				{
					potentialHits.Add(horizontalTiles);
					horizontalList.Add(horizontalTiles);
				}

				// If the selected tile exists in the horizontal list, remove it from there as well
				if (horizontalList.Contains(potentialTile))
				{
					horizontalList.Remove(potentialTile);
				}
			}
		}
	}
	#endregion

}




