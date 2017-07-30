using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeightFieldSim))]
public class HeightFieldSimEditor : Editor {

	public HeightFieldSim sim
	{
		get{
			return target as HeightFieldSim;
		}
	}

	public override void OnInspectorGUI ()
	{
		if(GUILayout.Button("Reset forces"))
			sim.ResetForces();
		base.OnInspectorGUI();
	}
}
