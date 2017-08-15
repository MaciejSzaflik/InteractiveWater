using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseEvent : EventArgs
{
	public Vector3 mousePosition;
	public MouseEvent(Vector3 mousePosition)
	{
		this.mousePosition = mousePosition;
	}
}

public class InputHandler : Singleton<InputHandler> {

	public event EventHandler OnButtonDown = delegate {};
	public event EventHandler OnButtonHold = delegate {};
	public event EventHandler OnButtonUp = delegate {};

	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			OnButtonDown(this,new MouseEvent(Input.mousePosition));
		}
		else if(Input.GetMouseButton(0))
		{
			OnButtonHold(this,new MouseEvent(Input.mousePosition));
		}
		else if(Input.GetMouseButtonUp(0))
		{
			OnButtonUp(this,new MouseEvent(Input.mousePosition));
		}
	}
}
