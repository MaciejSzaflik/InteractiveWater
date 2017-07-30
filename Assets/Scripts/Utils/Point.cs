using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point {

	public int x, y;

	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override string ToString ()
	{
		return string.Format("{0}.{1}",x,y);
	}
}
