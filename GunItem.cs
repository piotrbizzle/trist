using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunItem : Shootable {
    // constants
    public static float EmptyGunLifetime = 3f;
    public static float GunFlySpeed = 6f;

    // gun template
    public Gun gun;

    private bool pulled;
    private bool expires;
    private float lifetime;

    // related objects
    private Player player;

    
    public override void Start() {
	base.Start();

	this.pullable = true;
	this.player = GameObject.Find("level/player").GetComponent<Player>();
	
	if (this.gun.ammo == 0) {
	    this.expires = true;
	    this.lifetime = GunItem.EmptyGunLifetime;
	}
    }
    
    public override void Update() {
	// empty guns expire
	if (this.expires && !this.pulled && !this.markedForBreak) {
	    this.lifetime -= Time.deltaTime;
	    if (this.lifetime <= 0) {
		this.Break();
		return;
	    }
	}

	if (this.pulled && !this.markedForBreak) {
	    // check if touching player	    
	    Vector3 towardPlayerVector = (this.player.transform.position - this.transform.position);
	    if (towardPlayerVector.magnitude < 1f) {
		if (this.player.transform.childCount == Player.MaxGuns) {
		    // unclick if player guns are full
		    this.pulled = false;		   
		} else {
		    // add Gun to player then destroy this item
		    GameObject gunGo = new GameObject();
		    gunGo.AddComponent<Gun>().CopyFromTemplate(this.gun, this.gameObject.GetComponent<SpriteRenderer>().sprite);
		    gunGo.transform.SetParent(this.player.transform);
		    this.Break();
		    return;
		}
	    }

	    // move toward player
	    Vector3 moveVector = towardPlayerVector * GunItem.GunFlySpeed;
	    if ((moveVector.x < 0 && !this.IsSolidLeft()) || (moveVector.x >= 0 && !this.IsSolidRight())) {
		this.SetMomentumX(moveVector.x);
	    }
	    if ((moveVector.y < 0 && !this.OnSolidGround()) || (moveVector.y >= 0 && !this.IsSolidUp())) {
		this.SetMomentumY(moveVector.y);
	    }
	}
	
	base.Update();
    }

    public override void Hit() {
	this.pulled = true;
    }
}
