Shader "MyShaders/MenuSelect"
{
    Properties
    {
        
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex: SV_POSITION;
                float4 position: TEXCOORD1;
                float4 uv: TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
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
                float dist = cos(floor(0.5 + theta / rad) * rad-theta) * length(pos);

                return 1.0 - smoothstep(radius, radius + edgeThickness, dist);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 color = (0, 0, 0);
                fixed3 white = 1;
                
                color += polygon(i.uv, float2(0.62 - sin(_Time.w) * 0.02, 0.5), 0.005, 3, 0.0, 0.001) * white;
                color += polygon(i.uv, float2(0.38 - sin(_Time.w + UNITY_PI) * 0.02, 0.5), 0.005, 3, UNITY_PI, 0.001) * white;
                return fixed4(color, 1);
            }
            ENDCG
        }
    }
}
