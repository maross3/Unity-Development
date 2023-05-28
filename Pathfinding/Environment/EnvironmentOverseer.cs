using System.Collections.Generic;
using _Dev._Mike.Scripts.Ground.PathFinding;
using _Dev._Mike.Scripts.Ground.PathFinding.SearchClasses;
using GameMechanics.Navigation.PathFinding.DataStructure;
using Navigation.Interface;
using UnityEngine;
using Graph = _Dev._Mike.Scripts.Ground.PathFinding.DataStructure.Graph;

namespace Environment
{
    public class EnvironmentOverseer : MonoBehaviour
    {
        private Graph _graph;
        private PathMaster _pathMaster;
        
        private int _environmentWidth;
        private int _environmentHeight;
        private int _environmentTileSize;
        void Awake()
        {
            _graph = gameObject.AddComponent<Graph>();
            _pathMaster = gameObject.AddComponent<AStar>();
            //   InitGraph(_environmentWidth, _environmentHeight, _environmentTileSize);
        }

        public void InitGraph(int width, int height, int tileSize)
        {
            _environmentWidth = width;
            _environmentHeight = height;
            _environmentTileSize = tileSize;
            
        // pre-processing
        SetupEnvironment(new Vector2Int(width, height));

        // processing
        for (int y = 0, yIndex = 0; y < height; y += tileSize, yIndex++)
        for (int x = 0, xIndex = 0; x < width; x += tileSize, xIndex++)
        {
            var projectedPos = new Vector2(x + tileSize / 2, y + tileSize / 2);
            var hit = Physics2D.Raycast(projectedPos, Vector2.zero);
            if (hit.collider != null)
            {
                // grab the floor tile 
                var tile = hit.collider.gameObject.GetComponent<IFloorTile>();
                tile.overseer = this;
                // define center of node
                tile.TilePos = projectedPos;

                // assign to graph
                AddNodeToGraph(tile, new Vector2Int(xIndex, yIndex));
            }
            else
            {
                Debug.Log(@$"Did not find a tile at ({x}, {y})");
            }
        }
        // post processing
        FinishSetup();
    }

        public void SetupEnvironment(Vector2Int tileMapIndexDimensions)
        {
            _graph.nodes = new DevNode[tileMapIndexDimensions.x, tileMapIndexDimensions.y];
            _graph.walls = new List<DevNode>();
        }

        public void AddNodeToGraph(IFloorTile tile, Vector2Int nodeIndex)
        {
            _graph.AddNodeToGraph(tile, nodeIndex.x, nodeIndex.y, tile.Status);
            tile.TileNode = _graph.nodes[nodeIndex.x, nodeIndex.y];
            tile.AssignNodeObject();
        }

        public void FinishSetup()
        {
            _graph.Init();
            _pathMaster.Init(_graph);
        }
    }
}