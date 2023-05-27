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
    public float rateOfFire = 0.5f;
	
    public Vector3 aimVector;
    private float cooldown;

    public void Update() {
	if (this.cooldown > 0) {
	    this.cooldown -= Time.deltaTime;
	}
    }
	
    
    public void Fire() {
	if (this.ammo == 0 || this.cooldown > 0) {
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
	projectile.lifetime = 2.0f;

	this.cooldown = this.rateOfFire;
    }
}
