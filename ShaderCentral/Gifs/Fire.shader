Shader "MyShaders/Fire"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            fixed4 frag (v2f_img i) : SV_Target
            {
                float2 uv;
                float2 noise = float2(0,0);

                // y noise filter using uv map
                uv = float2(i.uv.x * 0.7 - 0.01, frac(i.uv.y - _Time.y * 0.27));
                noise.y = (tex2D(_MainTex, uv).a - 0.5) * 2.0;
                
                uv = float2(i.uv.x * 0.45 + 0.033, frac(i.uv.y * 1.9 - _Time.y * 0.61));
                noise.y += (tex2D(_MainTex, uv).a - 0.5) * 2.0;
                
                uv = float2(i.uv.x * 0.8 - 0.02, frac(i.uv.y * 2.5 - _Time.y * 0.51));
                noise.y += (tex2D(_MainTex, uv).a - 0.5) * 2.0;
                
                noise = clamp(noise, -0.8, 0.8);

                // add noise to blur
                float blur = (1.0 - i.uv.x) * 0.3;
                noise = (noise * blur) + i.uv;
                
                // tex2D(Sampler2D samp, float2 s), lookup, lookup coords 
                fixed4 color = tex2D(_MainTex, noise);
                
                color = fixed4(color.r * 2.0, color.g * 0.9, (color.g / color.r) * 0.2, 1.0);
                noise = clamp(noise, 0.05, 1.0);
                
                color.a = tex2D(_MainTex, noise).b * 2.0;
                color.a = color.a * tex2D(_MainTex, i.uv).b;

                return color;
            }
            ENDCG
        }
    }
}
