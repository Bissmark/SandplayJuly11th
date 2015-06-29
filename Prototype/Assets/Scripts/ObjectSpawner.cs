using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Holoville.HOTween;

public enum SPAWN_STATE
{
	NOT_SPAWNING,
	SCROLLING,
	PREVIEW,
	PLACING
};
 
public class ObjectSpawner : MonoBehaviour 
{
	public Camera 			UICamera;
	public Camera 			ViewCamera; /* The main camera the player sees the scene through */
	public GameObject 		UIGridPanel; /*The scoll panel that has all the object portraits*/
	public GameObject 		PanelItemPreview; /*Prefab for a generic item in the GridPanel. Used to create items in the BuildGridPanel function.*/
	public List<GameObject> PanelItems; /*List of the prefabs to be spawned by the spawning system*/
	public Sprite[] librarySprites;
	public GameObject sandboxWhole;
	public SaveScene test;

	/*public interface for letting other scrips access the current state*/
	public SPAWN_STATE CurrentState
	{
		get
		{
			return eCurrState;
		}
	}

	public int TouchID
	{
		get
		{
			return iTouchID;
		}
	}

	private SPAWN_STATE eCurrState = SPAWN_STATE.NOT_SPAWNING; /*The internal enum for the current spawning state*/

	private int 		iTouchID = -1; /* ID of the touch currently being used. Set when going from NOT_SPAWNING to SCROLLING*/
	private string 		sItemName = ""; /* Name of the picture they held down on when they started scrolling */
	private GameObject 	oCurrObject = null; /*The current mesh for the object being spawned*/
	private Tweener 	oTween = null; /* The HOTween used for moving the current object around */
	private float		fScrollHoldTimer = 0; /* timer for counting up how long the finger has been held in the SCROLLING state */
	private Transform 	lastTransform;
	private BoxCollider bc;
	private Vector3     vStoredFacing;

	//Generate panel items in the UIGrid for each item in the PanelItem list
	//Called once during start
	void BuildGridPanel()
	{
		librarySprites = Resources.LoadAll<Sprite> ("ScrollbarPreviewImages");

		for (int i = 0; i < PanelItems.Count; ++i) 
		{
			//create the new object, attach it to the grid and set its position
			GameObject go = (GameObject)Instantiate(PanelItemPreview, Vector3.zero, Quaternion.identity);
			go.transform.SetParent(UIGridPanel.transform);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = new Vector3(i * UIGridPanel.GetComponent<HorizontalLayoutGroup>().spacing, 0, 0);
			go.name = PanelItems[i].name;

            Image spriteCheck = go.GetComponentInChildren<Image>();
			if (spriteCheck == null)
            {
            	Debug.LogError("No sprite in children of PanelItemPreview gameobject!");
            	return;
            }

			//Set it to the right sprite
			spriteCheck.sprite = librarySprites[i];
			spriteCheck.name = PanelItems[i].name;
		}
	}

	void OnLevelWasLoaded(int level) 
	{
		if (level == 2)
			sandboxWhole.SetActive (true);
		else
			Debug.Log (level);
	}


	void Awake()
	{
		//OnLevelWasLoaded ();
		BuildGridPanel ();
	}


	void Update()
	{
		//variables used in multiple case statements have to be out of the switch
		SimpleTouch t = new SimpleTouch ();
		Ray r;
		RaycastHit info;

		switch (eCurrState) 
		{
		case SPAWN_STATE.NOT_SPAWNING:
			UIGridPanel.transform.parent.GetComponent<ScrollRect>().enabled = true;
			//look for a touch on the scroll bar
			for ( int i = 0 ; i < TouchHandler.touchCount ; ++i )
			{
				t = TouchHandler.GetTouch(i);
				r = UICamera.ScreenPointToRay(t.position);//ViewCamera.ScreenPointToRay(t.position);//

				PointerEventData pe = new PointerEventData(EventSystem.current);
				pe.position =  Input.mousePosition;

				//if we've just touched the scroll bar
				List<RaycastResult> hits = new List<RaycastResult>();
				EventSystem.current.RaycastAll( pe, hits );
				
				foreach(RaycastResult h in hits)
				{
					if ( h.gameObject.tag == "GridPanelItem" )
					{
						Debug.Log("Transition NOT_SPAWNING->SCROLLING");
						iTouchID = t.fingerId;
						sItemName = h.gameObject.name;
						fScrollHoldTimer = 0;
						eCurrState = SPAWN_STATE.SCROLLING;
					}
				}
			}
			break;


		case SPAWN_STATE.SCROLLING:
			t = GetTouchByID(iTouchID);

			//if we couldn't find the touch or the user lifted their finger, go back to NOT_SPAWNING
			if ( t.fingerId == -1 || t.phase == TouchPhase.Ended )
			{
				Debug.Log("Transition SCROLLING->NOT_SPAWNING");
				eCurrState = SPAWN_STATE.NOT_SPAWNING;
				iTouchID = -1;
				sItemName = "";
				break;
			}

			if ( t.phase != TouchPhase.Stationary )
				fScrollHoldTimer = 0;
			else
				fScrollHoldTimer += Time.deltaTime;

			//if they've held down their finger for a bit, switch to preview mode
			if (fScrollHoldTimer > 0.5f || IsTouchOnSandbox(t, out info) )
			{
				Debug.Log("Transition SCROLLING->PREVIEW");
				UIGridPanel.transform.parent.GetComponent<ScrollRect>().enabled = false;
				oCurrObject = (GameObject)Instantiate(FindPanelPrefabByName(sItemName));
				oCurrObject.name = sItemName;
                oCurrObject.GetComponent<Rigidbody>().isKinematic = true;
				vStoredFacing = Vector3.forward;

				Vector3 startPos = FindGridPanelEntryByName(sItemName).transform.position;
				r = ViewCamera.ScreenPointToRay( UICamera.WorldToScreenPoint(startPos) );//ViewCamera.ScreenPointToRay(startPos);
				startPos =  r.GetPoint(10f);
				oCurrObject.transform.position = startPos;
				oCurrObject.transform.forward = ViewCamera.transform.forward;
				oTween = HOTween.To(oCurrObject.transform, 0.3f, new TweenParms().Ease(EaseType.EaseOutCubic).Prop("position", ViewCamera.transform.position + ViewCamera.transform.forward * 10f));
				eCurrState = SPAWN_STATE.PREVIEW;
			}
			
			break;
		case SPAWN_STATE.PREVIEW:
			t = GetTouchByID(iTouchID);

			//if we couldn't find the touch or the user lifted their finger, go back to NOT_SPAWNING
			if ( t.fingerId == -1 || t.phase == TouchPhase.Ended )
			{
				Debug.Log("Transition PREVIEW->NOT_SPAWNING");
				eCurrState = SPAWN_STATE.NOT_SPAWNING;
				if (oTween != null)
					oTween.Kill();
				Vector3 endPos = FindGridPanelEntryByName(sItemName).transform.position;
				r = ViewCamera.ScreenPointToRay( UICamera.WorldToScreenPoint(endPos) );//ViewCamera.ScreenPointToRay(endPos);
				endPos =  r.GetPoint(5f);
				oTween = HOTween.To(oCurrObject.transform, 0.3f, new TweenParms().Ease(EaseType.EaseOutCubic).Prop("position", endPos).Prop("localScale", Vector3.zero)
									//when the tween is finished, destroy the object
									.OnComplete(() => { GameObject.Destroy(oCurrObject); oCurrObject = null; }));
				iTouchID = -1;
				sItemName = "";
				break;
			}

			oCurrObject.transform.Rotate(Vector3.up, Time.deltaTime * 60f);

			//if the mouse has moved over the sandbox, change to the PLACING state
			if ( IsTouchOnSandbox( t, out info ) )
			{
				Debug.Log("Transition PREVIEW->PLACING");
				eCurrState = SPAWN_STATE.PLACING;
				if (oTween != null)
					oTween.Kill();
				oTween = HOTween.To(oCurrObject.transform, 0.3f, new TweenParms().Ease(EaseType.EaseOutCubic).Prop("forward", vStoredFacing));
				   
				break;
			}
			break;
		case SPAWN_STATE.PLACING:
			t = GetTouchByID(iTouchID);
			
			//if we couldn't find the touch or the user lifted their finger, place the object and go back to NOT_SPAWNING
			if ( t.fingerId == -1 || t.phase == TouchPhase.Ended )
			{
				Debug.Log("Transition PLACING->NOT_SPAWNING");
				eCurrState = SPAWN_STATE.NOT_SPAWNING;
				iTouchID = -1;
				sItemName = "";
				if (oTween != null)
					oTween.Kill();
                oCurrObject.GetComponent<Rigidbody>().isKinematic = false;
				oCurrObject.transform.parent = transform;
				oCurrObject = null;
				break;
			}

			//if the touch isn't over the sandbox anymore, go back to PREVIEW
			if ( ! IsTouchOnSandbox( t, out info ) )
			{
				Debug.Log("Transition PLACING->PREVIEW");
				eCurrState = SPAWN_STATE.PREVIEW;
				if (oTween != null)
					oTween.Kill();
				oTween = HOTween.To(oCurrObject.transform, 0.3f, new TweenParms().Ease(EaseType.EaseOutCubic).Prop("position", ViewCamera.transform.position + ViewCamera.transform.forward * 10f)
									.Prop("forward", ViewCamera.transform.forward));
			}
			else
			{
                Vector3 targetPos = info.point + Vector3.up * 0.25f;

                Vector3[] points = new Vector3[5];
                points[0] = oCurrObject.GetComponent<Collider>().bounds.center;

                points[1] = points[2] = points[3] = points[4] = oCurrObject.GetComponent<Collider>().bounds.center - oCurrObject.GetComponent<Collider>().bounds.size;
                points[2].x += oCurrObject.GetComponent<Collider>().bounds.size.x;
                points[3].y += oCurrObject.GetComponent<Collider>().bounds.size.y;
                points[4].x += oCurrObject.GetComponent<Collider>().bounds.size.x;
                points[4].y += oCurrObject.GetComponent<Collider>().bounds.size.y;

                oCurrObject.GetComponent<Collider>().enabled = false;
                
                foreach (var v in points)
                {
                    if (Physics.Raycast(new Ray(v + Vector3.up * 15, Vector3.down), out info)
                        && info.collider.tag == "Object"
                        && targetPos.y < (info.point + Vector3.up * 0.25f).y)
                    {
                        targetPos.y = (info.point + Vector3.up * 0.25f).y;
                        oCurrObject.transform.position = targetPos;
                    }
                }
                oCurrObject.GetComponent<Collider>().enabled = true;
                oCurrObject.transform.position = Vector3.Lerp(oCurrObject.transform.position, targetPos, Time.deltaTime * 5f);

                //if (TouchHandler.touchCount == 2)
                    //oCurrObject.transform.Rotate(Vector3.up, Time.deltaTime * 100f);
				if(Input.GetKey("x") || Input.GetMouseButton(1))
				{
					oCurrObject.transform.Rotate(Vector3.up, Time.deltaTime * 100f);
				}
				if (Input.GetKeyDown ("up"))
				{
					BuryObject(oCurrObject);
				}
			}
			break;
		}
	}

	//small function that searches the list of prefabs for one with
	//the matching name. Is used to map the preview sprites to the prefabs in the state machine
	public GameObject FindPanelPrefabByName(string a_Name)
	{
		for (int i = 0; i < PanelItems.Count; ++i) 
		{
			if ( PanelItems[i].name == a_Name )
				return PanelItems[i];
		}

		return null;
	}

	GameObject FindGridPanelEntryByName(string a_Name)
	{
		foreach (Transform t in UIGridPanel.transform )
		{
			if ( t.name == a_Name )
				return t.gameObject;
		}
		return null;
	}

	//returns the touch with the given fingerID if it exists
	public SimpleTouch GetTouchByID(int a_TouchID)
	{
		SimpleTouch t = new SimpleTouch();
		t.fingerId = -1;
		//find the right touch
		for ( int i = 0 ; i < TouchHandler.touchCount ; ++i )
		{
			if (TouchHandler.GetTouch(i).fingerId == a_TouchID )
			{
				t = TouchHandler.GetTouch(i);
				break;
			}
		}

		return t;
	}

	public void BuryObject(GameObject modelBuried)
	{
		oCurrObject = modelBuried;
        oCurrObject.GetComponent<Collider>().transform.Translate(1, 1, 1);
	}

	//returns weather or not the given touch is over the sandbox
	//just a convenience function that makes sure the finger isn't over GUI stuff first
	bool IsTouchOnSandbox( SimpleTouch a_Touch, out RaycastHit a_Info )
	{
		Ray r = UICamera.ScreenPointToRay (a_Touch.position);//
		//if (!EventSystem.current.IsPointerOverGameObject ()) 
		//{
			if (Physics.Raycast (r)) 
			{
				a_Info = new RaycastHit ();
				return false;
			}
			r = ViewCamera.ScreenPointToRay (a_Touch.position);
		//}
		return  Physics.Raycast (r, out a_Info, 100f, 1 << LayerMask.NameToLayer ("SandBox"));
	}

	public void PickUpPiece( GameObject a_Piece, SimpleTouch a_Touch )
	{
		if ( eCurrState != SPAWN_STATE.NOT_SPAWNING )
		{
			Debug.LogWarning("Warning: Attempt to pick up piece while spawning");
			return;
		}

		iTouchID = a_Touch.fingerId;
		sItemName = a_Piece.name;
		fScrollHoldTimer = 0;
		oCurrObject = a_Piece;
		vStoredFacing = a_Piece.transform.forward;
		eCurrState = SPAWN_STATE.PREVIEW;
	}

	public void MainMenu () 
	{	
		Application.LoadLevel(0);
	}
	
	void Library () 
	{
		Application.LoadLevel(2);
	}
}
