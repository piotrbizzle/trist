using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShootableBlock : Shootable {    
    public override void Start() {	
	base.Start();
	this.gameObject.AddComponent<Block>().material = Level.Material.Breakable;	
    }

    public override void Break() {
	// set blocks to Air
	this.gameObject.GetComponent<Block>().Transform(Level.Material.Air);
	base.Break();
    }
}
