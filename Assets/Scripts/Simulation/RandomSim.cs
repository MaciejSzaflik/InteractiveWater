using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSim : MonoBehaviour {

	public GridSpawner visualization;

	public int countX = 0;
	public int countY = 0;

	protected float [][] values;

	public virtual void Init()
	{
		values = new float[countX][];

		for(int i = 0;i<countX;i++)
		{
			values[i] = new float[countY];
			for(int j = 0;j<countY;j++)
			{
				values[i][j] = 1;
			}
		}

		if(visualization != null)
			visualization.Init(countX,countY);
	}

	void Start()
	{
		Init();
	}

	void Update()
	{
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				values[i][j] = Mathf.Max(
					0, 
					values[i][j] + Random.Range(-0.1f,0.1f));
			}
		}

		if(visualization != null)
			visualization.UpdateGrid(values);
	}

}
