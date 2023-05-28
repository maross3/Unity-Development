using System;
using System.Collections.Generic;
using Global.Enum;
using UnityEngine;

namespace GameMechanics.Navigation.PathFinding.DataStructure
{
    public class DevNode : IComparable<DevNode>
    {
        public int CompareTo(DevNode other)
        {
            if (priority < other.priority) return -1;
            return priority > other.priority ? 1 : 0;
        }

        public NodeStatus status = NodeStatus.Open;
        public int xIndex = -1, yIndex = -1;
        public Vector3 position;
        public List<DevNode> neighbors = new List<DevNode>(); // should not be pub?
        public float distanceTraveled = Mathf.Infinity; // to -1, can't travel neg
        public DevNode previous = null;
        public float priority;
        public object payload;
        public GameObject nodeObject;

        public DevNode(int xIdx, int yIdx, NodeStatus ndeSts, object payld)
        {
            xIndex = xIdx;
            yIndex = yIdx;
            status = ndeSts;
            payload = payld;
        }

        public DevNode(int xIdx, int yIdx, NodeStatus ndeSts)
        {
            xIndex = xIdx;
            yIndex = yIdx;
            status = ndeSts;
        }

        public void Reset()
        {
            previous = null;
            distanceTraveled = Mathf.Infinity;
            priority = 0;
        }
    }
}