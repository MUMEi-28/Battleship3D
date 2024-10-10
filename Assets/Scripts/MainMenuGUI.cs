using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGUI : MonoBehaviour
{
	public void OnClickSingleplayer()
	{
		SceneManager.LoadScene("Game");
	}
	public void OnClickMultiplayer()
	{
		Debug.LogWarning("Add Multiplayer late");
	}
	public void OnClickQuit()
	{
		Application.Quit();
		Debug.Log("Quit");
	}
}
