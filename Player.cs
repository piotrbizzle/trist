using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Moveable {
    // constants
    public float MoveSpeed = 15f;
    public float JumpSpeed = 20f;
    public float DiveSpeed = 30f;
    
    // inferred
    private int jumpsRemaining = 2;
    private bool isFacingLeft = false;
    public Gun activeGun;

    public override void Start() {
	base.Start();
    }
    
    public override void Update() {
	this.ControlPlayer();
	this.RefreshJumps();	
        this.CheckGuns();
	base.Update();
    }

    private void RefreshJumps() {
	if (this.OnSolidGround() && this.momentumY <= 0) {
	    // TODO: move to variable maybe
	    this.jumpsRemaining = 2;
	}
    }

    private void CheckGuns() {
	// expect 1 camera + guns in transform
	int numberOfGuns = this.transform.childCount - 1;
	float angleIncrement = 360f / numberOfGuns;
	int gunIndex = 0;

	// get mouse pointer location in world
	Vector3 mousePointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	mousePointerPosition = new Vector3(mousePointerPosition.x, mousePointerPosition.y, 0.0f);

	// rotate and position gun
	Quaternion baseRotation = Quaternion.FromToRotation(Vector3.up, mousePointerPosition - this.transform.position);
	foreach (Transform child in this.transform) {
	    Gun gun = child.GetComponent<Gun>();
	    if (gun == null) {
		continue;
	    }
	    // set first one to active gun
	    if (gunIndex == 0) {
		this.activeGun = gun;
		gun.aimVector = (mousePointerPosition - this.transform.position).normalized;
		child.transform.rotation = baseRotation;
	    } else {
		child.transform.rotation = baseRotation * Quaternion.AngleAxis(angleIncrement * gunIndex, Vector3.forward);
	    }
	    child.transform.position = this.transform.position + new Vector3(0f, 0.5f, 0.0f);
	    child.transform.Translate(Vector2.up);
		
	    gunIndex++;	    
	}
    }
    
    private void ControlPlayer() {
	// get inputs
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");
	bool down = Input.GetKey("s");
	bool jump = Input.GetKeyDown("space");
	bool mouseClicked = Input.GetMouseButtonDown(0);

	// facing
	if (left && !right) {
	    this.isFacingLeft = true;
	}  else if (right && !left) {
	    this.isFacingLeft = false;
	}
	    
	// horizontal movement
	if (left && !right && !this.IsSolidLeft()) {
	    this.SetMomentumX(-1f * this.MoveSpeed);
	} else if (right && !left && !this.IsSolidRight()) {
	    this.SetMomentumX(this.MoveSpeed);
	} else if (this.OnSolidGround()) {
	    this.SetMomentumX(0f);	    
	}

	// dive
	if (down) {
	    if (!this.OnSolidGround()) {
		if (this.momentumY > this.DiveSpeed * -1f) {
		    this.SetMomentumY(this.DiveSpeed * -1f);
		}
	    }
	}
	
	// sweet jumps
	if (jump) {
	    if (this.jumpsRemaining > 0) {
		this.SetMomentumY(this.JumpSpeed);
		this.jumpsRemaining--;		
	    }
	}

	// shoot
	if (mouseClicked) {
	    this.activeGun.Fire();
	}
    }
}

