Shader "Hidden/GrassBrush"
{
    Properties
    {
        _BrushTex ("Brush Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _BrushUV ("Brush UV", Vector) = (0.5, 0.5, 0, 0)
        _BrushSize ("Brush Size", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _BrushTex;
            float4 _BrushUV;
            float4 _Color;
            float _BrushSize;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 delta = i.uv - _BrushUV.xy;
                float dist = length(delta);
                float2 brushUV = delta / _BrushSize + 0.5;
                float brush = tex2D(_BrushTex, brushUV).r;
                return _Color * brush;
            }
            ENDCG
        }
    }
}
