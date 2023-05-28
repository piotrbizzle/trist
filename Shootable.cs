using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : Moveable {
    private int previousGridX;
    private int previousGridY;
    
    public override void Start() {
	base.Start();
	this.level.RegisterTarget(this);
    }

    public override void Update() {
	this.previousGridX = this.gridX;
	this.previousGridY = this.gridY;
	base.Update();
	if (this.previousGridX != this.gridX || this.previousGridY != this.gridY) {
	    this.level.UpdateTarget(this);
	}
    }

    public void Break() {
	this.level.UnregisterTarget(this);
	GameObject.Destroy(this.gameObject);
    }
}
