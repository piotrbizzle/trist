using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Moveable {
    public float lifetime;
    public bool pulls;
    public bool isFriendly;
    public int damage;
    
    public override void Update() {
	this.lifetime -= Time.deltaTime;
	if (this.lifetime <= 0) {
	    GameObject.Destroy(this.gameObject);
	    return;
	}
	base.Update();
    }

    public override bool CheckPartialMove(Vector3 previousPosition, int previousGridX, int previousGridY) {
	// check if a target is hit
	// NOTE assumes projectiles don't occupy multiple spaces!
	List<Shootable> shootables = this.level.GetTargets(this.gridX, gridY);
	if (shootables.Count > 0) {
	    foreach (Shootable shootable in shootables) {
		// skip checking hits on friendly fire
		if (this.isFriendly == shootable.isFriendly) {
		    continue;
		}
		
		// skip checking hits on incompatible pull/pullable pairs
		if (!this.pulls && shootable.pullable) {
		    continue;
		}

		// don't things that are about to break
		if (shootable.markedForBreak) {
		    continue;
		}

		// precise hitbox check
		float shootableHalfWidth = shootable.GetComponent<SpriteRenderer>().sprite.rect.width / 200f;
		float shootableHalfHeight = shootable.GetComponent<SpriteRenderer>().sprite.rect.height / 200f;
		if (this.transform.position.x < shootable.transform.position.x - shootableHalfWidth) {
		    continue;
		}
		if (this.transform.position.x > shootable.transform.position.x + shootableHalfWidth) {		    
		    continue;
		}
		if (this.transform.position.y < shootable.transform.position.y - shootableHalfHeight) {
		    continue;
		}
		if (this.transform.position.y > shootable.transform.position.y + shootableHalfHeight) {		    
		    continue;
		}
		
		// end checking if we hit something
		shootable.Hit(this.damage);
		this.Break();
		return false;
	    }	   
	}
	
	// otherwise, check move as normal
	return base.CheckPartialMove(previousPosition, previousGridX, previousGridY);
    }

    public override bool DoesMaterialCollide(Level.Material material) {
	// projectiles don't deflect off breakables
	return material != Level.Material.Air && material != Level.Material.Breakable;
    }
}
