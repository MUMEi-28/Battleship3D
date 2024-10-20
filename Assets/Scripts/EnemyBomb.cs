using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : Bomb
{
	public EnemyAi enemyAi;
	public ShipController shipController;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ship"))
		{
			shipController = other.GetComponent<ShipController>();

			if (shipController.isBigShip)
			{
				print("HIT BIG SHIP");
			}
		}

		// If the bomb hits an EnemyShip
		if (other.CompareTag("PlayerShip"))
		{
			// Instantiate fire particles at the ship's position
			Instantiate(fireParticles, other.transform.position, Quaternion.identity);

			// Find the tile underneath the ship and handle it separately
			GameObject tileBelow = GetTileBelow(other.transform, "PlayerTiles");
			if (tileBelow != null)
			{
				// Change the layer and material for the tile
				tileBelow.gameObject.layer = LayerMask.NameToLayer("Default");
				tileBelow.GetComponent<Renderer>().material = shipHitMaterial;

				// Set the initial object (EnemyAi)	
				if (enemyAi.initialHitObject == null)
				{
					enemyAi.initialHitObject = tileBelow;
					enemyAi.nextGuessTarget = tileBelow;
					enemyAi.GetInitialVerticalTiles();
					enemyAi.GetInitialHorizontalTiles();

					print("SET INITIAL TILES ON NULL INITIAL HIT");
				}
				else
				{
					enemyAi.nextGuessTarget = tileBelow;
				}
			}




			// If enemy hit a ship then guess potential hit
			if (enemyAi.currentPhase == CurrentPhase.guessing) // Only Get the surrounding tiles if enemy is guessing
			{
				enemyAi.GuessPotentialHits(other.transform.gameObject);
				enemyAi.currentPhase = CurrentPhase.shipHit;
			}



			// Reset the bomb
			ResetBomb();



			// Add hit count to find out if the ship is vertical or horizontal
			enemyAi.RegisterHit();


			if (!shipController.isBigShip)
			{
				enemyAi.ResetGuessOnEmptyTile();
			}

		}
		// If the bomb directly hits a TargetTile without hitting a ship
		else if (other.CompareTag("PlayerTiles"))
		{
			// Instantiate water splash particle at the tile's position
			Instantiate(waterSplashParticle, other.transform.position, Quaternion.Euler(-90, 0, 0));

			// Change the layer and material for the tile
			other.gameObject.layer = LayerMask.NameToLayer("Default");
			other.GetComponent<Renderer>().material = tileHitMaterial;

			// Reset the bomb
			ResetBomb();


			// If enemy hit a tile then continue guessing
			if (enemyAi.currentPhase == CurrentPhase.vertical)
			{
				// Clear only if the ship is not big
				if (!shipController.isBigShip)
				{
					// Only go back to guessing if there are no more tiles to guess on the vertical list
					if (enemyAi.verticalList.Count <= 0)
					{
						enemyAi.RegisterMiss();
					}
				}
				else
				{
					print("TURN HORIZONTAL ON THE BIG SHIP");
					//	print("HIT NEXT TILES ON THE BIG SHIP");
					enemyAi.currentPhase = CurrentPhase.horizontal;
				}
			}

			// If enemy hit a tile then continue guessing
			if (enemyAi.currentPhase == CurrentPhase.horizontal)
			{
				// Clear only if the ship is not big
				if (!shipController.isBigShip)
				{
					// Only go back to guessing if there are no more tiles to guess on the horizontal list
					if (enemyAi.horizontalList.Count <= 0)
					{
						enemyAi.RegisterMiss();
					}
				}
				else
				{
					print("TURN VERTICAL ON THE BIG SHIP");
					enemyAi.currentPhase = CurrentPhase.vertical;
				}
			}
		}

	}
}




