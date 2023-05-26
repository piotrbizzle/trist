using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Moveable : MonoBehaviour {
    // constants
    private static float GravityAcceleration = -60.0f;
    
    // configurable
    public float bounceMultiplier = 0.1f;

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
	this.DetermineGridPosition();       
	this.AddGravity();
	this.ApplyMomentum();
	this.DetermineGridPosition();
	this.CorrectPosition();
    }

    private void DetermineGridPosition() {
	this.gridHeight = (int)Math.Floor(this.GetComponent<SpriteRenderer>().sprite.rect.height / 100);
	this.gridWidth = (int)Math.Floor(this.GetComponent<SpriteRenderer>().sprite.rect.width / 100);
	this.gridX = (int)Math.Floor(this.transform.position.x - (float)this.gridWidth / 2);
	this.gridY = (int)Math.Floor(this.transform.position.y - (float)this.gridHeight / 2);
    }

    private void AddGravity() {
	// check if object is on the ground
	if (this.OnSolidGround()) {	    
	    return;
	}

	// if not, add gravity
	this.AddMomentum(0f, Moveable.GravityAcceleration * Time.deltaTime);
    }
    
    private void ApplyMomentum() {
	this.transform.Translate(new Vector2(this.momentumX, this.momentumY) * Time.deltaTime);
    }

    private void CorrectPosition() {
	bool corrected = false;

	// record current grid position
	int previousGridX = this.gridX;
	int previousGridY = this.gridY;
	
	// move to a different grid space if needed
	while (!this.IsPositionLegal()) {
	    corrected = true;
	    previousGridX = this.gridX;
	    previousGridY = this.gridY;
	    
	    // step movement back
	    Vector2 reverseVector = new Vector2(this.momentumX, this.momentumY) * Time.deltaTime * -1f;
	    if (reverseVector.magnitude > 1) {
		reverseVector.Normalize();
	    }
	    this.transform.Translate(reverseVector);
	    this.DetermineGridPosition();            
	}

	// reverse momentum in bounced direction
	if (corrected) {
	    // always try bouncing X before bouncing Y
	    if (previousGridX != this.gridX) {
		this.momentumX = -1f * this.momentumX * this.bounceMultiplier;
	    } else if (previousGridY != this.gridY) {
		this.momentumY = -1f * this.momentumY * this.bounceMultiplier;
	    }
	}
    }
    
    private bool IsPositionLegal() {
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
	return material == Level.Material.Concrete;
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
	// only counts if object is pressed against floor
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
