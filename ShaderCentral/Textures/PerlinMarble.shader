Shader "MyShaders/PerlinMarble"
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
            #include "noiseSimplex.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 position: TEXCOORD1;
            };
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.position = v.vertex;
                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                float2 pos = i.position.xy * 2;
                const float scale = 800;
                pos *= scale;
                
                fixed3 color;
                float noise;
                
                bool marbling = true;
                
                if (marbling){

                    // 8 iterations of fractal noise * scale
                    float distortion = perlin(pos.x, pos.y) * scale / 0.8;
                    const float greenNoise = pos.x + distortion;
                    const float blueNoise = pos.y + distortion;
                    
                    distortion = perlin(greenNoise, blueNoise) * scale;

                    noise = perlin(pos.x + distortion, pos.y + distortion);
                    
                    color = fixed3(0.6 * (fixed3(2, 2, 2) * noise -
                        fixed3(noise * 0.1, noise * 0.2 - sin(greenNoise / 30) * 0.5, noise * 0.3 + sin(blueNoise / 40) * 0.2)));

                }else{
                    noise = perlin(pos.x, pos.y);
                    color = fixed3(1, 1, 1) * noise;
                }

                return fixed4(color, 1);
            }
            ENDCG
        }
    }
}

