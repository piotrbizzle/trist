using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour {    
    private Vector3 mousePointerPosition;
    
    public void Update() {
	// get mouse pointer location in world
	this.mousePointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	this.mousePointerPosition = new Vector3(this.mousePointerPosition.x, this.mousePointerPosition.y, 0.0f);

	// rotate player pointer
	this.transform.rotation = Quaternion.FromToRotation(Vector3.up,  this.mousePointerPosition - this.transform.parent.position);
    }
}
