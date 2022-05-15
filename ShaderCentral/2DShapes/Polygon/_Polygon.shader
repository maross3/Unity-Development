Shader "MyShaders/Polygon"
{
    Properties
    {
        _Sides("Sides", Int) = 3
        _Radius("Radius", Float) = 100.0
        _Rotation("Rotation", Range(0, 6.3)) = 0
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

            int _Sides;
            float _Radius;
            float _Rotation;
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos: TEXCOORD2;
                float4 position: TEXCOORD1;
                float2 uv: TEXCOORD0;
            };
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.position = v.vertex;
                o.uv = v.texcoord;
                return o;
            }
            
            float polygon(float2 pos, float2 center, float radius, int sides, float rotate, float edgeThickness){
                pos -= center;

                // Angle and radius from the current pixel
                float theta = atan2(pos.y, pos.x) + rotate;
                float rad = UNITY_TWO_PI/float(sides);

                // Shaping function that modulates the distance
                float dist = cos(floor(0.5 + theta / rad) * rad - theta) * length(pos);

                return 1.0 - smoothstep(radius, radius + edgeThickness, dist);
            }

            

            fixed4 frag (v2f i) : SV_Target
            {
                
                float2 pos = i.screenPos.xy * _ScreenParams.xy;
                float2 center = _ScreenParams.xy * 0.5;
                fixed3 color = polygon(pos, center, _Radius, _Sides, _Rotation, 1.0) * fixed3(0.5, 0.5, 1);
                
                
                return fixed4(color, 1);
            }
            ENDCG
        }
    }
}
