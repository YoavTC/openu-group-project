Shader "Unlit/Sprite Replace Colour Shader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _TargetColor ("Target Color", Color) = (1,1,1,1)
        _ReplaceColor ("Replace Color", Color) = (0,0,0,1)
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _TargetColor;
            float4 _ReplaceColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Compare each component of the color individually for an exact match
                if (col.r == _TargetColor.r && col.g == _TargetColor.g && col.b == _TargetColor.b && col.a == _TargetColor.a)
                {
                    col = _ReplaceColor;
                }

                return col;
            }
            ENDCG
        }
    }
}
