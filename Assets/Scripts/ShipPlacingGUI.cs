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
	private Camera mainCam;
	public float placingShipCameraSize;
	public float gameCameraSize;
	public Transform camGamePosition;

	private void Start()
	{
		gameCameraSize = Camera.main.orthographicSize;
		mainCam = Camera.main;
	}

	public void ChangeText(string text)
	{
		notifyText.text = text;	
	}

	public void OnClickPlay()
	{
		mainCam.transform.rotation = camGamePosition.rotation;
		mainCam.transform.position = camGamePosition.position;

		mainCam.orthographicSize = gameCameraSize;
	}
}
