using Environment;
using GameMechanics.Navigation.PathFinding.DataStructure;
using Global.Enum;
using UnityEngine;

namespace Navigation.Interface
{
    public interface IFloorTile
    {
        CropType crop { get; set; }
        Vector2 TilePos { get; set; }
        NodeStatus Status { get; set; }
        void AssignRowCol(int row, int col);
        DevNode TileNode { get; set; }
        EnvironmentOverseer overseer { get; set; }
        void AssignNodeObject();
    }
}