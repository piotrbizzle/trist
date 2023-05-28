using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : Moveable {
    private int previousGridX;
    private int previousGridY;
    private bool markedForBreak;
    private bool registered;

    public override void Update() {
	if (!this.registered) {
	    this.level.RegisterTarget(this);
	    this.registered = true;
	}
	if (this.markedForBreak) {
	    this.level.UnregisterTarget(this);
	    GameObject.Destroy(this.gameObject);
	    return;
	}
	
	this.previousGridX = this.gridX;
	this.previousGridY = this.gridY;
	base.Update();
	if (this.previousGridX != this.gridX || this.previousGridY != this.gridY) {
	    this.level.UpdateTarget(this);
	}
    }

    public void Break() {
	this.markedForBreak = true;
    }
}
