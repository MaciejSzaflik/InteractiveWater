using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinSim : RandomSim {

	public float scaleI = 1;
	public float scaleJ = 1;
	public float scaleT = 1;

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
				values[i][j] = Mathf.Max(0, Mathf.Sin(Time.time * scaleT+ i * scaleI+ j * scaleJ )*0.5f + 1);
			}
		}

		if(visualization != null)
			visualization.UpdateGrid(values);
	}

}
