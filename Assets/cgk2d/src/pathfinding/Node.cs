using UnityEngine;

public class Node : IHeapItem<Node> {
	
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
	public int movementPenalty;

	public int gCost;
	public int hCost;
	public Node parent;
	int heapIndex;
	
	public Node(bool walkable, Vector3 worldPos, int x, int y, int penalty) {
		this.walkable = walkable;
		this.worldPosition = worldPos;
        this.gridX = x;
        this.gridY = y;
        this.movementPenalty = penalty;
	}

	public int fCost {
		get {
			return this.gCost + this.hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = this.fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = this.hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}

    public override string ToString() {
        return "Node(" + this.gridX + "," + this.gridY + ")(walkable:" + this.walkable.ToString() + ")";
    }
}
