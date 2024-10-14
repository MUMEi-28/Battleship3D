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
	public GameObject[] playerTiles;



	public GameObject bombPrefab;
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
		int randomTile = Random.Range(0, playerTiles.Length);

		chosenTile = playerTiles[randomTile];

		return playerTiles[randomTile];
	}

	/*public bool isTileEmpty()
	{

	}*/
}

