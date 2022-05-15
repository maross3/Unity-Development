Shader "MyShaders/Line"
{
    Properties
    {
        _Color("Color", Color) = (1.0,1.0,1.0,1.0)
        _LineWidth("Line Width", Float) = 0.01
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
                float2 uv: TEXCOORD0;
                float4 position: TEXCOORD1;
                float4 screenPos: TEXCOORD2;
            };
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.position = v.vertex;
                o.uv = v.texcoord;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }
            
            fixed4 _Color;
            float _LineWidth;

            float online(float start, float end, float lineWidth, float edgeThickness)
            {
                float halfWidth = lineWidth * 0.5;
                return smoothstep(start - halfWidth - edgeThickness, start - halfWidth, end) -
                    smoothstep(start + halfWidth, start + halfWidth + edgeThickness, end);
            }

            // returns s
            float getDelta( float x ){
            	return (sin(x) + 1.0)/2.0;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
            	float2 uvLocal = i.uv;
                float2 uvScreen = i.screenPos.xy / i.screenPos.w;
                
                // uv line (0.0, 0.0), (1.0, 1.0)
                // can use local or screen uv
            	fixed3 uvLine = lerp(fixed3(0, 0, 0), _Color.rgb,
            	    online(uvScreen.x, uvScreen.y, _LineWidth, _LineWidth * 0.1));

                // uv sin
                fixed3 uvSin = _Color * online(uvScreen.y, lerp(0.3, 0.7, getDelta(uvScreen.x * UNITY_TWO_PI)), _LineWidth, 0.002);
                
                float2 pos = i.position.xy * 2;
                
                // i pos sin line
                fixed3 sinLine = _Color * online(pos.y, lerp(-0.4, 0.4, getDelta(pos.x * UNITY_PI)), _LineWidth, 0.002);

                
                return fixed4(uvSin, 1.0);
            }
            ENDCG
        }
    }
}
