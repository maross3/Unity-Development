using System;
using Global.Enum;
using Unity.VisualScripting;
using UnityEngine;

namespace _Dev._Mike.Scripts
{
    public class MouseArgs : EventArgs
    {
        /// <summary>
        /// The location of the mouse in vector2 format
        /// </summary>
        public Vector2 Location { get;}
        /// <summary>
        /// Current mouse button pressed. Empty if no buttons are pressed
        /// </summary>
        public MouseButtons Button { get; }
        /// <summary>
        /// used for observer indexing and tracing callstacks
        /// </summary>
        public MouseEventTypes EventType { get; }
        public MouseArgs(Vector2 mousePos2D, int buttonIndex, MouseEventTypes mouseEvent)
        {
            Location = mousePos2D;
            Button = buttonIndex switch
            {
                0 => MouseButtons.Left,
                1 => MouseButtons.Right,
                2 => MouseButtons.Middle,
                _ => MouseButtons.Empty
            };
        }
    }
}