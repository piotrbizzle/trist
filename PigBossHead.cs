using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PigBossHead : Shootable {
    // configurables
    public float orbitSpeed;
    public float weakenedTime;
    public Sprite weakSprite;
    public float reloadCooldown;
    
    private Sprite readySprite;
    private Vector3 hingePosition;
    private float currentOrbit;
    private float weakenedCooldown;
    
    // related objects
    private Player player;
    public PigBoss pigBoss;
    
    public override void Start() {
	base.Start();
	this.player = GameObject.Find("level/player").GetComponent<Player>();
	this.readySprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
	this.hingePosition = this.transform.position;
	this.pigBoss.totalHeads += 1;
    }

    public override void Update() {	
	// position
	this.currentOrbit += this.orbitSpeed * Time.deltaTime;
	if (this.currentOrbit >= 360) {
	    this.currentOrbit -= 360;
	}
	this.transform.position = this.hingePosition + Quaternion.Euler(0, 0, this.currentOrbit) * Vector3.up * 0.5f;	

	// rotation
	if (this.weakenedCooldown > 0) {
	    this.weakenedCooldown -= Time.deltaTime;
	    this.gameObject.GetComponent<SpriteRenderer>().sprite = this.weakSprite;
	} else {
	    this.gameObject.GetComponent<SpriteRenderer>().sprite = this.readySprite;	    	
	    Quaternion baseRotation = Quaternion.FromToRotation(Vector3.up, this.player.transform.position - this.transform.position);
	    this.transform.rotation = baseRotation;
	}
	
	// attack
	if (this.reloadCooldown > 0) {
	    this.reloadCooldown -= Time.deltaTime;	    
	} else {
	    Gun gun = this.gameObject.GetComponent<Gun>();
	    gun.aimVector = (this.player.transform.position - this.transform.position).normalized;
	    gun.Fire(false);
	    if (gun.ammo == 0) {		
		gun.ammo = gun.maxAmmo;
		this.weakenedCooldown = this.weakenedTime;
		this.reloadCooldown = gun.reloadTime;
	    }
	}

	base.Update();
    }

    public override void GetDestroyed() {
	this.pigBoss.totalHeads -= 1;
	if (this.pigBoss.totalHeads <= 0) {
	    this.pigBoss.Break();
	}
	base.GetDestroyed();
    }

    public override void Hit(int damage) {
	// immune unless weakened
	if (this.weakenedCooldown <= 0) {
	    return;
	}	
	this.health -= damage;
	if (this.health <= 0) {
	    this.Break();
	}
    }
}
