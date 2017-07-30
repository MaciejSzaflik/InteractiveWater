using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : Vizualisation {

	public float colorDiv = 2;
	public float spacing = 0.25f;
	public ParticleSystem particles;
	public Gradient gradient;
	public float scaleY;

	private ParticleSystem.Particle[] particleGrid;

	public override void Init (int countX, int countY, System.Action<Point> onInteraction)
	{
		base.Init(countX, countY, onInteraction);

		particles.Emit(countX*countY);
		particleGrid = new ParticleSystem.Particle[countX*countY];
		particles.GetParticles(particleGrid);
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				particleGrid[i + j*countY].position = new Vector3(i * spacing, 0, j * spacing);
			}
		}
		particles.SetParticles(particleGrid,particleGrid.Length);

		CreateCollider();
	}

	protected void CreateCollider()
	{
		minPosition = new Vector2(
			particleGrid[0].position.x,
			particleGrid[0].position.z
		);

		maxPosition = new Vector2(
			particleGrid[particleGrid.Length-1].position.x,
			particleGrid[particleGrid.Length-1].position.z
		);

		boxCollider = this.gameObject.AddComponent<BoxCollider>();

		boxCollider.center = new Vector3(
			(maxPosition.x - minPosition.x)/2,
			0,
			(maxPosition.y - minPosition.y)/2);
		boxCollider.size = boxCollider.center*2 + new Vector3(spacing*2,1,spacing*2);
	}

	protected override Point GetCellFromPosition(Vector3 pos)
	{
		int x = Mathf.FloorToInt((pos.x - minPosition.x + spacing)/spacing);
		int z = Mathf.FloorToInt((pos.z - minPosition.y + spacing)/spacing);

		return new Point(x,z);
	}

	public override void UpdateGrid (float[][] value)
	{
		for(int i = 0;i<value.Length;i++)
		{
			for(int j = 0;j<value[i].Length;j++)
			{
				Vector3 tempPos = particleGrid[i + j*value[i].Length].position;
				tempPos.y = scaleY*value[i][j];
				particleGrid[i + j*value[i].Length].position = tempPos;
				particleGrid[i + j*value[i].Length].startColor = gradient.Evaluate(tempPos.y/colorDiv);
			}
		}
		particles.SetParticles(particleGrid,particleGrid.Length);
	}
}
