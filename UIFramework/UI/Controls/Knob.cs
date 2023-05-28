using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Controls
{
    // todo, make constraints for knob
    // todo, make a wave tile
    // todo, integrate wave tile into effect matrix
    // todo, update the sample based off the tile
    // once that is finished, lets hook up the synthesizer to play on key press
    // we need to edit the pitch based off the key
    public class Knob : Control
    {
        public float Value;
        public int radius;
        public float angle;
        public float rotationSpeed; 
        public int indicatorRadius = 20;
        public int indicatorDistanceFromCenter = 20;
        
        private Image image;
        private Vector3 mousePos;
        private GameObject pivotObject;
        public override void Start()
        {
            bounds = new Rect(Position.x, Position.y, radius * 2, radius * 2);
            base.Start();

        }

        private void OnMouseDown()
        {
            mousePos = Input.mousePosition;
        }

        private void OnMouseUp()
        {
            mousePos = Vector3.zero;
        }

        private void OnMouseDrag()
        {
            Vector3 mousePosition = Input.mousePosition;
            var diff = mousePosition - mousePos;
            mousePos = mousePosition;
            
            // Calculate the rotation speed based on the distance and speed of mouse movement
            var rotation = (diff.x + diff.y) * Time.deltaTime * rotationSpeed;

            Bounds newbounds = sprite.bounds;
            Vector3 center = newbounds.center;
            sprite.transform.RotateAround(center, Vector3.forward, rotation);
            Value = Mathf.Repeat(sprite.transform.eulerAngles.z, 360f) / 360f;
        }

        public override void Update()
        {
            if (!invalidate) return;
            bounds = new Rect(Position.x, Position.y, radius * 2, radius * 2);
            base.Update();
        }

        /// <summary>
        /// draws knob for mixing.
        /// </summary>
        /// <TODO>
        /// Some reason it doesn't allow dynamic scaling. texture bug on unity's end?
        /// </TODO>
        public override void Draw()
        {
            var spriteTexture = sprite.sprite.texture;
            spriteTexture.ClearTexture(new Rect(0,0, bounds.width, bounds.height));
            spriteTexture.DrawCircle(new Vector2(radius, radius), radius, backColor);

            // Calculate the position of the inner circle based on the angle and distance
            var innerPosition = new Vector2(
                radius + Mathf.Cos(angle * rotationSpeed * Mathf.Deg2Rad) * indicatorDistanceFromCenter,
                radius - Mathf.Sin(angle * rotationSpeed * Mathf.Deg2Rad) * indicatorDistanceFromCenter
            );
                
            spriteTexture.DrawCircle(innerPosition, indicatorRadius, foreColor);
            spriteTexture.Apply();
        }

        
        public override void ViewInEditor()
        {
            DrawingUtil.GizmoDrawCircle(Position, radius, Color.green, angle);
    
            var smallCircleRadius = radius / 4;
    
            // Adjust the distance from the center as desired
            var distanceFromCenter = smallCircleRadius * 3; 
            
            // Adjust the size of the inner circle as desired
            var innerCircleRadius = smallCircleRadius / 2; 
    
            // Calculate the position of the inner circle based on the angle and distance
            var innerPosition = new Vector2(
                Position.x + Mathf.Cos(angle * rotationSpeed * Mathf.Deg2Rad) * distanceFromCenter,
                Position.y + Mathf.Sin(angle * rotationSpeed * Mathf.Deg2Rad) * distanceFromCenter
            );
    
            DrawingUtil.GizmoDrawCircle(innerPosition, innerCircleRadius, Color.red);
        }

    }
}