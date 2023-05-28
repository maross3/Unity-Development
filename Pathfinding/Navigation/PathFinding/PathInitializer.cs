using System.Collections;
using _Dev._Mike.Scripts.Ground.PathFinding;
using _Dev._Mike.Scripts.Ground.PathFinding.DataStructure;
using _Dev._Mike.Scripts.Ground.PathFinding.SearchClasses;
using GameMechanics.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Navigation.PathFinding
{
    public class PathInitializer : MonoBehaviour
    {
        #region Properties

        private PathMaster pathMaster;

        private Graph graph;

        [BoxGroup("Path")] public Vector2 startNode;
        [BoxGroup("Path")] public Vector2 endNode;

        [Button]
        void TestPathfinding()
        {
            // StartCoroutine(pathMaster.StartSearch());
        }

        #endregion

        private void Start()
        {
            pathMaster = gameObject.AddComponent<AStar>();
            StartCoroutine(WaitForGrid());
        }

        private IEnumerator WaitForGrid()
        {
            var floorGrid = GetComponent<FloorGrid>();
            yield return new WaitUntil(() => floorGrid.graphLoaded);
            graph = floorGrid.graph;
            PathfindSetup();
        }

        // todo, why is this commented out?
        void PathfindSetup()
        {
            /*
            DevNode[] failCase = new DevNode[2];
            DevNode start = null;
            try
            {
                start = graph.nodes[(int) startNode.x, (int) startNode.y];
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Log($"Position node out of range. {e}");
                
                startNode.x = UnityEngine.Random.Range(0, graph.nodes.GetLength(0));
                startNode.y = UnityEngine.Random.Range(0, graph.nodes.GetLength(1));
                start = graph.nodes[(int)startNode.x, (int)startNode.y];
                
                failCase[0] = new DevNode(-1, -1, NodeStatus.Blocked);
            }

            DevNode end = null;
            
            try
            {
                end = graph.nodes[(int) endNode.x, (int) endNode.y];
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Log($"Position node out of range. {e}");
                   
                endNode.x = UnityEngine.Random.Range(0, graph.nodes.GetLength(0));
                endNode.y = UnityEngine.Random.Range(0, graph.nodes.GetLength(1));
                end = graph.nodes[(int)endNode.x, (int)endNode.y];
                
                failCase[1] = new DevNode(-1, -1, NodeStatus.Blocked);
            }

            if (failCase[0] != null || failCase[1] != null)
            {
                //return failCase ?
            }

            var tileS = (GroundTile) start.payload;
            tileS.ChangeSize(0.7f);
            tileS.VisualizeSprite(Color.green);

            var tileE = (GroundTile) end.payload;
            tileE.ChangeSize(0.7f);
            tileE.VisualizeSprite(Color.red);
            */
            
            graph.Init();
            pathMaster.Init(graph);
        }
    }
}