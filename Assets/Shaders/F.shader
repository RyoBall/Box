Shader "Custom/Picture"
{
    Properties
    {
        _MainTex("MainTex",2D) = "white" {}
        _TimeSpeed("YimeSpeed",Range(0.0,1.0)) = 0.1
    }
    SubShader
    {
        Tags { 
            "RenderType"="TransParent"
            "Queue"="AlphaTest"
            "LightMode" = "UniversalForward"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#pragma shader_feature _ALPHATEST_ON

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
            
            float _Tilt;
            float _TimeSpeed;
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x -= _Time.x * 0.5;
                half4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                half a = albedo.a;
                
                //float fade = (sin(_Time.y * _TimeSpeed) + 1.0) * 0.5;  // 范围 [0, 1]
                //albedo.a *= fade;
                
                // 如果需要硬边缘，可以保留clip但调整阈值
                clip(albedo.a - 0.01);

                return albedo;
            }
            ENDHLSL
        }
    }
    
}