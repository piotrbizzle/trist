using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyGunHaver : NPC {
    // configurables
    public Sprite heldGunSprite;
    public Gun gun;
    public float alertDistance;
    private float runSpeed = 4f;

    // related objects
    private Player player;
    
    public override void Start() {
	base.Start();
	this.player = GameObject.Find("level/player").GetComponent<Player>();
    }
    
    public override void NPCMove() {
	if (this.PlayerManhattanDistance() < this.alertDistance) {
	    this.Fight();
	} else {
	    this.Wander();
	}
    }
    
    public int PlayerManhattanDistance() {
	return Math.Abs(this.player.gridX - this.gridX) + Math.Abs(this.player.gridY - this.gridY);
    }

    public void Fight() {
	// move toward player
	if (this.player.transform.position.x < this.transform.position.x) {
	    this.isFacingLeft = true;
	    if (!this.IsApproachingEdge() && !this.IsSolidLeft()) {
		this.SetMomentumX(-1f * this.runSpeed);
	    } else {
		this.SetMomentumX(0);
	    }
	} else {
	    this.isFacingLeft = false;
	    if (!this.IsApproachingEdge() && !this.IsSolidRight()) {
		this.SetMomentumX(this.runSpeed);
	    } else {
		this.SetMomentumX(0);
	    }
	}
    }
}
