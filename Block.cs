using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour {
    // configurables
    public Level.Material material;

    // inferred fields
    private bool inited = false; 
    public int gridX;
    public int gridY;
    public int gridHeight;
    public int gridWidth;

    // related objects
    private Level level;

    
    public void Start() {
	// work out level grid position
	this.level = this.transform.parent.GetComponent<Level>();
	this.DetermineGridPosition();
	
	// lock block into place
	this.transform.position = new Vector2(this.gridX + (float)this.gridWidth / 2, this.gridY + (float)this.gridHeight / 2);
    }
    
    public virtual void Update() {
	// add to grid on first update to guarantee parent Level is ready
	if (!this.inited) {
	    for (int i = 0; i < this.gridWidth; i++) {
		for (int j = 0; j < this.gridHeight; j++) {		    
		    this.level.SetBlock(this.gridX + i, this.gridY + j, this.material);
		}
	    }
	    this.inited = true;
	}
    }

    public void Transform(Level.Material material) {
	for (int i = 0; i < this.gridWidth; i++) {
	    for (int j = 0; j < this.gridHeight; j++) {
		this.level.SetBlock(this.gridX + i, this.gridY + j, material);
	    }
	}
    }

    private void DetermineGridPosition() {
	this.gridHeight = (int)Math.Floor(this.GetComponent<SpriteRenderer>().sprite.rect.height / 100);
	this.gridWidth = (int)Math.Floor(this.GetComponent<SpriteRenderer>().sprite.rect.width / 100);
	this.gridX = (int)Math.Floor(this.transform.position.x - (float)this.gridWidth / 2);
	this.gridY = (int)Math.Floor(this.transform.position.y - (float)this.gridHeight / 2);
    }
}
