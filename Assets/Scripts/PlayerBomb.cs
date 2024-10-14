using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : Bomb
{
	private void OnTriggerEnter(Collider other)
	{
		// If the bomb hits an EnemyShip
		if (other.CompareTag("EnemyShip"))
		{
			// Instantiate fire particles at the ship's position
			Instantiate(fireParticles, other.transform.position, Quaternion.identity);

			// Find the tile underneath the ship and handle it separately
			GameObject tileBelow = GetTileBelow(other.transform, "TargetTiles");
			if (tileBelow != null)
			{
				// Change the layer and material for the tile
				tileBelow.gameObject.layer = LayerMask.NameToLayer("Default");
				tileBelow.GetComponent<Renderer>().material = shipHitMaterial;
			}

			// Reset the bomb
			ResetBomb();
		}
		// If the bomb directly hits a TargetTile without hitting a ship
		else if (other.CompareTag("TargetTile"))
		{
			// Instantiate water splash particle at the tile's position
			Instantiate(waterSplashParticle, other.transform.position, Quaternion.Euler(-90, 0, 0));

			// Change the layer and material for the tile
			other.gameObject.layer = LayerMask.NameToLayer("Default");
			other.GetComponent<Renderer>().material = tileHitMaterial;

			// Reset the bomb
			ResetBomb();
		}
	}
}
