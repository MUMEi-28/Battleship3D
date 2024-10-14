using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CurrentPhase
{
	guessing,
	vertical,
	horizontal
}

public class EnemyAi : MonoBehaviour
{
	public CurrentPhase currentPhase;
	public List<GameObject> playerTiles;
	public Transform bombTargetHeight;



	public EnemyBomb enemyBomb;
	public GameObject chosenTile;


	public bool hitTest;
	private void Update()
	{
		if (hitTest)
		{
			GuessTile();
			hitTest = false;
		}
	}
	public GameObject GuessTile()
	{
		// Ensure there are still tiles to choose from
		if (playerTiles.Count == 0)
		{
			Debug.Log("No more tiles to guess.");
			return null;
		}

		// Choose a random tile index
		int randomTileIndex = Random.Range(0, playerTiles.Count);

		// Get the tile at the random index
		chosenTile = playerTiles[randomTileIndex];

		// Set the target for the bomb
		enemyBomb.targetPosition = chosenTile.transform;

		// Position the bomb target right above the target
		bombTargetHeight.position = new Vector3(chosenTile.transform.position.x, 10, chosenTile.transform.position.z);

		// Remove the chosen tile from the list to prevent guessing it again
		playerTiles.RemoveAt(randomTileIndex);

		return chosenTile;
	}

	/*public bool isTileEmpty()
	{

	}*/
}

