using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level : MonoBehaviour {
    public enum Material {Air, Concrete};

    public int blocksX;
    public int blocksY;

    private Material[,] grid;

    public void Start() {
	// init grid
	this.grid = new Material[this.blocksX, this.blocksY];
	for (int i = 0; i < this.grid.GetLength(0); i++) {
	    for (int j = 0; j < this.grid.GetLength(1); j++) {
		this.grid[i,j] = Material.Air;
	    }
	}
    }

    public Material GetBlock(int x, int y) {
	if (x < 0 || x >= this.blocksX || y < 0 || y >= this.blocksY) {
	    return Material.Air;
	}
	return this.grid[x,y];
    }
    
    public void SetBlock(int x, int y, Material material) {
	this.grid[x,y] = material;
    }
}
