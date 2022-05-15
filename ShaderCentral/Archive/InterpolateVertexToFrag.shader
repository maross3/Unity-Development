Shader "MyShaders/InterpolateVertexToFrag"
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

            // vertex to fragment
            struct v2f
            {
                float4 vertex: SV_POSITION; // model space to clip space (system value position)
                float4 position: TEXCOORD1; // TEXCOORDn for custom values
                float2 uv: TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.position = v.vertex;
                o.uv = v.texcoord;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // saturate clamps negative to 0
                fixed3 color = saturate(i.position * 2);
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
