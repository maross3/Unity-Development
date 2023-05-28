using System.Collections.Generic;
using UnityEngine;
using Utils;
using Color = UnityEngine.Color;

namespace UI
{
    public class EffectTile
    {
        public int row;
        public int column;
        public Rect bounds;
        public Texture2D texture;
        public List<Control> controls = new List<Control>();

        public EffectTile(Rect bounds)
        {
            if (bounds.width <= 0 || bounds.height <= 0) return;
            this.bounds = bounds;
            texture = new Texture2D((int) bounds.width, (int) bounds.height, TextureFormat.RGBA32, false, true);
        }

        public void OnDrawGizmosSelected()
        {
            bounds.GizmoSelectedRect(Color.cyan);
        }

        public void Draw()
        {
            texture.Init(bounds);
            texture.DrawRect(bounds, Color.green);
            controls.ForEach(control => control.Draw());
            texture.End(bounds);
        }
    }
}