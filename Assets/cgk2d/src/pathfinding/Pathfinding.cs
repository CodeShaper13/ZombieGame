using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

[RequireComponent(typeof(PathRequestManager))]
[RequireComponent(typeof(NavGrid))]
public class Pathfinding : MonoBehaviour {
	
	private PathRequestManager requestManager;
	private NavGrid grid;
	
	private void Awake() {
        this.requestManager = this.GetComponent<PathRequestManager>();
        this.grid = this.GetComponent<NavGrid>();
	}	
	
	public void StartFindPath(PathRequest request) {
        this.StartCoroutine(this.FindPath(request.pathStart, request.pathEnd, request.ignoreUnwalkableStartAndEnd));
	}
	
	private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, bool ignoreUnwalkableStartAndEnd) {		
		Stopwatch sw = new Stopwatch();
		sw.Start();
		
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;
		
		Node startNode = this.grid.nodeFromWorldPoint(startPos);
		Node targetNode = this.grid.nodeFromWorldPoint(targetPos);

		startNode.parent = startNode;

        if(startNode != null && targetNode != null && (ignoreUnwalkableStartAndEnd || (startNode.walkable && targetNode.walkable))) {
            Heap<Node> openSet = new Heap<Node>(this.grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);
			
			while (openSet.Count > 0) {
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);
				
				if (currentNode == targetNode) {
					sw.Stop();
//					print ("Path found: " + sw.ElapsedMilliseconds + " ms");
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in this.grid.getNeighbors(currentNode)) {
                    if(closedSet.Contains(neighbour)) {
                        continue; // already visited this node.
                    }
                    if(!neighbour.walkable) {
                        // Node can't be walked on, only continue if ignoreUnwalkableStartAndEnd is true and node is end node.
                        if(!(ignoreUnwalkableStartAndEnd == true && neighbour == targetNode)) {
                            continue;
                        }
                    }
					
					int newMovementCostToNeighbour = currentNode.gCost + this.getDistance(currentNode, neighbour) + neighbour.movementPenalty;
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = this.getDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						
						if (!openSet.Contains(neighbour)) {
							openSet.Add(neighbour);
                        } else {
							openSet.UpdateItem(neighbour);
                        }
					}
				}
			}
		}
		yield return null;
		if (pathSuccess) {
			waypoints = this.retracePath(startNode, targetNode);
		}
		this.requestManager.FinishedProcessingPath(waypoints,pathSuccess);		
	}
	
	private Vector3[] retracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

        Vector3[] waypoints;
        if(path.Count == 1) {
            waypoints = new Vector3[] { path[0].worldPosition };
        }
        else {
            waypoints = this.simplifyPath(path);
            Array.Reverse(waypoints);
        }

		return waypoints;		
	}
	
	private Vector3[] simplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i++) {
			Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX,path[i - 1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i - 1].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
	private int getDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY) {
			return 14 * dstY + 10 * (dstX - dstY);
        }
		return 14 * dstX + 10 * (dstY - dstX);
	}
}
