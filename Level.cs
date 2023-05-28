using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntVector2 {
    public int x;
    public int y;
    
    public IntVector2(int xIn, int yIn) {
	this.x = xIn;
	this.y = yIn;
    }
}

public class Level : MonoBehaviour {
    public enum Material {Air, Concrete};

    public int blocksX;
    public int blocksY;
    private Material[,] blockGrid;

    private Dictionary<int, IntVector2> targetRegistry = new Dictionary<int, IntVector2>(){};
    private List<Shootable>[,] targetGrid;

    
    public void Start() {
	// init grid
	this.targetGrid = new List<Shootable>[this.blocksX, this.blocksY];
	this.blockGrid = new Material[this.blocksX, this.blocksY];
	for (int i = 0; i < this.blockGrid.GetLength(0); i++) {
	    for (int j = 0; j < this.blockGrid.GetLength(1); j++) {
		this.blockGrid[i,j] = Material.Air;
		this.targetGrid[i,j] = new List<Shootable>();
	    }
	}
    }

    public Material GetBlock(int x, int y) {
	if (x < 0 || x >= this.blocksX || y < 0 || y >= this.blocksY) {
	    return Material.Air;
	}
	return this.blockGrid[x,y];
    }
    
    public void SetBlock(int x, int y, Material material) {
	this.blockGrid[x,y] = material;
    }

    // TODO: some targets may not be 1x1!
    public void RegisterTarget(Shootable target) {
	if (target.gridX < 0 || target.gridX >= this.blocksX || target.gridY < 0 || target.gridY >= this.blocksY) {
	    // target out of level
	    return;
	}
	this.targetRegistry[target.gameObject.GetInstanceID()] = new IntVector2(target.gridX, target.gridY);
	this.targetGrid[target.gridX, target.gridY].Add(target);	
    }

    public void UnregisterTarget(Shootable target) {
	int targetID = target.gameObject.GetInstanceID();
	IntVector2 targetPosition = this.targetRegistry[targetID];
	this.targetGrid[targetPosition.x, targetPosition.y].Remove(target);
	this.targetRegistry.Remove(targetID);
    }   

    public void UpdateTarget(Shootable target) {
	if (target.gridX < 0 || target.gridX >= this.blocksX || target.gridY < 0 || target.gridY >= this.blocksY) {
	    // target out of level, destroy it
	    this.UnregisterTarget(target);
	    target.Break();
	    return;
	}
	int targetID = target.gameObject.GetInstanceID();
	IntVector2 previousPosition = this.targetRegistry[targetID];
	this.targetRegistry[targetID] = new IntVector2(target.gridX, target.gridY);
	this.targetGrid[previousPosition.x, previousPosition.y].Remove(target);	
	this.targetGrid[target.gridX, target.gridY].Add(target);	
    }

    public List<Shootable> GetTargets(int x, int y) {
	if (x < 0 || x >= this.blocksX || y < 0 || y >= this.blocksY) {
	    // out of bounds
	    return new List<Shootable>();
	}
	return targetGrid[x, y];
    }
}
