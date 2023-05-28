using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace UI
{
    // todo, this is garbage, I think.
    // the ui system is going to have a designer, This would be more like the form code
    public class SynthesizerUI : Control
    {
        public EffectMatrix effectMatrix;
        public Rect gridRect = new Rect(0, 0, 100, 100);
        public int rows = 1;
        public int columns = 1;
        
        public override void ViewInEditor()
        {
            bounds.GizmoSelectedRect(Color.blue);
            effectMatrix ??= gameObject.AddComponent<EffectMatrix>();
            effectMatrix.ViewInEditor();
        }

        public override void Start()
        {
            texture = new Texture2D((int) bounds.width, (int)bounds.height, TextureFormat.RGBA32, false, true);
            effectMatrix = gameObject.AddComponent<EffectMatrix>(); 
        }


        public override void Draw()
        {
            texture.Init(bounds);
            texture.DrawRect(bounds, Color.red);
            texture.End(bounds);
            
            effectMatrix.bounds = gridRect;
            effectMatrix.Draw();
        }
    }
}