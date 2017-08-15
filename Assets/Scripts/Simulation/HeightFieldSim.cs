using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class HeightFieldSim : RandomSim {

	protected float[][] velocities;
	public float dampValue = 0.99f;

	public float powerAddMax = 5;
	public float iterAddPower = 3;

	public bool simple = true;
	public bool clamp = true;

	public float waveSpeed = 3;
	public float columnSize = 1;
	public float timeStep = 0.1f;

	public float[][] tempValues;

	public override void Init ()
	{
		velocities = new float[countX][];
		values = new float[countX][];
		tempValues = new float[countX][];

		for(int i = 0;i<countX;i++)
		{
			velocities[i] = new float[countY];
			values[i] = new float[countY];
			tempValues[i] = new float[countY];

			for(int j = 0;j<countY;j++)
				values[i][j] = velocities[i][j] = 0;
		}

		if(visualization != null)
			visualization.Init(countX,countY,AddValueAtPoint);
	}

	public void ResetForces()
	{
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
				values[i][j] = velocities[i][j] = 0;
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
			ComplexUpdateValues();

		if(visualization != null)
			visualization.UpdateGrid(values);

	}

	private void ComplexUpdateValues ()
	{
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				float force = clamp? GetClamped(i,j) : GetWrapped(i,j);

				force-= 4*values[i][j];
				force*=waveSpeed*waveSpeed;
				force/=columnSize*columnSize;

				velocities[i][j] += force*timeStep;
				tempValues[i][j] = values[i][j] + velocities[i][j]*timeStep;
			
				velocities[i][j] *= dampValue;
			}
		}
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				values[i][j] = Mathf.Max(0,tempValues[i][j]);
			}
		}
	}

	private float GetClamped(int i ,int j)
	{
		return values[Mathf.Max(0,i - 1)][j] + 
			values[Mathf.Min(i + 1,countX-1)][j] + 
			values[i][Mathf.Max(0,j - 1)] +
			values[i][Mathf.Min(j + 1,countY-1)];
	}

	private float GetWrapped(int i ,int j)
	{
		return values[(i - 1) < 0 ? countX - 1 : i - 1][j] + 
			   values[(i + 1) >= countX ? 0: i + 1][j] + 
			   values[i][(j - 1) < 0 ? countY - 1 : j - 1] +
			   values[i][(j + 1) >= countY ? 0: j + 1];
	}



	private void BasicUpdateValues()
	{
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				velocities[i][j] += (values[Mathf.Max(0,i - 1)][j] + 
						values[Mathf.Min(i + 1,countX-1)][j] + 
						values[i][Mathf.Max(0,j - 1)] +
						values[i][Mathf.Min(j + 1,countY-1)])/4 - values[i][j];

				velocities[i][j] *= dampValue;
			}
		}
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				values[i][j] += velocities[i][j];
			}
		}
	}

	//for latex...
	public override string ToString ()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("Values: ");
		Array.ForEach(values,(array)=>{
			sb.Append(Stringfy.RoundedArray(array));
			sb.AppendLine(" \\\\");
		});
		sb.AppendLine("Velocities: ");
		Array.ForEach(velocities,(array)=>{
			sb.Append(Stringfy.RoundedArray(array));
			sb.AppendLine(" \\\\");
		});
		return sb.ToString();
	}

}
