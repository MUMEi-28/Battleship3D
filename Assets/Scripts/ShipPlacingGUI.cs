using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipPlacingGUI : MonoBehaviour
{
	public TMP_Text notifyText;
	public GameObject playBtn;

	public void ChangeText(string text)
	{
		notifyText.text = text;	
	}
}
