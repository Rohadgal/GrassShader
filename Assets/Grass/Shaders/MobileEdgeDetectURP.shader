Shader "Custom/MobileEdgeDetectURP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float3 center = tex2D(_MainTex, i.uv).rgb;

                // Sample neighbors
                float3 up    = tex2D(_MainTex, i.uv + float2(0, _MainTex_TexelSize.y)).rgb;
                float3 down  = tex2D(_MainTex, i.uv - float2(0, _MainTex_TexelSize.y)).rgb;
                float3 left  = tex2D(_MainTex, i.uv - float2(_MainTex_TexelSize.x, 0)).rgb;
                float3 right = tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.x, 0)).rgb;

                // Edge detection
                float edge = length(right - left) + length(up - down);
                float threshold = 0.1;

                if (edge > threshold)
                {
                    // Check if any sampled pixel is close to black
                    float blackThreshold = 0.05; // tweak this
                    bool isEdgeBlack = all(right < blackThreshold) ||
                                       all(left  < blackThreshold) ||
                                       all(up    < blackThreshold) ||
                                       all(down  < blackThreshold) ||
                                       all(center < blackThreshold);

                    float3 darkBlue = float3(0.0, 0.0, 1.0);
                    float3 lightBlue = float3(0.3, 0.3, 1.0);

                    float3 color = isEdgeBlack ? darkBlue : lightBlue;
                    return float4(color, 1.0);
                }

                return float4(1.0, 1.0, 1.0, 1.0); // white background
            }
            ENDHLSL
        }
    }
}
