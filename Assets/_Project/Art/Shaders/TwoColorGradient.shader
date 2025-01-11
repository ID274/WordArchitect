Shader "Custom/TwoColorGradient"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 0, 1, 1)
        _Direction ("Direction (0=Vertical, 1=Horizontal, 2=Diagonal)", Range(0, 2)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            float4 _Color1;
            float4 _Color2;
            float _Direction;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);

                float gradient = 0.0;
                if (_Direction == 0) // Vertical
                    gradient = i.uv.y;
                else if (_Direction == 1) // Horizontal
                    gradient = i.uv.x;
                else if (_Direction == 2) // Diagonal
                    gradient = (i.uv.x + i.uv.y) * 0.5;

                float4 gradientColor = lerp(_Color1, _Color2, gradient);
                return texColor * gradientColor; // Combine texture and gradient
            }
            ENDCG
        }
    }
}
