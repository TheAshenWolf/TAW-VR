Shader "Drawing/DrawingUnlit"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        [PerRendererData] _Splat ("Splat", 2D) = "black" {}
        [PerRendererData] _Coordinate("Coordinate", Vector) = (0,0,0,0)
        [PerRendererData] _DrawColor("Draw Color", Color) = (1,0,0,0)
        [PerRendererData] _IsDrawing("Is drawing", float) = 0
        [PerRendererData] _BrushSize("Brush size", float) = 0.999
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
                float2 uv1 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _Splat;
            float4 _MainTex_ST, _Splat_ST;
            fixed4 _Coordinate, _DrawColor;
            int _IsDrawing;
            float _BrushSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.uv1, _Splat);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 baseColor = tex2D(_MainTex, i.uv);
                fixed4 splatColor = tex2D(_Splat, i.uv1);
                float draw = (saturate(1 - distance(i.uv1, _Coordinate.xy)) > _BrushSize) * _IsDrawing;
                fixed4 drawColor = _DrawColor * (draw * 1);

                //fixed4 finalColor =  ((drawColor+splatColor) > fixed4(0,0,0,0)) ? drawColor+splatColor : baseColor+splatColor+drawColor;
                fixed4 finalColor = splatColor.a == 0 && drawColor.a == 0 ? baseColor+splatColor+drawColor : splatColor.a == 0 ? splatColor+drawColor : splatColor;//splatColor+drawColor;

                //  return draw;
                return finalColor;
                // return saturate(baseColor+splatColor+drawColor);
            }
            ENDCG
        }
    }
}