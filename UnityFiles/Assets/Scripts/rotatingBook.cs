using UnityEngine;
using System.Collections;

public class rotatingBook : MonoBehaviour {
	
	Transform book;
	Vector3 pos;
	// Use this for initialization
	void Start () {
		book = transform.Find("book");
		pos = book.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		//float scaleY = Mathf.Sin(Time.time) * 0.5F + 1;
		book.Rotate(0,0,1);
		book.position = new Vector3(pos.x,pos.y + Mathf.Sin(Time.time)*.5f+.5f,pos.z);
	}
}
