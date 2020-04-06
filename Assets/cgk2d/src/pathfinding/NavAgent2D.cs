using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class NavAgent2D : MonoBehaviour {

	public float speed = 1;

	private Vector3[] path;
	private int targetIndex;
    public float stoppingDistance;

    private void OnDrawGizmos() {
        if(this.hasPath()) {
            for(int i = this.targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(this.path[i], Vector3.one);

                if(i == targetIndex) {
                    Gizmos.DrawLine(this.transform.position, this.path[i]);
                }
                else {
                    Gizmos.DrawLine(this.path[i - 1], this.path[i]);
                }
            }
        }
    }

    private void OnDestroy() {
        this.StopAllCoroutines();
    }

    /// <summary>
    /// Sets the Agent's destination.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="stoppingDistance"> The distance from the destination to stop at. </param>
    /// <param name="ignoreUnwalkableStartAndEnd"> If true, the start and end nodes will be counted as walkable, even if the node is not. </param>
    public void setDestination(Vector3 destination, bool ignoreUnwalkableStartAndEnd = true) {
        // If the path's target is the agents current spot, do nothing.
        if(destination == this.transform.position) {
            return;
        }
        PathRequestManager.RequestPath(this.transform.position, destination, ignoreUnwalkableStartAndEnd, this);
    }

    /// <summary>
    /// Stops the Agent.  This will delete the path.
    /// </summary>
    public void stop() {
        this.path = null;
        this.StopCoroutine("callback_followPath");
    }

    /// <summary>
    /// Returns true if the Agent has a path that they are following.
    /// </summary>
    public bool hasPath() {
        return this.path != null && this.path.Length > 0;
    }

    /// <summary>
    /// Returns the distance from the Agent to the end of the path in a straight line.
    /// -1 is returned if the Agent has no path.
    /// </summary>
    public float getDirectDistanceToEnd() {
        if(!this.hasPath()) {
            return -1f;
        }
        return Vector2.Distance(this.transform.position, this.path[this.path.Length - 1]);
    }

    /// <summary>
    /// Returns the distance the Agent has left on their path.
    /// -1 is returned if the Agent has no path.
    /// </summary>
    public float getDistanceToEnd() {
        if(!this.hasPath()) {
            return -1f;
        }

        // Start off with the distance from the miner to the next node.
        float dis = Vector2.Distance(this.transform.position, this.path[this.targetIndex]);

        // Total the distances between the nodes.
        for(int i = this.targetIndex; i < this.path.Length - 1; i++) {
            dis += Vector2.Distance(this.path[i], this.path[i + 1]);
        }

        return dis;
    }

    public void onPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			this.path = newPath;
			this.targetIndex = 0;

            this.StopCoroutine("callback_followPath");
			this.StartCoroutine("callback_followPath");
		}
	}

	private IEnumerator callback_followPath() {
		Vector3 currentWaypoint = path[0];

		while (this.hasPath()) {
            // Check if the Agent should stop early.
            if(this.stoppingDistance > 0) {
                if(this.getDirectDistanceToEnd() <= this.stoppingDistance) {
                    this.stop();
                    yield break;
                }
            }

            if (this.transform.position == currentWaypoint) {
				this.targetIndex ++;
				if (this.targetIndex >= this.path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

            // Move the Agent.
			this.transform.position = Vector3.MoveTowards(this.transform.position, currentWaypoint, this.speed * Time.deltaTime);

			yield return null;
		}
	}
}
