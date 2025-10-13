Shader "Unlit/EdgeShader"
{
    Properties
    {
        _OutLineLength("OutLineLength",Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "UniversalForward"}

        Pass
        {
            ZWrite On
            Cull Front
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float3 normalWS : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };
            
            float _OutLineLength;

            Varyings vert (Attributes v)
            {
                Varyings o;
                //o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.positionCS = TransformObjectToHClip(v.vertex);
                o.positionCS.xy += TransformWorldToHClipDir(TransformObjectToWorldDir(v.normal)).xy * o.positionCS.w * _OutLineLength;
                //o.positionCS = TransformObjectToHClip(v.vertex + normalize(v.normal) * _OutLineLength);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                return half4(0,0,0,1);
            }
            ENDHLSL
        }
    }
}
