using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar : MonoBehaviour {
    public Sprite[] sprites;

    public void SetHealth(int health) {
	this.GetComponent<SpriteRenderer>().sprite = this.sprites[health - 1];
    }
}
