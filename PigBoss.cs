using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBoss : Block {
    public List<PigBossHead> heads;
    public float maxHeadCooldown;
    private float headCooldown;
    private bool markedForBreak;
    
    public override void Update() {
	if (this.markedForBreak) {
	    GameObject.Destroy(this.gameObject);
	    return;
	}
	if (this.headCooldown >= 0) {
	    this.headCooldown -= Time.deltaTime;
	} else {
	    this.heads[0].Attack();
	}
	
	base.Update();
    }

    public void NextHead() {
	this.headCooldown = this.maxHeadCooldown;
	if (this.heads.Count > 1) {
	    // rotate to next head
	    this.heads.Add(this.heads[0]);
	    this.heads.RemoveAt(0);
	}
    }

    public void Break() {
	// set blocks to Air
	this.gameObject.GetComponent<Block>().Transform(Level.Material.Air);
	this.markedForBreak = true;
    }
}
