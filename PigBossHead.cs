using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PigBossHead : Shootable {
    // configurables
    public float orbitSpeed;

    private Vector3 hingePosition;
    private float currentOrbit;

    // related objects
    private Player player;
    public PigBoss pigBoss;
    
    public override void Start() {
	base.Start();
	this.player = GameObject.Find("level/player").GetComponent<Player>();
	this.hingePosition = this.transform.position;
    }

    public void HeadMove() {
	// position
	this.currentOrbit += this.orbitSpeed * Time.deltaTime;
	if (this.currentOrbit >= 360) {
	    this.currentOrbit -= 360;
	}
	this.transform.position = this.hingePosition + Quaternion.Euler(0, 0, this.currentOrbit) * Vector3.up * 0.5f;

	// rotation
	Quaternion baseRotation = Quaternion.FromToRotation(Vector3.up, this.player.transform.position - this.transform.position);
	this.transform.rotation = baseRotation;

    }

    public override void GetDestroyed() {
	this.pigBoss.heads.Remove(this);
    }
}
