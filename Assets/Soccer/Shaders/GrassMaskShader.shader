Shader "Custom/GrassMaskShader"
{
    Properties
    {
        _GrassTex ("Grass Texture", 2D) = "white" {}
        _GrassMask ("Grass Mask", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _WindSpeed ("Wind Speed", Float) = 1.0
        _WindStrength ("Wind Strength", Float) = 0.05
        _Tiling ("Tiling", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _GrassTex;
            sampler2D _GrassMask;
            float4 _MainColor;
            float _WindSpeed;
            float _WindStrength;
            float4 _Tiling;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv * _Tiling.xy + _Tiling.zw;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float time = _Time.y;
                float wind = sin((IN.uv.x + time * _WindSpeed) * 6.283) * _WindStrength;

                float2 windedUV = IN.uv + float2(0, wind);

                float4 grass = tex2D(_GrassTex, windedUV);
                float mask = tex2D(_GrassMask, IN.uv).r;

                float4 finalColor = grass * _MainColor;
                finalColor.a *= mask; // Apply mask to alpha

                return finalColor;
            }
            ENDHLSL
        }
    }
}
