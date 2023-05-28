using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace UI
{
    public abstract class Control : MonoBehaviour
    {
        public GameObject parent;
        protected Texture2D texture;
        
        public Rect bounds = new Rect(0,0,100,100);
        public Color backColor = Color.black;
        public Color foreColor = Color.yellow;
        public bool visible;
        public float X => bounds.x;
        public float Y => bounds.y;
        public float Width => bounds.width;
        public float Height => bounds.height;
        public Vector2 Position => bounds.position;
        protected bool invalidate;
        
        public abstract void Draw();

        [Button]
        public virtual void Invalidate() => invalidate = true;
        protected SpriteRenderer sprite;
        
        public virtual void Start()
        {
            var parentGo = gameObject;
            sprite = gameObject.GetComponent<SpriteRenderer>();
            sprite.gameObject.transform.position = new Vector3(bounds.x, bounds.y);
            parent = parentGo;
            texture = new Texture2D((int) bounds.width, (int)bounds.height, TextureFormat.RGBA32, false, true);
            sprite.sprite = Sprite.Create(texture, new Rect(0, 0, bounds.width, bounds.height), Vector2.zero);
            invalidate = true;
        }
        
        public virtual void Update()
        {
            if (!invalidate) return;
            invalidate = false;
            
            texture = new Texture2D((int) bounds.width, (int)bounds.height, TextureFormat.RGBA32, false, true);
            sprite.sprite = Sprite.Create(texture, new Rect(0, 0, bounds.width, bounds.height), Vector2.zero);
            sprite.gameObject.transform.position = new Vector3(bounds.x, bounds.y);
            
            Draw();
        }

        public virtual void ViewInEditor()
        {
            bounds.GizmoSelectedRect(Color.magenta);
        }
        
        [Button]
        public void InspectorInvalidate()
        {
            Start();
            Invalidate();
            Update();
        }
    }
}