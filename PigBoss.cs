using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBoss : Block {
    public List<PigBossHead> heads;  // expect them to also be guns!
    private bool markedForBreak;
    
    public override void Update() {
	if (this.markedForBreak) {
	    GameObject.Destroy(this.gameObject);
	    return;
	}

	foreach (PigBossHead head in this.heads) {
	    head.HeadMove();
	}
	base.Update();
    }

    public void Break() {
	// set blocks to Air
	this.gameObject.GetComponent<Block>().Transform(Level.Material.Air);
	this.markedForBreak = true;
    }
}
