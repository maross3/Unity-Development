using System;
using System.Collections;
using System.Collections.Generic;
using _Dev._Mike.Scripts.Ground.PathFinding.DataStructure;
using ElectroMag._DevPooling;
using GameMechanics.Navigation.PathFinding.DataStructure;
using Navigation.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameMechanics.Navigation
{
    /// <summary>
    /// Generates a grid structure used for pathfinding
    /// </summary>
    public class FloorGrid : MonoBehaviour
    {
        #region Properties

        private bool LazyLoad => (lazyLoadTimer != 0) || lazyLoading;
        [HideInInspector] public Graph graph;

        [ShowIf("debug")] public bool graphLoaded = false;

        [BoxGroup("Processing")] [HorizontalGroup("Processing/Horizontal")] [SerializeField]
        private bool lazyLoading;

        [BoxGroup("Processing")] [HorizontalGroup("Processing/Horizontal")] [ShowIf("lazyLoading")] [SerializeField]
        private float lazyLoadTimer;

        [BoxGroup("Processing")] [SerializeField]
        private Vector2 gridSize;

        [PropertySpace(SpaceBefore = 15, SpaceAfter = 20)]
        [Button("Generate Grid", ButtonSizes.Large)]
        void GenerateNewGrid()
        {
            PlaceTiles();
        }

        [BoxGroup("PoolMaster Parameters")] [SerializeField]
        private string generatorName;

        [BoxGroup("PoolMaster Parameters")] [SerializeField]
        private string objectPoolName;

        private bool debug;

        [HideIf("debug")]
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        void ActivateDebug()
        {
            debug = true;
        }

        [ShowIf("debug")]
        [Button(ButtonSizes.Large), GUIColor(1, 0, 0)]
        void DeactivateDebug()
        {
            debug = false;
        }

        #endregion

        void Start()
        {
            graph = gameObject.AddComponent<Graph>();

            if (LazyLoad) StartCoroutine(DelayWork());
            else PlaceTiles();
        }

        IEnumerator DelayWork()
        {
            yield return new WaitForSeconds(lazyLoadTimer);
            PlaceTiles();
        }

        private void PlaceTiles()
        {
            // Grid horizontal cell count
            var gridX = (int) gridSize.x;
            // Grid vertical cell count
            var gridY = (int) gridSize.y;

            // 2D array of DevNodes 
            graph.nodes = new DevNode[gridX, gridY];
            graph.walls = new List<DevNode>();

            for (int y = 0; y < gridSize.y; y++)
            for (int x = 0; x < gridSize.x; x++)
                PlaceTile(x, y);

            graphLoaded = true;
        }

        void PlaceTile(int x, int y)
        {
            var go = PoolMaster.FetchObject(generatorName, objectPoolName);
            IFloorTile tile = go.GetComponent<IFloorTile>();

            go.transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
            tile.TilePos = new Vector2(go.transform.position.x, go.transform.position.y);
            if (tile == null)
                throw new Exception("Tile was null");

            tile.AssignRowCol(y, x);

            graph.AddNodeToGraph(tile, x, y, tile.Status);
            tile.TileNode = graph.nodes[x, y];

            go.SetActive(true);
        }
    }
}