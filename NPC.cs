using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPC : Shootable {
    private float walkSpeed = 2f;
    public bool isFacingLeft;    
    
    public override void Update() {
	this.NPCMove();
	base.Update();
    }

    public virtual void NPCMove() {
	this.Wander();
    }

    public bool IsApproachingEdge() {
	// check if you're going to walk off a ledge
	if (this.isFacingLeft) {
	    return this.level.GetBlock(this.gridX + 1 - this.gridWidth, this.gridY - 1) == Level.Material.Air;
	} else {
	    return this.level.GetBlock(this.gridX + this.gridWidth, this.gridY - 1) == Level.Material.Air;
	}
    }
    
    public void Wander() {
	if (!this.OnSolidGround()) {
	    return;
	}
	if (this.isFacingLeft) {
	    if (this.IsApproachingEdge() || this.IsSolidLeft()) {
		this.isFacingLeft = false;
	    } else {
		this.SetMomentumX(-1f * this.walkSpeed);
	    }
	} else {
	    if (this.IsApproachingEdge() || this.IsSolidRight()) {
		this.isFacingLeft = true;
	    } else {
		this.SetMomentumX(this.walkSpeed);
	    }
	}
    }
}
