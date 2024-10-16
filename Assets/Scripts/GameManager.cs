using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	placingShip,
	playerTurn,
	enemyTurn
}
public class GameManager : MonoBehaviour
{
	public GameState gameState;
	public TileTargetSelector tileTargetSelector;
	public ShipDragAndDrop shipDragAndDrop;

	[Header("TESTING")]
	public Camera placingShipCam;
	public Camera gameCam;

	private void Start()
	{
		UpdateCamera();
	}

	public void UpdateCamera()
	{
		if (gameState == GameState.placingShip)
		{
			shipDragAndDrop.enabled = true;
			tileTargetSelector.enabled = false;

			placingShipCam.enabled = true;
			gameCam.enabled = false;
		}
		else if (gameState == GameState.playerTurn)
		{
			shipDragAndDrop.enabled = false;
			tileTargetSelector.enabled = true;

			placingShipCam.enabled = false;
			gameCam.enabled = true;
		}
	}
}

