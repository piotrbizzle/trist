using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour {
    // configurables
    public float projectileSpeed;
    public Sprite projectileSprite;
    public bool breakOnHit = true;
    public bool affectedByGravity = false;
    public int maxAmmo = -1;
    public int ammo = -1;
    public float rateOfFire = 0.5f;
    public float reloadTime = 1.0f;
    public Sprite deselectedSprite;
    public Sprite selectedSprite;
    public Sprite itemSprite;
    public bool pulls;
    public int damage;
    public int numberOfProjectiles = 1;
    
    public Vector3 aimVector;
    private float cooldown;
    
    public void Update() {
	if (this.cooldown > 0) {
	    this.cooldown -= Time.deltaTime;
	}
    }	
    
    public void Fire(bool isFriendly) {
	// check if ready to fire
	if (this.cooldown > 0) {
	    return;
	} else if (this.ammo == 0) {
	    this.Discard(this.transform.parent.GetComponent<Player>().isFacingLeft);
	    return;
	} else if (this.ammo > 0) {
	    this.ammo--;
	}

	// create projectiles
	for (int i = 0; i < this.numberOfProjectiles; i++) {
	    GameObject projectileGo = new GameObject();
	    projectileGo.transform.position = this.transform.position;
	    projectileGo.transform.rotation = this.transform.rotation;
	    projectileGo.AddComponent<SpriteRenderer>().sprite = this.projectileSprite;
	
	    if (this.ammo != -1) {
		// pickupable weapon
		projectileGo.transform.SetParent(this.transform.parent.parent);
	    } else {
		// player weapon
		projectileGo.transform.SetParent(this.transform.parent);
	    }

	    
	    Projectile projectile = projectileGo.AddComponent<Projectile>();
	    Vector3 projectileVector = new Vector3(this.aimVector.x, this.aimVector.y, 0);
	    projectileVector = this.Scatter(projectileVector, i);
	    projectile.AddMomentum(projectileVector.x * this.projectileSpeed, projectileVector.y * this.projectileSpeed);

	    // pass on projectile properties
	    projectile.breakOnHit = this.breakOnHit;
	    projectile.affectedByGravity = this.affectedByGravity;
	    projectile.lifetime = 2.0f;
	    projectile.pulls = this.pulls;
	    projectile.isFriendly = isFriendly;
	    projectile.damage = this.damage;
	}

	this.cooldown = this.rateOfFire;
    }

    private Vector3 Scatter(Vector3 vector, int scatterAmount) {
	if (scatterAmount == 0) {
	    return vector;
	}
	return Quaternion.Euler(0, 0, Random.Range(-scatterAmount * 10f, scatterAmount * 10f)) * vector;			     
    }

    public void Discard(bool isFacingLeft) {
	if (this.maxAmmo != -1) {
	    // create GunItem
	    GameObject gunItemGo = new GameObject();
	    gunItemGo.transform.position = this.transform.position;
	    gunItemGo.transform.SetParent(this.transform.parent.parent);
	    gunItemGo.AddComponent<SpriteRenderer>().sprite = this.itemSprite;
	    GunItem gunItem = gunItemGo.AddComponent<GunItem>();
	    gunItem.gun = this.GetComponent<Gun>();

	    if (isFacingLeft) {
		gunItem.SetMomentumX(5f);
	    } else {
		gunItem.SetMomentumX(-5f);
	    }
	    gunItem.SetMomentumY(5f);
	    
	    // destroy Gun
	    GameObject.Destroy(this.gameObject);
	}
    }
	

    public void Select() {
	this.gameObject.GetComponent<SpriteRenderer>().sprite = this.selectedSprite;
    }

    public void Deselect() {
	this.gameObject.GetComponent<SpriteRenderer>().sprite = this.deselectedSprite;
    }
    
    public void CopyFromTemplate(Gun gun, Sprite itemSprite) {
	// there has to be a better way
	this.projectileSpeed = gun.projectileSpeed;
	this.projectileSprite = gun.projectileSprite;
	this.breakOnHit = gun.breakOnHit;
	this.affectedByGravity = gun.affectedByGravity;
	this.maxAmmo = gun.maxAmmo;
	this.ammo = gun.ammo;
	this.rateOfFire = gun.rateOfFire;
	this.selectedSprite = gun.selectedSprite;
	this.deselectedSprite = gun.deselectedSprite;
	this.damage = gun.damage;
	this.numberOfProjectiles = gun.numberOfProjectiles;
	this.itemSprite = itemSprite;

	
	// set sprite
	this.gameObject.AddComponent<SpriteRenderer>().sprite = this.deselectedSprite;
	this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Aimer";
    }
}
