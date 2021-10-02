using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour {

	// Use this for initialization
	public int index;
	[SerializeField] bool keyDown;
	[SerializeField] int maxIndex;
	public AudioSource audioSource;

	[SerializeField] RectTransform menuButton1;
	public Bounds bounds1;
	[SerializeField] RectTransform menuButton2;
	public Bounds bounds2;
	[SerializeField] RectTransform menuButton3;
	public Bounds bounds3;
	[SerializeField] RectTransform menuButton4;
	public Bounds bounds4;
	[SerializeField] Camera cam;
	public Vector2 point;
	void Start () {
		if (!audioSource)
		{
			audioSource = GetComponent<AudioSource>();
		}
		Vector3[] corners = new Vector3[4];

		float scaleFactor;
		if (menuButton1)
		{
			bounds1 = new Bounds();
			menuButton1.GetWorldCorners(corners);
			bounds1.SetMinMax(cam.WorldToViewportPoint(corners[1]), cam.WorldToViewportPoint(corners[3]));
			scaleFactor = .5f / bounds1.center.x;
			bounds1.center *= scaleFactor;
			bounds1.extents *= scaleFactor;
			bounds1.center = new Vector3(bounds1.center.x, bounds1.center.y, 0);
			bounds1.extents = new Vector3(Mathf.Abs(bounds1.extents.x), Mathf.Abs(bounds1.extents.y), 0.1f);
		}
		bounds2 = new Bounds();
		if (menuButton2)
		{
			menuButton2.GetWorldCorners(corners);
			bounds2.SetMinMax(cam.WorldToViewportPoint(corners[1]), cam.WorldToViewportPoint(corners[3]));
			scaleFactor = .5f / bounds2.center.x;
			bounds2.center *= scaleFactor;
			bounds2.extents *= scaleFactor;
			bounds2.center = new Vector3(bounds2.center.x, bounds2.center.y, 0);
			bounds2.extents = new Vector3(Mathf.Abs(bounds2.extents.x), Mathf.Abs(bounds2.extents.y), 0.1f);
		}
		bounds3 = new Bounds();
		if (menuButton3)
		{
			menuButton3.GetWorldCorners(corners);
			bounds3.SetMinMax(cam.WorldToViewportPoint(corners[1]), cam.WorldToViewportPoint(corners[3]));
			scaleFactor = .5f / bounds3.center.x;
			bounds3.center *= scaleFactor;
			bounds3.extents *= scaleFactor;
			bounds3.center = new Vector3(bounds3.center.x, bounds3.center.y, 0);
			bounds3.extents = new Vector3(Mathf.Abs(bounds3.extents.x), Mathf.Abs(bounds3.extents.y), 0.1f);
		}
		bounds4 = new Bounds();
		if (menuButton4)
		{
			menuButton4.GetWorldCorners(corners);
			bounds4.SetMinMax(cam.WorldToViewportPoint(corners[1]), cam.WorldToViewportPoint(corners[3]));
			scaleFactor = .5f / bounds4.center.x;
			bounds4.center *= scaleFactor;
			bounds4.extents *= scaleFactor;
			bounds4.center = new Vector3(bounds4.center.x, bounds4.center.y, 0);
			bounds4.extents = new Vector3(Mathf.Abs(bounds4.extents.x), Mathf.Abs(bounds4.extents.y), 0.1f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetAxis ("Vertical") != 0) || (Input.GetAxis ("D-Pad Y") != 0)){
			if(!keyDown){
				if ((Input.GetAxis ("Vertical") < 0) || (Input.GetAxis("D-Pad Y") < 0)) {
					if(index < maxIndex){
						index++;
					}else{
						index = 0;
					}
				} else if ((Input.GetAxis("Vertical") > 0) || (Input.GetAxis("D-Pad Y") > 0)){
					if (index > 0){
						index --; 
					}else{
						index = maxIndex;
					}
				}
				keyDown = true;
			}
		}else{
			keyDown = false;
		}

		point = cam.ScreenToViewportPoint(Input.mousePosition);
		if (bounds1.Contains(point))
		{
			index = 0;
		}
		else if (bounds2.Contains(point))
		{
			if (maxIndex > 0)
			{
				index = 1;
			}
		}
		else if (bounds3.Contains(point))
		{
			if (maxIndex > 1)
			{
				index = 2;
			}
		}
		else if (bounds4.Contains(point)) { 
			if(maxIndex > 2)
            {
				index = 3;
            }
		}
	}

}
