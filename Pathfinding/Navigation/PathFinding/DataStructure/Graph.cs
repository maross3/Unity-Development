using System;
using System.Collections.Generic;
using GameMechanics.Navigation.PathFinding.DataStructure;
using Global.Enum;
using Navigation.Interface;
using UnityEngine;

namespace _Dev._Mike.Scripts.Ground.PathFinding.DataStructure
{
    public class Graph : MonoBehaviour
    {
        public int Width => _width;
        public int Height => _height;
        public DevNode[,] nodes;
        public List<DevNode> walls;

        private int[,] mapData;
        private int _width;
        private int _height;
        public static Action<IFloorTile, NodeStatus> ChangeTileStatus;
        public static Action ResetState;

        static readonly Vector2[] Directions =
        {
            new(0f, 1f),
            new(1f, 1f),
            new(1f, 0f),
            new(1f, -1f),
            new(0f, -1f),
            new(-1f, -1f),
            new(-1f, 0f),
            new(-1f, 1f)
        };

        #region Init

        public void Init()
        {
            _width = nodes.GetLength(0);
            _height = nodes.GetLength(1);

            walls = new List<DevNode>();
            for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
            {
                if (nodes[x, y].status == NodeStatus.Blocked) walls.Add(nodes[x, y]);
                else nodes[x, y].neighbors = GetNeighbors(x, y);
            }
            // ChangeTileStatus += ChangeTileStatusMethod;
            // ResetState += ResetToInit;
        }

        void ResetToInit()
        {
            walls.Clear();

            for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
            {
                nodes[x, y].Reset();
                if (nodes[x, y].status != NodeStatus.Blocked) nodes[x, y].neighbors = GetNeighbors(x, y);
                else walls.Add(nodes[x, y]);
            }
        }

        public void CleanUpNodes()
        {
            //walls.Clear(); 
            for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
            {
                nodes[x, y].Reset();

                //if (nodes[x,y].status == NodeStatus.Blocked) walls.Add(nodes[x,y]);
                // else 

                //nodes[x, y].neighbors = GetNeighbors(x, y);
            }
        }

        #endregion

        private void ChangeTileStatusMethod(IFloorTile obj, NodeStatus status)
        {
            var x = obj.TileNode.xIndex;
            var y = obj.TileNode.yIndex;
            nodes[x, y].status = status;

            if (status == NodeStatus.Blocked)
            {
                walls.Add(nodes[x, y]);
                nodes[x, y].neighbors.Clear();
            }
            else
            {
                if (walls.Contains(nodes[x, y]))
                    walls.Remove(nodes[x, y]);

                nodes[x, y].neighbors = GetNeighbors(x, y);
            }
        }

        public void AddNodeToGraph(IFloorTile payld, int x, int y, NodeStatus status)
        {
            var node = new DevNode(x, y, status, payld);
            nodes[x, y] = node;
            node.position = new Vector3(payld.TilePos.x, payld.TilePos.y, 0);
        }

        List<DevNode> GetNeighbors(int x, int y)
        {
            return GetNeighbors(x, y, nodes, Directions);
        }

        private List<DevNode> GetNeighbors(int x, int y, DevNode[,] nodeArray, Vector2[] directions)
        {
            var neighbors = new List<DevNode>(Directions.Length);
            foreach (var direction in Directions)
            {
                var neighborX = x + (int) direction.x;
                var neighborY = y + (int) direction.y;

                if (IsWithinBounds(neighborX, neighborY) &&
                    nodeArray[neighborX, neighborY] != null &&
                    nodeArray[neighborX, neighborY].status != NodeStatus.Blocked)
                    neighbors.Add(nodeArray[neighborX, neighborY]);
            }

            return neighbors;
        }

        public bool IsWithinBounds(int x, int y)
        {
            return (x >= 0 && x < _width && y >= 0 && y < _height);
        }

        public float GetNodeDistance(DevNode source, DevNode target)
        {
            var dx = Mathf.Abs(source.xIndex - target.xIndex);
            var dy = Mathf.Abs(source.yIndex - target.yIndex);

            var min = Mathf.Min(dx, dy);
            var max = Mathf.Max(dx, dy);

            var diagSteps = min;
            var straightSteps = max - min;

            return (1.4f * diagSteps + straightSteps);
        }

        public int GetManhattanDistance(DevNode source, DevNode target)
        {
            int dx = Mathf.Abs(source.xIndex - target.xIndex);
            int dy = Mathf.Abs(source.yIndex - target.yIndex);
            return (dx + dy);
        }
    }
}