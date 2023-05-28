using System.Collections.Generic;
using _Dev._Mike.Scripts.Ground.PathFinding.DataStructure;
using GameMechanics.Navigation.PathFinding.DataStructure;

namespace _Dev._Mike.Scripts.Ground.PathFinding.Interface
{
    public interface IPathfinder
    {
        /// <summary>
        /// Sets the current path of the finder. Object is who the path is intended for
        /// </summary>
        /// <param name="finder"></param>
        /// <param name="newPath"></param>
        void SetCurrentPath(Stack<DevNode> newPath);

        bool HasPath { get; set; }
        Stack<DevNode> CurrentPath { get; set; }
        DevNode CurrentNode { get; set; }
    }
    
}

