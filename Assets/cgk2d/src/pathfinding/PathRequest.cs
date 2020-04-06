using System;
using UnityEngine;

public struct PathRequest {

    public Vector3 pathStart;
    public Vector3 pathEnd;
    public bool ignoreUnwalkableStartAndEnd;
    public NavAgent2D agent;

    public PathRequest(Vector3 _start, Vector3 _end, bool ignoreUnwalkableStartAndEnd, NavAgent2D agent) {
        this.pathStart = _start;
        this.pathEnd = _end;
        this.ignoreUnwalkableStartAndEnd = ignoreUnwalkableStartAndEnd;
        this.agent = agent;
    }
}