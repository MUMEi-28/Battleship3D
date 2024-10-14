using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipPlacingGUI : MonoBehaviour
{
	public TMP_Text notifyText;
	public GameObject playBtn;

	[Header("Camera")]
	public Camera gameCamera;
	public Camera placingShipCamera;
	public GameManager gameManager;
	public void ChangeText(string text)
	{
		notifyText.text = text;	
	}

	// REPOSITION THE CAMERA AND SET THE GAME STATE TO PLAYER TURN
	public void OnClickPlay()
	{
		placingShipCamera.enabled = false;
		gameCamera.enabled = true;

		gameManager.gameState = GameState.playerTurn;
		gameManager.UpdateCamera();
	}
}
