using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : Moveable {
    // configurable
    public bool pullable;
    public int health;
    
    private int previousGridX;
    private int previousGridY;
    public bool markedForBreak;
    public bool isFriendly;
    private bool registered;

    public override void Update() {
	if (!this.registered) {
	    this.level.RegisterTarget(this);
	    this.registered = true;
	}
	if (this.markedForBreak) {
	    this.GetDestroyed();
	    return;
	}
	
	this.previousGridX = this.gridX;
	this.previousGridY = this.gridY;
	base.Update();
	if (this.previousGridX != this.gridX || this.previousGridY != this.gridY) {
	    this.level.UpdateTarget(this);
	}
    }

    public virtual void Hit(int damage) {
	this.health -= damage;
	if (this.health <= 0) {
	    this.Break();
	}
    }
    
    public override void Break() {
	this.markedForBreak = true;
    }

    public virtual void GetDestroyed() {
	this.level.UnregisterTarget(this);
	GameObject.Destroy(this.gameObject);
    }    
    
}
