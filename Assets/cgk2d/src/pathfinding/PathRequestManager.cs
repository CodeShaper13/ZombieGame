using UnityEngine;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {

    private static PathRequestManager instance;

    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	private PathRequest currentPathRequest;
    private Pathfinding pathfinding;
	private bool isProcessingPath;

	void Awake() {
		instance = this;
		this.pathfinding = this.GetComponent<Pathfinding>();
	}

    /// <summary>
    /// Request a new path.
    /// Once an attempt to plot a path has been made, successful or not,
    /// the callback Action is invoked.
    /// </summary>
	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, bool ignoreUnwalkableStartAndEnd, NavAgent2D agent) {
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, ignoreUnwalkableStartAndEnd, agent);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}

	private void TryProcessNext() {
		if(!this.isProcessingPath && this.pathRequestQueue.Count > 0) {
            this.currentPathRequest = this.pathRequestQueue.Dequeue();
            this.isProcessingPath = true;
            this.pathfinding.StartFindPath(this.currentPathRequest);
		}
	}

	public void FinishedProcessingPath(Vector3[] path, bool success) {
        if(this.currentPathRequest.agent != null) {
            this.currentPathRequest.agent.onPathFound(path, success);
            this.isProcessingPath = false;
            this.TryProcessNext();
        }
	}
}
