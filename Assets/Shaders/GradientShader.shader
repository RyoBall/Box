Shader "Custom/GradientShader"
{
    Properties
    {
        [HDR]_ColorTop ("Top Color", Color) = (1,1,1,1)
        [HDR]_ColorBottom ("Bottom Color", Color) = (0,0,1,1)
        _Tilt ("Tilt", Range(0.0, 1.0)) = 0
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalRenderPipeline"
            "LightMode" = "UniversalForward"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

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

            float4 _ColorTop;
            float4 _ColorBottom;
            float _Tilt;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float x = (i.uv.x + _Time.y * 0.1) % 1.0f;
                float y = (i.uv.y + _Time.y * 0.1) % 1.0f;
                float lerpValue = lerp(x,y,_Tilt);
                
                half3 bottomSRGB = LinearToSRGB(_ColorBottom.rgb);
                half3 topSRGB = LinearToSRGB(_ColorTop.rgb);
                half3 resultSRGB = lerp(bottomSRGB, topSRGB, lerpValue);
                half4 col = half4(SRGBToLinear(resultSRGB), 1.0);
                
                return col;
            }
            ENDHLSL
        }
    }
    
}