Shader "MyShaders/Wood"
{
    Properties
    {
        _PaleColor("Pale Color", Color) = (0.733, 0.565, 0.365, 1)
        _DarkColor("Dark Color", Color) = (0.49, 0.286, 0.043, 1)
        _Frequency("Frequency", Float) = 2.0
        _NoiseScale("Noise Scale", Float) = 6.0
        _RingScale("Ring Scale", Float) = 0.6
        _Contrast("Contrast", Float) = 4.0
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

            fixed4 _PaleColor;
            fixed4 _DarkColor;
            
            float _Frequency;
            float _NoiseScale;
            float _RingScale;
            float _Contrast;

            float4 frag (v2f i) : COLOR
            {
                
                float3 pos = i.position.xyz * 2;
                float n = snoise(pos);
                
                float ring = frac(_Frequency * pos.z + _NoiseScale * n);
                ring *= _Contrast * (1 - ring);
                
                float delta = pow(ring, _RingScale) + n;
                
                fixed3 color = lerp(_DarkColor, _PaleColor, delta);

                return fixed4( color, 1.0 );
            }
            ENDCG
        }
    }
}
