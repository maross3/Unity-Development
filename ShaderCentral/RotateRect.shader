Shader "MyShaders/RotateRect"
{
    Properties
    {
        _Color("Square Color", Color) = (1.0,1.0,0,1.0)
        _Radius("Radius", Float) = 0.5
        _Size("Size", Float) = 0.3
        _Anchor("Anchor", Vector) = (0.15, 0.15, 0, 0)
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
                float4 position : TEXCOORD1;
            };
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.position = v.vertex;
                return o;
            }

            fixed4 _Color;
            float _Radius;
            float _Size;
            float4 _Anchor;
            
            float rect(float2 pt, float2 size, float2 center){
                float2 p = pt - center;
                float2 halfsize = size * 0.5;

                float2 validSpace = step(-halfsize, p) - step(halfsize, p);

                return validSpace.x * validSpace.y;
            }

            float rect(float2 pt, float2 anchor, float2 size, float2 center){
                float2 p = pt - center;
                float2 halfsize = size * 0.5;

                float2 validSpace = step(-halfsize - anchor, p) - step(halfsize - anchor, p);

                return validSpace.x * validSpace.y;
            }

            float2x2 getRotationMatrix(float theta)
            {
                float sinTheta = sin(theta);
                float cosTheta = cos(theta);

                return float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
            }

            float2x2 getScaleMatrix(float scale)
            {
                return float2x2(scale,0,0,scale);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(cos(_Time.y), sin(_Time.y)) * _Radius;
                float2 pos = i.position * 2.0;
                float2 size = _Size;
                
                float2x2 mtrx = getRotationMatrix(_Time.y);
                // multiply matrix by pos - center, add back to preserve center pos
                float2x2 mtrxScale = getScaleMatrix((sin(_Time.y) + 1) / 3 + 0.5);
                mtrx = mul(mtrx, mtrxScale);
                
                float2 newPoint = mul(mtrx, pos - center) + center;
                
                float3 color = _Color * rect(newPoint, _Anchor.xy, size, center);
                
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
