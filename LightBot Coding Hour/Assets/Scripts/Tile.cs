using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
	public float xpos { get; set; }
	public float zpos { get; set; }
	public float height { get; set; }
	public string colour = "Grey";
	public MeshRenderer colorGrid;

	void Start()
	{
		xpos = transform.localPosition.x;
		zpos = transform.localPosition.z;
		height = transform.localPosition.y;
	}
}
