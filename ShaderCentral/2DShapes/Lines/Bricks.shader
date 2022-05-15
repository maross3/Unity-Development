Shader "MyShaders/Bricks"
{
    Properties
    {
        _BrickColor("Brick Color", Color) = (0.9, 0.3, 0.4, 1)
        _MortarColor("Mortar Color", Color) = (0.7, 0.7, 0.7, 1)
        _TileCount("Tile Count", Int) = 10
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

            fixed4 _BrickColor;
            fixed4 _MortarColor;
            int _TileCount;
            
            float brick(float2 pos, float mortarHeight, float edgeThickness){
                float halfMortarHeight = mortarHeight / 2;
                
                // bottom
                float result = 1 - smoothstep(halfMortarHeight, halfMortarHeight + edgeThickness, pos.y);
                
                // top
                result += smoothstep(1 - halfMortarHeight - edgeThickness, 1 - halfMortarHeight, pos.y);

                // middle
                result += smoothstep(0.5 - halfMortarHeight - edgeThickness, 0.5 - halfMortarHeight, pos.y) -
                            smoothstep(0.5 + halfMortarHeight, 0.5 + halfMortarHeight + edgeThickness, pos.y);

                if(pos.y > 0.5) pos.x = frac(pos.x + 0.5);
                result += smoothstep(-halfMortarHeight - edgeThickness, -halfMortarHeight, pos.x) -
                            smoothstep(halfMortarHeight, halfMortarHeight + edgeThickness, pos.x) +
                                smoothstep(1 - halfMortarHeight - edgeThickness, 1 - halfMortarHeight, pos.x);
                
                // saturate to range [0, 1]
                return saturate(result);
            }
            
            fixed4 frag (v2f_img i) : SV_Target
            {
                float2 uv = frac(i.uv * _TileCount);
                
                fixed3 color = lerp(_BrickColor.rgb, _MortarColor.rgb, brick(uv, 0.05, 0.001)); 
                
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
