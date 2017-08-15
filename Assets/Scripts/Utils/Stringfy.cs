using UnityEngine;
using System;
using System.Text;

public class Stringfy
{
	//for latex..
	public static string RoundedArray(float[] array)
	{
		StringBuilder sb = new StringBuilder();
		Array.ForEach(array,(value) =>{
			sb.AppendFormat("{0:F1}",value);
			sb.Append(" &");
		});
		sb.Length -= 1;
		return sb.ToString();
	}
}

