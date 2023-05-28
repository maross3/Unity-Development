using System.Collections.Generic;
using _Dev._Mike.Scripts.Ground.PathFinding.DataStructure;
using GameMechanics.Navigation.PathFinding.DataStructure;

namespace _Dev._Mike.Scripts.Ground.PathFinding.SearchClasses
{
    public class AStar : PathMaster
    {
        protected override void DoSearch(DevNode curNode, DevNode goalNode, ref PriorityQueue<DevNode> frontNodes, ref List<DevNode> exploredNodes)
        {
            if (curNode == null) return;
            
            for (int i = 0; i < curNode.neighbors.Count; i++)
            {
                var neighbor = curNode.neighbors[i];
                if (exploredNodes.Contains(neighbor)) 
                    continue;
                
                var neighborDistance = Graph.GetNodeDistance(curNode, neighbor);
                var distTraveled = neighborDistance + curNode.distanceTraveled + (int) curNode.status;
                
                if (float.IsPositiveInfinity(neighbor.distanceTraveled) || 
                    distTraveled < neighbor.distanceTraveled)
                {
                    neighbor.previous = curNode;
                    neighbor.distanceTraveled = distTraveled;
                }

                if (!frontNodes.Contains(neighbor) && Graph != null)
                {
                    var distToGoal = Graph.GetNodeDistance(neighbor, goalNode);
                    neighbor.priority = neighbor.distanceTraveled + distToGoal;
    
                    frontNodes.Enqueue(neighbor);
                }
            }
        }
    }
}