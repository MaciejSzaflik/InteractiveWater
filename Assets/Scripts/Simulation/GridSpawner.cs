using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : Vizualisation {
	public GameObject prefabToSpawn;
	public GameObject spawnerRoot;

	public int sizeX = 0;
	public int sizeZ = 0;

	private GameObject[][] spawnedElements;

	private float sizeXElement;
	private float sizeZElement;

	public override void Init (int countX, int countY, System.Action<Point> onInteraction)
	{
		base.Init(countX, countY, onInteraction);

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

		CreateCollider();
		OnMouseInteraction(onInteraction);
	}

	private void CreateCollider()
	{
		minPosition = new Vector2(
			spawnedElements[0][0].transform.position.x,
			spawnedElements[0][0].transform.position.z
		);

		maxPosition = new Vector2(
			spawnedElements[countX-1][countY-1].transform.position.x,
			spawnedElements[countX-1][countY-1].transform.position.z
		);

		boxCollider = this.gameObject.AddComponent<BoxCollider>();

		boxCollider.center = new Vector3(
			(maxPosition.x - minPosition.x)/2,
			0,
			(maxPosition.y - minPosition.y)/2);
		boxCollider.size = boxCollider.center*2 + new Vector3(sizeXElement,1,sizeZElement);
	}

	protected override Point GetCellFromPosition(Vector3 pos)
	{
		int x = Mathf.FloorToInt((pos.x - minPosition.x)/sizeXElement);
		int z = Mathf.FloorToInt((pos.z - minPosition.y)/sizeZElement);

		return new Point(x,z);
	}

	public override void UpdateGrid(float[][] value)
	{
		for(int i = 0;i<value.Length;i++)
		{
			for(int j = 0;j<value[i].Length;j++)
			{
				spawnedElements[i][j].transform.localScale = new Vector3(sizeXElement,value[i][j] + 0.1f,sizeZElement);
				float positionY = (value[i][j] - 1)/2;
				Vector3 currentPosition = spawnedElements[i][j].transform.localPosition;
				currentPosition.y = positionY;
				spawnedElements[i][j].transform.localPosition = currentPosition;
			}
		}
	}


}
