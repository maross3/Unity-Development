using UnityEngine;

namespace Utils
{
    public static class DrawingUtil
    {
        /// <summary>
        /// Validates a texture width and height, correcting if changed.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="bounds"></param>
        public static void ValidateTexture(ref Texture2D texture, Rect bounds)
        {
            var width = (int) bounds.width;
            var height = (int) bounds.height;
            if (width <= 0 || height <= 0) return;
            texture ??= new Texture2D(width, height, TextureFormat.RGBA32, false, true);
            if (texture.width != width || texture.height != height)
                texture = new Texture2D(width, height, TextureFormat.RGBA32, false, true);
        }

        /// <summary>
        /// Init GUI Drawing
        /// </summary>
        public static void Init(this Texture2D texture, Rect bounds)
        {
            ValidateTexture(ref texture, bounds);
            ClearTexture(texture, bounds);
        }

        /// <summary>
        /// End and apply gui calls to a texture
        /// </summary>
        public static void End(this Texture2D texture, Rect bounds)
        {
            if (texture.width <= 0 || texture.height <= 0) return;
            texture.Apply();
            GUI.DrawTexture(bounds, texture);
        }

        /// <summary>
        /// Clears a texture to transparent
        /// </summary>
        /// <param name="texture">The Texture</param>
        /// <param name="bounds">The expected bounds of the texture.</param>
        public static void ClearTexture(this Texture2D texture, Rect bounds)
        {
            if (texture.width <= 0 || texture.height <= 0)
            {
                texture.height = (int) bounds.height;
                texture.width = (int) bounds.width;
            }
            
            Color[] pixels = texture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = Color.clear;

            texture.SetPixels(pixels);
        }
        
        /// <summary>
        /// Draws a rect on a texture
        /// </summary>
        public static void DrawRect(this Texture2D texture, Rect bounds, Color color)
        {
            ValidateTexture(ref texture, bounds);
            var width = (int) bounds.width;
            var height = (int) bounds.height;

            var pixels = new Color[width * height];
            for (var i = 0; i < pixels.Length; i++) pixels[i] = color;

            texture.SetPixels(0, 0, width, height, pixels);
            texture.Apply();
        }
        
        /// <summary>
        /// Draws a circle on a texture
        /// </summary>
        public static void DrawCircle(this Texture2D texture, Vector2 center, float radius, Color color)
        {
            int width = texture.width;
            int height = texture.height;
            var centerX = (float) Mathf.RoundToInt(center.x);
            var centerY = (float) Mathf.RoundToInt(center.y);
            var radiusSquared = (float) Mathf.RoundToInt(radius * radius);

            Color[] pixels = texture.GetPixels();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var index = y * width + x;
                    var dx = x - centerX;
                    var dy = y - centerY;
                    if (dx * dx + dy * dy <= radiusSquared) pixels[index] = color;
                }
            }

            texture.SetPixels(pixels);
        }
        
        /// <summary>
        /// Draws a gizmo rect at the given center position with the given radius.
        /// </summary>
        public static void GizmoSelectedRect(this Rect rect, Color color = default)
        {
            Vector3 center = new Vector3(rect.x + rect.width / 2f, Screen.height - rect.y - rect.height  / 2f, 0f);
            Vector3 size = new Vector3(rect.width, rect.height, 0f);
            if (size.x <= 0 || size.y <= 0) return;
            
            Gizmos.color = color;
            Gizmos.DrawWireCube(center, size);
        }
        
        /// <summary>
        /// Draws a gizmo circle at the given center position with the given radius.
        /// </summary>
        public static void GizmoDrawCircle(Vector2 center, float radius, Color color = default, float rotationAngle = 0, int segments = 32)
        {
            if (radius <= 0) return;
            Gizmos.color = color;
            float angleStep = 2f * Mathf.PI / segments;

            Vector3 prevPoint = Vector3.zero;
            Vector3 currPoint = Vector3.zero;

            for (int i = 0; i <= segments; i++)
            {
                float angle = i * angleStep + rotationAngle;
                float x = center.x + Mathf.Cos(angle) * radius;
                float y = Screen.height - (center.y + Mathf.Sin(angle) * radius); // Invert Y-coordinate
                currPoint.Set(x, y, 0f);

                if (i > 0) Gizmos.DrawLine(prevPoint, currPoint);
                prevPoint = currPoint;
            }
        }
    }
}