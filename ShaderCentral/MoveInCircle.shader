Shader "MyShaders/MoveInCircle"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 0, 1)
        _Size("Size", Float) = 0.3
        _Radius("Radius", Float) = 0.5
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
            fixed4 _Color;
            float _Size;
            float _Radius;

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
            
            
            float rect(float2 pt, float2 size, float2 center){
                float2 p = pt - center;
                float2 halfsize = size * 0.5;

                float2 validSpace = step(-halfsize, p) - step(halfsize, p);

                return validSpace.x * validSpace.y;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2( cos(_Time.y), sin(_Time.y)) * _Radius;
                float2 pos = i.position.xy * 2;
                float2 size = _Size;
                
                float3 square = _Color * rect(pos, size,center);
                
                return fixed4(square, 1);
            }
            ENDCG
        }
    }
}
