using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour 
{
	public GameObject prefabToSpawn;
	public GameObject spawnerRoot;

	public int sizeX = 0;
	public int sizeZ = 0;

	private GameObject[][] spawnedElements;

	private float sizeXElement;
	private float sizeZElement;

	public void Init(int countX, int countY)
	{
		spawnedElements = new GameObject[countX][];

		sizeXElement = (float)sizeX/countX;
		sizeZElement = (float)sizeZ/countY;

		for(int i = 0;i<countX;i++)
		{
			spawnedElements[i] = new GameObject[countY];
			for(int j = 0;j<countY;j++)
			{
				GameObject obj = Instantiate(prefabToSpawn) as GameObject;
				obj.name = string.Format("{0}.{1}.{2}",prefabToSpawn.name,i,j);
				spawnedElements[i][j] = obj;
				obj.transform.SetParent(spawnerRoot.transform);
				obj.transform.localScale = new Vector3(sizeXElement,1,sizeZElement);
				obj.transform.localPosition = new Vector3(i*sizeXElement,1,j*sizeZElement);
			}
		}
	}

	public void UpdateGrid(float[][] value)
	{
		for(int i = 0;i<value.Length;i++)
		{
			for(int j = 0;j<value[i].Length;j++)
			{
				spawnedElements[i][j].transform.localScale = new Vector3(sizeXElement,value[i][j],sizeZElement);
				float positionY = (value[i][j] - 1)/2;
				Vector3 currentPosition = spawnedElements[i][j].transform.localPosition;
				currentPosition.y = positionY;
				spawnedElements[i][j].transform.localPosition = currentPosition;
			}
		}
	}
}
