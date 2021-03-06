﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Visualization : MonoBehaviour {

	protected int countX;
	protected int countY;
	protected Vector2 minPosition;
	protected Vector2 maxPosition;
	protected BoxCollider boxCollider;

	public virtual void Init(int countX, int countY, System.Action<Point> onInteraction)
	{
		this.countX = countX;
		this.countY = countY;
		AddOnMouseInteraction(onInteraction);
	}
	public virtual void UpdateGrid(float[][] value){}
	protected virtual void AddOnMouseInteraction (System.Action<Point> onInteraction)
	{
		InputHandler.Instance.OnButtonHold += (object sender, System.EventArgs e) => {
			if(onInteraction==null)
				return;

			Ray ray = Camera.main.ScreenPointToRay((e as MouseEvent).mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
			{
				Point point = GetCellFromPosition(hit.point);
				if(point.x < 0 || point.y < 0)
					return;
				if(point.x >= countX || point.y >= countY)
					return;

				onInteraction(point);
			}
		};
	}
	protected abstract Point GetCellFromPosition(Vector3 pos);

}
