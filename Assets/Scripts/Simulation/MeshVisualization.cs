using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshVisualization : Visualization {

	public Material material;
	public float spacing = 0.25f;
	private Vector3[] vertices;
	private Mesh mesh;
	public float scaleY = 1;

	public override void Init (int countX, int countY, System.Action<Point> onInteraction)
	{
		this.countX = countX;
		this.countY = countY;
		AddOnMouseInteraction(onInteraction);

		mesh = new Mesh();
		CreateStartVertices();
		mesh.vertices = vertices;
		mesh.triangles = CreateTrisArray();

		mesh.MarkDynamic();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = material;

		CreateCollider();
	}

	protected void CreateCollider()
	{
		minPosition = mesh.bounds.min;
		maxPosition = mesh.bounds.max;

		boxCollider = this.gameObject.AddComponent<BoxCollider>();

		boxCollider.center = mesh.bounds.center;
		boxCollider.size = new Vector3(mesh.bounds.size.x,2,mesh.bounds.size.z);
	}

	protected override Point GetCellFromPosition(Vector3 pos)
	{
		int x = Mathf.FloorToInt((pos.x - minPosition.x + spacing)/spacing);
		int z = Mathf.FloorToInt((pos.z - minPosition.y + spacing)/spacing);

		return new Point(x,z);
	}


	private void CreateStartVertices()
	{
		vertices = new Vector3[countX*countY];

		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				vertices[i + j*countY] = new Vector3(i * spacing, 0, j * spacing);
			}
		}
	}

	private int[] CreateTrisArray()
	{
		List<int> tris = new List<int>();

		for(int x = 0;x<countX - 1;x++)
		{
			for(int y = 0;y<countY - 1;y++)
			{
				tris.Add(x + y*countX + countX);
				tris.Add(x + y*countX + 1);
				tris.Add(x + y*countX);

				tris.Add(x + y*countX + 1);
				tris.Add(x + y*countX + countX);
				tris.Add(x + y*countX + countX + 1);

			}
		}

		return tris.ToArray();
	}

	public override void UpdateGrid (float[][] value)
	{
		for(int i = 0;i<countX;i++)
		{
			for(int j = 0;j<countY;j++)
			{
				vertices[i + j*countY] = new Vector3(i * spacing, scaleY*value[i][j], j * spacing);
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
	}
}
