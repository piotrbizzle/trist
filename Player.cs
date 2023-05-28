using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Moveable {
    // constants
    public static int MaxGuns = 6;
    public float MoveSpeed = 15f;
    public float JumpSpeed = 20f;
    public float DiveSpeed = 30f;
    private static Quaternion[][] RotationsMap = new Quaternion[][]{
	new Quaternion[]{Quaternion.AngleAxis(180, Vector3.forward)},
	new Quaternion[]{Quaternion.AngleAxis(120, Vector3.forward), Quaternion.AngleAxis(240, Vector3.forward)},
	new Quaternion[]{Quaternion.AngleAxis(90, Vector3.forward), Quaternion.AngleAxis(180, Vector3.forward), Quaternion.AngleAxis(270, Vector3.forward)},
	new Quaternion[]{Quaternion.AngleAxis(72, Vector3.forward), Quaternion.AngleAxis(144, Vector3.forward), Quaternion.AngleAxis(216, Vector3.forward), Quaternion.AngleAxis(288, Vector3.forward)},
	new Quaternion[]{Quaternion.AngleAxis(60, Vector3.forward), Quaternion.AngleAxis(120, Vector3.forward), Quaternion.AngleAxis(180, Vector3.forward), Quaternion.AngleAxis(240, Vector3.forward), Quaternion.AngleAxis(300, Vector3.forward)},
	    };
    
    // inferred
    private int jumpsRemaining = 2;
    public bool isFacingLeft = false;
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
	if (this.OnSolidGround()) {
	    // TODO: move to variable maybe
	    this.jumpsRemaining = 2;
	}
    }

    private void CheckGuns() {
	int numberOfGuns = this.transform.childCount;
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
		gun.Select();
		this.activeGun = gun;
		gun.aimVector = (mousePointerPosition - this.transform.position).normalized;
		child.transform.rotation = baseRotation;
		child.transform.position = this.transform.position + new Vector3(0f, 0.5f, 0.0f);
		child.transform.Translate(Vector2.up);
	    } else {
		gun.Deselect();
		child.transform.rotation = baseRotation * Player.RotationsMap[numberOfGuns - 2][gunIndex - 1];
		child.transform.position = this.transform.position + new Vector3(0f, 0.5f, 0.0f);
		child.transform.Translate(Vector2.up);
	    }
		
	    gunIndex++;	    
	}
    }
    
    private void ControlPlayer() {
	// get inputs
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");
	bool down = Input.GetKey("s");
	bool jump = Input.GetKeyDown("space");
	bool previousGun = Input.GetKeyDown("q");
	bool nextGun = Input.GetKeyDown("e");
	bool discard = Input.GetKeyDown("r");
	bool mouse1Pressed = Input.GetMouseButton(0);

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
	if (mouse1Pressed) {
	    if (this.activeGun != null) {
		this.activeGun.Fire();
	    }
	}

	// discard gun
	if (discard) {
	    this.activeGun.Discard(this.isFacingLeft);
	}

	// switch guns
	if (previousGun) {
	    this.transform.GetChild(this.transform.childCount - 1).SetAsFirstSibling();
	}
	if (nextGun) {
	    this.transform.GetChild(0).SetAsLastSibling();
	}
    }
}

