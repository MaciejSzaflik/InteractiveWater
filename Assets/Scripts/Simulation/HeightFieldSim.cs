using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightFieldSim : RandomSim {


	protected float[][] velocites;
	public float dumpValue = 0.99f;

	public float powerAddMax = 5;
	public float iterAddPower = 3;

	public bool simple = true;

	public float waveSpeed = 3;
	public float columnSize = 1;
	public float timeStep = 0.1f;

	public float[][] tempValues;

	public override void Init ()
	{
		velocites = new float[countX][];
		values = new float[countX][];
		tempValues = new float[countX][];

		for(int i = 0;i<countX;i++)
		{
			velocites[i] = new float[countY];
			values[i] = new float[countY];
			tempValues[i] = new float[countY];

			for(int j = 0;j<countY;j++)
				values[i][j] = velocites[i][j] = 0;
		}

		if(visualization != null)
			visualization.Init(countX,countY,AddValueAtPoint);
	}

	public void ResetForces()
	{
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
				values[i][j] = velocites[i][j] = 0;
		}
	}


	private void AddValueAtPoint(Point p)
	{
		values[p.x][p.y] = Mathf.Min(powerAddMax,values[p.x][p.y]+iterAddPower);
	}

	void Update()
	{
		columnSize = Mathf.Max(columnSize,0.00001f);
		waveSpeed = Mathf.Max(waveSpeed,0.00001f);
		timeStep = Mathf.Max(timeStep,0.00001f);

		timeStep = Mathf.Min(timeStep,columnSize/waveSpeed - 0.01f);
		waveSpeed = Mathf.Min(waveSpeed,columnSize/timeStep - 0.01f);

		if(simple)
			BasicUpdateValues();
		else
			ComplexeUpdateValues();

		if(visualization != null)
			visualization.UpdateGrid(values);
	}

	private void ComplexeUpdateValues()
	{
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				float force = values[Mathf.Max(0,i - 1)][j] + 
					values[Mathf.Min(i + 1,countX-1)][j] + 
					values[i][Mathf.Max(0,j - 1)] +
					values[i][Mathf.Min(j + 1,countY-1)];
				force-= 4*values[i][j];
				force*=waveSpeed*waveSpeed;
				force/=columnSize*columnSize;

				velocites[i][j] += force*timeStep;
				tempValues[i][j] = values[i][j] + velocites[i][j]*timeStep;
			
				velocites[i][j] *= dumpValue;
			}
		}
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				values[i][j] = tempValues[i][j];
			}
		}
	}


	private void BasicUpdateValues()
	{
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				velocites[i][j] += 
					(values[Mathf.Max(0,i - 1)][j] + 
						values[Mathf.Min(i + 1,countX-1)][j] + 
						values[i][Mathf.Max(0,j - 1)] +
						values[i][Mathf.Min(j + 1,countY-1)])/4 - values[i][j];

				velocites[i][j] *= dumpValue;
			}
		}
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				values[i][j] += velocites[i][j];
			}
		}
	}

}
