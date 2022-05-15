Shader "MyShaders/Radar"
{
    Properties
    {
        _AxisColor("Axis Color", Color) = (0.8, 0.8, 0.8, 1)
        _SweepColor("Sweep Color", Color) = (0.1, 0.3, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
         
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 position: TEXCOORD1;
                float2 uv: TEXCOORD0;
            };
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.position = v.vertex;
                o.uv = v.texcoord;
                return o;
            }
            
            float getDelta(float x){
                return (sin(x)+1.0)/2.0;
            }

            // sweep and gradient
            float sweep(float2 pos, float2 center, float radius, float lineWidth, float edgeThickness){
                float2 pt = pos - center;
                float theta = _Time.z;
                
                float2 projectedPoint = float2(cos(theta), -sin(theta)) * radius;
                
                // clamp between 0 and 1
                float validDrawPt = clamp(dot(pt, projectedPoint) / dot(projectedPoint, projectedPoint), 0.0, 1.0);
                float lngth = length(pt - projectedPoint * validDrawPt);

                float gradient = 0;
                

                if(length(pt) < radius)
                {
                    
                const float gradientAngle = UNITY_PI * 0.5; // pi/2
                    
                // returns remainder to return # between 0 and 2Pi, 
                float angle = fmod(theta + atan2(pt.y, pt.x), UNITY_TWO_PI);
                    
                // clamp in between 0 and gradient angle / half of gradient angle
                gradient = clamp(gradientAngle - angle, 0, gradientAngle) / gradientAngle * 0.5;
                    
                }
                return gradient + 1.0 - smoothstep(lineWidth, lineWidth + edgeThickness, lngth);
            }

            // outline with smoothstep
            float circle(float2 pos, float2 center, float radius, float lineWidth, float edgeThickness)
            {
                float2 pt = pos - center;
                float lineLength = length(pt);
                float halfLineWidth = lineWidth / 2.0;
                return smoothstep(radius - halfLineWidth - edgeThickness, radius - halfLineWidth, lineLength) -
                        smoothstep(radius + halfLineWidth, radius + halfLineWidth + edgeThickness, lineLength);
            }

            // draw line
            float online(float start, float end, float lineWidth, float edgeThickness)
            {
                float halfWidth = lineWidth * 0.5;
                return smoothstep(start - halfWidth - edgeThickness, start - halfWidth, end) -
                        smoothstep(start + halfWidth, start + halfWidth + edgeThickness, end);
            }
            
            fixed4 _AxisColor;
            fixed4 _SweepColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 color = online(i.uv.y, 0.5, 0.002, 0.001) * _AxisColor;// x axis
                color += online(i.uv.x, 0.5, 0.002, 0.001) * _AxisColor; // y axis

                float2 center = 0.5;
                color += circle(i.uv, center, 0.3, 0.002, 0.001) * _AxisColor; // circle one
                color += circle(i.uv, center, 0.2, 0.002, 0.001) * _AxisColor; // circle two
                color += circle(i.uv, center, 0.1, 0.002, 0.001) * _AxisColor; // circle three

                color += sweep(i.uv, center, 0.3, 0.003, 0.001) * _SweepColor; // radar sweep
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
