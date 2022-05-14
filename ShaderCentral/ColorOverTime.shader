Shader "MyShaders/ColorOverTime"
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
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 frag (v2f_img i) : SV_Target
            {
                // the float4 _Time's y property = time
                
                fixed3 color = fixed3((sin(_Time.y) + 1) / 2, 0, (cos(_Time.y) + 1) / 2);
                return fixed4(color ,1);
            }
            ENDCG
        }
    }
}
