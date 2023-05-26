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
    
    public override void Update() {	
	this.ControlPlayer();
	this.RefreshJumps();
	base.Update();
    }

    private void RefreshJumps() {
	if (this.OnSolidGround() && this.momentumY <= 0) {
	    // TODO: move to variable maybe
	    this.jumpsRemaining = 2;
	}
    }
    
    private void ControlPlayer() {
	// get inputs
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");
	bool down = Input.GetKey("s");
	bool jump = Input.GetKeyDown("space");

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

	// jump
	if (down) {
	    if (!this.OnSolidGround()) {
		if (this.momentumY > this.DiveSpeed * -1f) {
		    this.SetMomentumY(this.DiveSpeed * -1f);
		}
	    }
	}
	
	// sweet jumps
	if (jump) {
	    // regular jump
	    if (this.jumpsRemaining > 0) {
		this.SetMomentumY(this.JumpSpeed);
		this.jumpsRemaining--;		
	    }
	}
    }
}

