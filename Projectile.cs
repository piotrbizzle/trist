using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Moveable {
    public float lifetime;
    
    public override void Update() {
	this.lifetime -= Time.deltaTime;
	if (this.lifetime <= 0) {
	    GameObject.Destroy(this.gameObject);
	    return;
	}
	base.Update();
    }
}
