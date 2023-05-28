using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunItem : Moveable {
    // constants
    public static float EmptyGunLifetime = 3f;
    public static float GunFlySpeed = 6f;

    // gun template
    public Gun gun;

    private bool clicked;
    private bool expires;
    private float lifetime;

    // related objects
    private Player player;

    
    public override void Start() {
	base.Start();
	
	this.player = GameObject.Find("level/player").GetComponent<Player>();
	this.gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
	
	if (this.gun.ammo == 0) {
	    this.expires = true;
	    this.lifetime = GunItem.EmptyGunLifetime;
	}
    }
    
    public override void Update() {
	// empty guns expire
	if (this.expires && !this.clicked) {
	    this.lifetime -= Time.deltaTime;
	    if (this.lifetime <= 0) {
		GameObject.Destroy(this.gameObject);
		return;
	    }
	}

	if (this.clicked) {
	    // check if touching player	    
	    Vector3 towardPlayerVector = (this.player.transform.position - this.transform.position);
	    if (towardPlayerVector.magnitude < 1f) {
		if (this.player.transform.childCount == Player.MaxGuns) {
		    // unclick if player guns are full
		    this.clicked = false;		   
		} else {
		    // add Gun to player then destroy this item
		    GameObject gunGo = new GameObject();
		    gunGo.AddComponent<Gun>().CopyFromTemplate(this.gun, this.gameObject.GetComponent<SpriteRenderer>().sprite);
		    gunGo.transform.SetParent(this.player.transform);
		    GameObject.Destroy(this.gameObject);
		    return;
		}
	    }

	    // move toward player
	    Vector3 moveVector = towardPlayerVector * GunItem.GunFlySpeed;
	    this.SetMomentumX(moveVector.x);
	    this.SetMomentumY(moveVector.y);
	}
	
	base.Update();
    }

    public void OnMouseOver() {
	// respond to right click only
	if (!Input.GetMouseButtonDown(1)) {
	    return;
	}
	this.clicked = true;	
    }
}
