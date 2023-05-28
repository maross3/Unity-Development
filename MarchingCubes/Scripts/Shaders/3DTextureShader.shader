Shader "Custom/3DTextureShader" {
    Properties {
        _MainTex ("Texture3D", 3D) = "" {}
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler3D _MainTex;

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 color = tex3D(_MainTex, float3(IN.uv_MainTex, IN.uv_MainTex.y));
            o.Albedo = color.rgb;
            o.Alpha = color.a;
        }
        ENDCG
    }
}