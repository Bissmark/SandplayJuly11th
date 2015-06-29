using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeSandbox : MonoBehaviour 
{
	public GameObject sandboxWhole;
	public Button sandBox;

	public GameObject originalSand;
	public Mesh Sand;
	public MeshCollider sandCollider;
	public GameObject water;

	MeshFilter mf;
	MeshCollider mc;

	// Use this for initialization
	void Start () 
	{
		mf = originalSand.GetComponent (typeof(MeshFilter)) as MeshFilter;
		mc = originalSand.GetComponent (typeof(MeshCollider)) as MeshCollider;
	}
	
	// Update is called once per frame
	public void OnClick () 
	{
		if (sandBox.interactable == true) 
		{
			mf.mesh = Sand;
			mc.sharedMesh = sandCollider.sharedMesh;
			sandboxWhole.SetActive (false);
		}
	}
}