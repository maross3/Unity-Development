Shader "MyShaders/UVBlendX"
{
    Properties
    {
        _ColorA("Color A", Color) = (1,0,0,1)
        _ColorB("Color B", Color) = (0,0,1,1) 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _ColorA;
            fixed4 _ColorB;

            fixed4 frag (v2f_img i) : SV_Target
            {
                float delta = i.uv.x;
                fixed3 color = lerp(_ColorA, _ColorB, delta);
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
