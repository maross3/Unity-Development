using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace UI
{
    public class EffectMatrix : Control
    {
        public int rows;
        public int columns;
        public int padding = 5;
        
        private List<EffectTile> tiles = new List<EffectTile>();

        public override void Start()
        {
            base.Start();
            InitializeMatrix();
        }
 
        [Button]
        public void InitializeMatrix()
        {
            tiles = new List<EffectTile>();

            var cellWidth = (Width - (columns + 1) * padding) / columns;
            var cellHeight = (Height - (rows + 1) * padding) / rows;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < columns; col++)
                {
                    var tileX = Position.x + padding + col * (cellWidth + padding);
                    var tileY = Position.y + padding + row * (cellHeight + padding);

                    var tile = new EffectTile(new Rect(tileX, tileY, cellWidth, cellHeight));
                    tile.row = row;
                    tile.column = col;
                    
                    tiles.Add(tile);
                }
            } 
        }
        
        public void Insert(int row, int column, EffectTile tile)
        {
            tiles[GetIndex(row, column)] = tile;
        }

        public void Add(EffectTile tile)
        {
            tiles.Add(tile);
        }

        public void Remove(int row, int column)
        {
            var index = GetIndex(row, column);
            tiles.RemoveAt(index);
        }

        public void Swap(int row1, int column1, int row2, int column2)
        {
            var index1 = GetIndex(row1, column1);
            var index2 = GetIndex(row2, column2);
            (tiles[index1], tiles[index2]) = (tiles[index2], tiles[index1]);
        }

        private int GetIndex(int row, int column)
        {
            return row * columns + column;
        }
#region Unity Drawing

        public override void Draw()
        {
            texture.Init(bounds);
            texture.DrawRect(bounds, Color.blue);
            texture.End(bounds);
            
            var cellWidth = (Width - (columns + 1) * padding) / columns;
            var cellHeight = (Height - (rows + 1) * padding) / rows; 
            tiles.ForEach(tile =>
            {
                var tileX = Position.x + padding + tile.column * (cellWidth + padding);
                var tileY = Position.y + padding + tile.row * (cellHeight + padding);
                tile.bounds = new Rect(tileX, tileY, cellWidth, cellHeight);
                tile.Draw();
            });
        }

        public override void ViewInEditor()
        {
            bounds.GizmoSelectedRect(Color.yellow);
            
            var cellWidth = (Width - (columns + 1) * padding) / columns;
            var cellHeight = (Height - (rows + 1) * padding) / rows; 
            
            tiles.ForEach(tile =>
            {
                var tileX = Position.x + padding + tile.column * (cellWidth + padding);
                var tileY = Position.y + padding + tile.row * (cellHeight + padding);
                tile.bounds = new Rect(tileX, tileY, cellWidth, cellHeight);
                tile.OnDrawGizmosSelected();
            });
        }
        
        #endregion
    }
}