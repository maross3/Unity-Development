Shader "MyShaders/Circle"
{
    Properties
    {
        _Color("Color", Color) = (1.0,1.0,0,1.0)
        _Radius("Radius", Float) = 0.3
        _SoftEdge("Soften Edges", Float) = 0.005
    }
    SubShaderShader "MyShaders/Circle"
{
    Properties
    {
        _Color("Color", Color) = (1.0,1.0,0,1.0)
        _Radius("Radius", Float) = 0.3
        _SoftEdge("Soften Edges", Float) = 0.005
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

            float _SoftEdge;

            // draw circle
            float circle(float2 pos, float2 center, float radius)
            {
                float2 pt = pos - center;
                return 1.0 - step(radius, length(pt));
            }

            // blurred edges
            float circle(float2 pos, float2 center, float radius, bool soften)
            {
                float2 pt = pos - center;
                float edge = (soften) ? radius * _SoftEdge : 0;
                return 1.0 - smoothstep(radius - edge, radius + edge, length(pt));
            }

            // outline
            float circle(float2 pos, float2 center, float radius, float lineWidth)
            {
                float2 pt = pos - center;
                float lineLength = length(pt);
                float halfLineWidth = lineWidth / 2.0;
                return step(radius - halfLineWidth, lineLength) - step(radius + halfLineWidth, lineLength);
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
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 position : TEXCOORD1;
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
           
            fixed4 _Color;
            float _Radius;
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos = i.position * 2;

                float rad = _Radius * 0.2;
                fixed3 color = _Color * circle(pos, float2(0,0), _Radius, rad, 0.03);
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
