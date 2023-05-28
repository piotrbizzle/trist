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

    private Dictionary<int, List<IntVector2>> targetRegistry = new Dictionary<int, List<IntVector2>>(){};
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

	List<IntVector2> targetSpacesList = new List<IntVector2>();
	for (int i = 0; i < target.gridWidth + 1; i++) {
	    for (int j = 0; j < target.gridHeight + 1; j++) {
		targetSpacesList.Add(new IntVector2(target.gridX + i, target.gridY + j));
		this.targetGrid[target.gridX + i, target.gridY + j].Add(target);	
	    }
	}
	this.targetRegistry[target.gameObject.GetInstanceID()] = targetSpacesList;
    }

    public void UnregisterTarget(Shootable target) {
	int targetID = target.gameObject.GetInstanceID();       
	List<IntVector2> targetPositions = this.targetRegistry[targetID];
	foreach (IntVector2 targetPosition in targetPositions) {
	    this.targetGrid[targetPosition.x, targetPosition.y].Remove(target);
	}
	this.targetRegistry.Remove(targetID);
    }   

    public void UpdateTarget(Shootable target) {
	if (target.markedForBreak) {
	    return;
	}
	if (target.gridX < 0 || target.gridX >= this.blocksX || target.gridY < 0 || target.gridY >= this.blocksY) {
	    // target out of level, destroy it
	    this.UnregisterTarget(target);
	    target.Break();
	    return;
	}

	// remove previous target grid entries
	int targetID = target.gameObject.GetInstanceID();
	List<IntVector2> previousPositions = this.targetRegistry[targetID];
	foreach (IntVector2 previousPosition in previousPositions) {
	    this.targetGrid[previousPosition.x, previousPosition.y].Remove(target);
	}

	// add new target grid entries
	List<IntVector2> targetSpacesList = new List<IntVector2>();
	for (int i = 0; i < target.gridWidth + 1; i++) {
	    for (int j = 0; j < target.gridHeight + 1; j++) {
		targetSpacesList.Add(new IntVector2(target.gridX + i, target.gridY + j));
		this.targetGrid[target.gridX + i, target.gridY + j].Add(target);	
	    }
	}
	this.targetRegistry[target.gameObject.GetInstanceID()] = targetSpacesList;

    }

    public List<Shootable> GetTargets(int x, int y) {
	if (x < 0 || x >= this.blocksX || y < 0 || y >= this.blocksY) {
	    // out of bounds
	    return new List<Shootable>();
	}
	return targetGrid[x, y];
    }
}
