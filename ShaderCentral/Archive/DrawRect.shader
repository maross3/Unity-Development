Shader "MyShaders/DrawRect"
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
                
                float horizontal = step(-halfSize.x, pt.x) - step(halfSize.x, pt.x);
                float verticle = step(-halfSize.y, pt.y) - step(halfSize.y, pt.y);
                
                return horizontal * verticle;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos = i.position.xy;
                float2 size = 0.5;
                float2 center = 0;
                
                float inRect = rect(pos, size, center);

                fixed3 color = fixed3(1,1,0) * inRect;
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
