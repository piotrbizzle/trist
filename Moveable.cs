using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Moveable : MonoBehaviour {
    // constants
    private static float GravityAcceleration = -60.0f;
    
    // configurable
    public float bounceMultiplier = 0.1f;
    public bool breakOnHit = false;
    public bool affectedByGravity = true;
    
    // inferred fields
    public float momentumX;
    public float momentumY;

    public int gridX;
    public int gridY;
    public int gridHeight;
    public int gridWidth;

    // related objects
    public Level level;

    public virtual void Start() {
	// work out level grid position
	this.level = this.transform.parent.GetComponent<Level>();
	this.DetermineGridPosition();
    }
    
    public virtual void Update() {
	// avoid stutter on startup
	if (Time.deltaTime > 0.1f) {
	    return;
	}
	this.DetermineGridPosition();       
	this.AddGravity();
	this.ApplyMomentum();
	this.DetermineGridPosition();
    }

    private void DetermineGridPosition() {
	this.gridHeight = (int)Math.Floor(this.GetComponent<SpriteRenderer>().sprite.rect.height / 100);
	this.gridWidth = (int)Math.Floor(this.GetComponent<SpriteRenderer>().sprite.rect.width / 100);
	this.gridX = (int)Math.Floor(this.transform.position.x - (float)this.gridWidth / 2);
	this.gridY = (int)Math.Floor(this.transform.position.y - (float)this.gridHeight / 2);
    }

    private void AddGravity() {
	// check if object is on the ground
	if (!this.affectedByGravity || this.OnSolidGround()) {
	    return;
	}

	// if not, add gravity
	this.AddMomentum(0f, Moveable.GravityAcceleration * Time.deltaTime);
    }
    
    private void ApplyMomentum() {
	// close distance to end of vector 1 blocklength at a time
	Vector3 unitMove = new Vector3(this.momentumX, this.momentumY, 0).normalized * 0.1f;
	Vector3 remainingMove = new Vector3(this.momentumX, this.momentumY, 0) * Time.deltaTime;

	// record current grid position
	int previousGridX = this.gridX;
	int previousGridY = this.gridY;
	Vector3 previousPosition = this.transform.position;

	while (remainingMove.magnitude > 0) {
	    if (remainingMove.magnitude <= unitMove.magnitude) {
		this.transform.position += remainingMove;
		remainingMove = new Vector3(0, 0, 0);
	    } else {
		this.transform.position += unitMove;
		remainingMove -= unitMove;
	    };

	    // check for collisions
	    this.CheckPartialMove(previousPosition, previousGridX, previousGridY);

	    // no collision
	    previousPosition = this.transform.position;
	    previousGridX = this.gridX;
	    previousGridY = this.gridY;	
	}
    }

    // returns true if partial move is safe else false
    public virtual bool CheckPartialMove(Vector3 previousPosition, int previousGridX, int previousGridY) {
	// collision
	if (!this.IsPositionLegal()) {
	    // breakables
	    if (this.breakOnHit) {
		this.Break();
		return false;
	    }
		
	    this.transform.position = previousPosition;
		
	    if (previousGridX != this.gridX) {
		this.momentumX = -1f * this.momentumX * this.bounceMultiplier;	
	    }
	    if (previousGridY != this.gridY) {
		this.momentumY = -1f * this.momentumY * this.bounceMultiplier;
	    }
	    return false;
	} else {
	    return true;
	}
    }

    public virtual void Break() {
	GameObject.Destroy(this.gameObject);
    }
    
    private bool IsPositionLegal() {
	this.DetermineGridPosition();
	for (int i = 0; i < this.gridWidth + 1; i++) {
	    for (int j = 0; j < this.gridHeight + 1; j++) {
		if (this.DoesMaterialCollide(this.level.GetBlock(this.gridX + i, this.gridY + j))) {
		    return false;
		}
	    }
	}
	return true;
    }

    public virtual bool DoesMaterialCollide(Level.Material material) {
	return material != Level.Material.Air;
    }    
    
    public void AddMomentum(float x, float y) {
	this.momentumX += x;
	this.momentumY += y;
    }

    public void SetMomentumX(float x) {
	this.momentumX = x;
    }

    public void SetMomentumY(float y) {
	this.momentumY = y;
    }

    // TODO: maybe rewrite these as one method
    public bool OnSolidGround() {
	float bottomY = this.transform.position.y - (float)this.gridHeight / 2;
	if (bottomY - Math.Floor(bottomY) >= 0.1f) {
	    return false;
	}
	
	for (int i = 0; i < this.gridWidth + 1; i++) {
	    if (this.DoesMaterialCollide(this.level.GetBlock(this.gridX + i, this.gridY - 1))) {
		return true;
	    }
	}
	return false;
    }
    
    public bool IsSolidUp() {
	float topY = this.transform.position.y + (float)this.gridHeight / 2;
	if (Math.Ceiling(topY) - topY >= 0.1f) {
	    return false;
	}
	
	for (int i = 0; i < this.gridWidth + 1; i++) {
	    if (this.DoesMaterialCollide(this.level.GetBlock(this.gridX + i, this.gridY + 1))) {
		return true;
	    }
	}
	return false;
    }

    public bool IsSolidLeft() {
	float leftX = this.transform.position.x - (float)this.gridWidth / 2;
	if (leftX - Math.Floor(leftX) >= 0.1f) {
	    return false;
	}
	
	for (int i = 0; i < this.gridHeight + 1; i++) {
	    if (this.DoesMaterialCollide(this.level.GetBlock(this.gridX - 1, this.gridY + i))) {
		return true;
	    }
	}
	return false;
    }

    public bool IsSolidRight() {
	float rightX = this.transform.position.x + (float)this.gridWidth / 2;
	if (Math.Ceiling(rightX) - rightX >= 0.1f) {
	    return false;
	}
	
	for (int i = 0; i < this.gridHeight + 1; i++) {
	    if (this.DoesMaterialCollide(this.level.GetBlock(this.gridX + this.gridWidth + 1, this.gridY + i))) {	
		return true;
	    }
	}
	return false;
    }
    
}
