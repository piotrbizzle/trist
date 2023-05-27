using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour {
    // configurables
    public float projectileSpeed;
    public Sprite projectileSprite;
    public bool breakOnHit = true;
    public bool affectedByGravity = false;
    public int ammo = -1;
	
    public Vector3 aimVector;
        
    public void Fire() {
	if (this.ammo == 0) {
	    return;
	} else if (this.ammo > 0) {
	    this.ammo--;
	}
	
	GameObject projectileGo = new GameObject();
	projectileGo.transform.position = this.transform.position;
	projectileGo.transform.rotation = this.transform.rotation;
	projectileGo.AddComponent<SpriteRenderer>().sprite = this.projectileSprite;
	projectileGo.transform.SetParent(this.transform.parent.parent);

	Projectile projectile = projectileGo.AddComponent<Projectile>();
	projectile.AddMomentum(this.aimVector.x * this.projectileSpeed, this.aimVector.y * this.projectileSpeed);
	projectile.breakOnHit = this.breakOnHit;
	projectile.affectedByGravity = this.affectedByGravity;
    }
}
