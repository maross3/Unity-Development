 Shader "MyShaders/Lava"
{
    Properties
    {
        _Scale("Scale", Range(0.1, 3)) = 1
        _MainTex("Main Texture", 2D) = "white" {}
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
            #include "noiseSimplex.cginc"
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv: TEXCOORD0;
                float4 noise: TEXCOORD1;
                float4 screenPos: TEXCOORD2;
            };

            sampler2D _MainTex;
            float _Scale;

            float random( float3 pt, float seed ){
                float3 scale = float3(12.9898, 78.233, 151.7182);
                return frac(sin( dot( pt + seed, scale )) * 43758.5453 + seed ) ;
            }
            
            v2f vert (appdata_base v)
            {
                v2f newV;
                
                newV.noise = 0;
                newV.noise.x = 10 * -0.1 * turbulence(0.5 * v.normal + _Time.y / 4);
                const float3 size = 100;

                const float pNoise = _Scale * 0.5 * pnoise(0.05 * v.vertex + _Time.y, size);

                // displace the vertex
                const float displacement = pNoise - _Scale * newV.noise.x;
                const float3 newPos = v.vertex + v.normal * displacement;

                newV.pos = UnityObjectToClipPos(newPos);
                newV.uv = v.texcoord;

                return newV;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 fragCoord = float3(i.screenPos.xy * _ScreenParams, 0);
                
                // random frag offset
                float rand = 0.01 * random(fragCoord, 0);

                // vertical lookup using noise + offset
                float2 uv = float2(0, 1.3 * i.noise.x + rand);
                
                fixed3 color = tex2D(_MainTex, uv).rgb;
                
                return fixed4( color, 1 );
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}

