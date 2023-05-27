using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour {
    // configurables
    public float projectileSpeed;
    public Sprite projectileSprite;
    public Vector3 aimVector;
        
    public void Fire() {
	GameObject projectileGo = new GameObject();
	projectileGo.transform.position = this.transform.position;
	projectileGo.transform.rotation = this.transform.rotation;
	projectileGo.AddComponent<SpriteRenderer>().sprite = this.projectileSprite;
	projectileGo.transform.SetParent(this.transform.parent.parent);

	Projectile projectile = projectileGo.AddComponent<Projectile>();
	projectile.AddMomentum(this.aimVector.x * this.projectileSpeed, this.aimVector.y * this.projectileSpeed);
	projectile.breakOnHit = false;
	projectile.affectedByGravity = true;
    }
}
