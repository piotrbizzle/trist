using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyGunHaver : NPC {
    // configurables
    public Sprite heldGunSprite;
    public Sprite gunItemSprite;
    public Gun gun;
    private GameObject gunGo;
    public float alertDistance;  // TODO: rework as zone
    private float runSpeed = 4f;    
    public Sprite standSprite;
    public Sprite reloadSprite;

    
    private float reloadCooldown;
    
    // related objects
    private Player player;
    
    public override void Start() {
	this.player = GameObject.Find("level/player").GetComponent<Player>();

	// add gun
	this.gunGo = new GameObject();
	this.gunGo.AddComponent<Gun>().CopyFromTemplate(this.gun, this.gunItemSprite);
	this.gunGo.GetComponent<SpriteRenderer>().sprite = this.heldGunSprite
;
	this.gunGo.transform.parent = this.transform;

	// set up sprites
	this.standSprite = this.GetComponent<SpriteRenderer>().sprite;
	
	base.Start();
    }    
    
    public override void NPCMove() {
	if (this.PlayerManhattanDistance() < this.alertDistance) {	    
	    this.Fight();
	} else {
	    this.gunGo.GetComponent<Gun>().aimVector = new Vector3(0, -1, 0);
	    this.gunGo.transform.rotation = Player.RotationsMap[0][0];
	    this.gunGo.transform.position = this.transform.position + new Vector3(0f, 0.5f, 0.0f);
	    this.Wander();
	}
    }
    
    public int PlayerManhattanDistance() {
	return Math.Abs(this.player.gridX - this.gridX) + Math.Abs(this.player.gridY - this.gridY);
    }

    public void Fight() {
	// reload if needed
	if (this.reloadCooldown > 0) {
	    this.SwapSprite(this.reloadSprite);
	    this.reloadCooldown -= Time.deltaTime;
	    return;
	}
	this.SwapSprite(this.standSprite);

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

	// aim at player
	Quaternion baseRotation = Quaternion.FromToRotation(Vector3.up, this.player.transform.position - this.transform.position);
	Gun gun = this.gunGo.GetComponent<Gun>();
	gun.aimVector = (this.player.transform.position - this.transform.position).normalized;
	this.gunGo.transform.rotation = baseRotation;
	
	// assume 2 units tall
	this.gunGo.transform.position = this.transform.position + new Vector3(0f, 0.5f, 0.0f);

	// try to fire gun
	gun.Fire(false);
	if (gun.ammo == 0) {
	    gun.ammo = gun.maxAmmo;
	    this.reloadCooldown = gun.reloadTime;
	}
    }

    public override void GetDestroyed() {	
	this.gunGo.GetComponent<Gun>().Discard(this.isFacingLeft);
	base.GetDestroyed();
    }

    private void SwapSprite(Sprite sprite) {
	this.transform.Translate(Vector3.down * ((float)(this.GetComponent<SpriteRenderer>().sprite.rect.height - sprite.rect.height) / 200f));
	this.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
