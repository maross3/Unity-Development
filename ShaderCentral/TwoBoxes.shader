Shader "MyShaders/TwoBoxes"
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
                float4 position: TEXCOORD1;
                float2 uv : TEXCOORD0;
            };
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.position = v.vertex;
                o.uv = v.texcoord;
                return o;
            }
            
            float rect(float2 pos, float2 size, float center)
            {
                float2 pt = pos - center;
                float2 halfSize = size * 0.5;

                float2 validSpace = step(-halfSize, pt) - step(halfSize, pt);
                
                return validSpace.x * validSpace.y;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 firstPos = i.position.xy;
                float2 firstSize = 0.3;
                float2 firstCenter = float2(-0.25, 0);

                float2 secondPos = i.position.xy;
                float2 secondSize = 0.25;
                float2 secondCenter = 0.25;
                
                float firstRect = rect(firstPos, firstSize, firstCenter);
                float secondRect = rect(secondPos, secondSize, secondCenter);
                
                fixed3 color = fixed3(1, 1, 0) * firstRect + fixed3(0, 0, 1) * secondRect;
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
