using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Dev._Mike.Scripts.Ground.PathFinding.DataStructure;
using _Dev._Mike.Scripts.Ground.PathFinding.Interface;
using GameMechanics.Navigation.PathFinding.DataStructure;
using Global.Enum;
using UnityEngine;

namespace _Dev._Mike.Scripts.Ground.PathFinding
{
    // TODO path master needs to grab List from floorgrid
    // as grid generates, it populates list, then feeds this.

    public abstract class PathMaster : MonoBehaviour
    {
        private const float Tick = 0.1f;

        protected Graph Graph;
        private bool _foundGoal;

        private WaitForSeconds tickTimer;

        // TODO Xfer to event handlers
        public Action initSearch;
        public Action draw;

        /// <summary>
        /// Event that invokes <see cref="IPathfinder.SetCurrentPath"/> <br></br>
        /// Contains an object reference to the <see cref="IPathfinder"/>, and a path reference <see cref="Path"/> of nodes
        /// </summary>
        public event EventHandler<Stack<DevNode>> PathFound;


        public bool showIterations;

        private int _iterations = 0;

        void Start()
        {
            //gameObject.AddComponent<PathDirector>();
        }

        public void Init(Graph graph)
        {
            Graph = graph;

            for (int x = 0; x < Graph.Width; x++)
            for (int y = 0; y < Graph.Height; y++)
                graph.nodes[x, y].Reset();

            _iterations = 0;
            _foundGoal = false;

            initSearch?.Invoke();

            Debug.Log(@$"length of graph nodes:{graph.nodes.Length}");
        }

        public void RequestPath(IPathfinder finder, DevNode goal)
        {
            PathFound += FeedDirectorCurrentPath;
            StartCoroutine(StartSearch(finder, goal));
        }

        void FeedDirectorCurrentPath(object finder, Stack<DevNode> path)
        {
          //  PathDirector.SetPath((IPathfinder) finder, path);
        }

        private IEnumerator StartSearch(IPathfinder finder, DevNode goal)
        {
            var start = finder.CurrentNode;

            if (start == null || goal == null || Graph == null || (start.yIndex == goal.yIndex && start.xIndex == goal.xIndex))
            {
                Debug.Log("Invalid argument in StartSearch ");
                CleanUp();
                PathFound?.Invoke(finder, new Stack<DevNode>());
                PathFound -= FeedDirectorCurrentPath;
                yield break;
            }
            if (start.status == NodeStatus.Blocked || goal.status == NodeStatus.Blocked)
            {
                CleanUp();
                Debug.Log("Goal or start is Blocked!");
                PathFound?.Invoke(finder, new Stack<DevNode>());
                PathFound -= FeedDirectorCurrentPath;
                yield break;
            }

            var frontNodes = new PriorityQueue<DevNode>();
            var exploredNodes = new List<DevNode>();

            var isComplete = false;
            frontNodes.Enqueue(start);
            start.distanceTraveled = 0;

            tickTimer = new WaitForSeconds(Tick);
            var startTime = Time.realtimeSinceStartup;
            var iterations = 0;
            yield return null;

            while (!isComplete)
            {
                iterations++;
                
                var curNode = frontNodes.Dequeue();

                if (!exploredNodes.Contains(curNode))
                    exploredNodes.Add(curNode);

                DoSearch(curNode, goal, ref frontNodes, ref exploredNodes);

                if (frontNodes.Contains(goal))
                {
                    isComplete = true;
                    PathFound?.Invoke(finder, GetPathNodes(goal));
                    PathFound -= FeedDirectorCurrentPath;
                }

                if (!showIterations) continue;
                draw?.Invoke();
                yield return tickTimer;
            }

            Debug.Log("Found in " + iterations + " steps");
        }

        public void CleanUp()
        {
            Graph.CleanUpNodes();
        }

        protected abstract void DoSearch(DevNode curNode, DevNode goalNode, ref PriorityQueue<DevNode> frontNodes,
            ref List<DevNode> exploredNodes);

        protected Stack<DevNode> GetPathNodes(DevNode end)
        {
            var path = new Stack<DevNode>();
            if (end == null) return path;

            var curNode = end;

            while (curNode != null)
            {
                // DEBUG
                // IDebuggable tile = (IDebuggable) curNode.payload;
                //if (tile != null) tile.VisualizeSprite(Color.blue);

                // TODO stack of vector 3 positions
                // run these on a clone of graph
                // We are 
                path.Push(curNode);
                curNode = curNode.previous;
            }

            CleanUp();
            return path;
        }
    }
}