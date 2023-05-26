using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Moveable : MonoBehaviour {
    // constants
    private static float GravityAcceleration = -10f;
    
    // configurable
    public float bounceMultiplier = 0.1f;

    // inferred fields
    private float momentumX;
    private float momentumY;

    private int gridX;
    private int gridY;
    private int gridHeight;
    private int gridWidth;

    // related objects
    private Level level;

    public void Start() {
	// work out level grid position
	this.level = this.transform.parent.GetComponent<Level>();
	this.DetermineGridPosition();
    }
    
    public void Update() {
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

    public bool DoesMaterialCollide(Level.Material material) {
	return material == Level.Material.Concrete;
    }
    
    public void AddMomentum(float x, float y) {
	this.momentumX += x;
	this.momentumY += y;
    }

    public bool IsOnSolidGround() {
	for (int i = 0; i < this.gridWidth + 1; i++) {
	    if (this.DoesMaterialCollide(this.level.GetBlock(this.gridX + i, this.gridY - 1))) {
		return true;
	    }
	}
	return false;
    }
    
}
