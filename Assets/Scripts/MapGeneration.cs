using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
	public GameObject cube;
	public int x, y;

	private void Start()
	{
		for (int i = 0; i < x; i++)
		{
			for (int j = 0; j < y; j++)
			{
				GameObject item = Instantiate(cube, new Vector3(i, 0, j), Quaternion.identity, transform);
				item.name = i.ToString();
			}
		}
	}
}
