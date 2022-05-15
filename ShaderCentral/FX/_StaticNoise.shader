Shader "MyShaders/Noise"
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

            #include "UnityCG.cginc"

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

            float random (float2 pos, float seed) {
                const float a = 12.9898;
                const float b = 78.233;
                const float c = 43758.543123;
                return frac(sin(dot(pos, float2(a, b)) + seed) * c );
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 color = random(i.uv, _Time.y) * fixed3(1,1,1);
                
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
