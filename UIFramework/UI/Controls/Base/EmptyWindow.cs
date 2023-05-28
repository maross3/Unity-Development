using UnityEngine;
using Utils;

namespace UI
{
    public class EmptyWindow : Control
    {
        public override void ViewInEditor()
        {
            bounds.GizmoSelectedRect(Color.green);
        }
        
        public override void Draw()
        {
            texture.Init(bounds);
            texture.DrawRect(bounds, backColor);
            texture.End(bounds);
        }
    }
}