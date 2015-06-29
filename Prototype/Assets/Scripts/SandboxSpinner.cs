using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectSpawner))]
public class SandboxSpinner : MonoBehaviour {

	public Camera ViewCamera; /* The main camera the player sees the scene through */
	
	private int 			iTouchID = -1; /* The ID of the current touch. -1 denotes no current active touch */ 
	private ObjectSpawner 	oSpawner = null; /* The spwaner object attached to the sandbox */
	private float 			fRotationalVelocity = 0;
	public GameObject 		boxRotater;
	Ray r;
	int speed = 60;

	public GameObject saveBar;
	public GameObject questionBar;
	public OpenLibrary libraryAnimation;

	void Start()
	{
		oSpawner = GetComponent<ObjectSpawner> ();
	}

	// Update is called once per frame
	void Update () 
	{
        float multiplyer = 1f;
		Ray r;
		RaycastHit info;
		//Vector3 move; 

		//If we don't have a valid touch yet, check if there is one
		if ( iTouchID == -1)
		{
			SimpleTouch t = new SimpleTouch();

			for ( int i = 0 ; i < TouchHandler.touchCount ; ++ i )
			{
				t = TouchHandler.GetTouch(i);
				r = ViewCamera.ScreenPointToRay(t.position);
				//Either the spawner isn't spawning, or if it is, make sure we're not using the same touch as it
				if ( oSpawner.CurrentState == SPAWN_STATE.NOT_SPAWNING && t.phase == TouchPhase.Began)
				{
					if(saveBar.activeSelf == false)
					{
						if(libraryAnimation.isLibraryOpen == false)
						{
							if ( Physics.Raycast(r, out info ) && info.collider.tag == "Rotater" )
							{
								iTouchID = t.fingerId;
								//move = info.normal;
								break;
							}
						}
					}
				}
			}
		}
		else
		{
            multiplyer = 5f;
			//defer to spawner if we've accidently grabbed the same touch
            if (oSpawner.CurrentState != SPAWN_STATE.NOT_SPAWNING )
			{
				iTouchID = -1;
				return;
			}
			//if the touch is over, return
			if ( TouchHandler.GetTouch(iTouchID).phase == TouchPhase.Ended )
			{
				iTouchID = -1;
				return;
			}
			fRotationalVelocity = TouchHandler.GetTouch(iTouchID).deltaPosition.x / Time.deltaTime * 0.2f;
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Rotate(Vector3.up, speed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			transform.Rotate(Vector3.down, speed * Time.deltaTime);
		}
		//transform.Rotate(Vector3.up, speed * Time.deltaTime); //(move add back in)
		transform.Rotate(Vector3.up, fRotationalVelocity * Time.deltaTime); //This is for web player
        fRotationalVelocity -= fRotationalVelocity * Time.deltaTime * 3f * multiplyer;
        //Debug.Log(fRotationalVelocity);
	}
}