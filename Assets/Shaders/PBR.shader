Shader "Unlit/PBR_WithShadow"
{
    Properties
    {
        _BaseColor("Color",Color) = (1,1,1,1)
        _Metallic("Metallic",Range(0,1)) = 0.5
        _Roughness("Roughness",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"
            "RenderPipeline"="UniversalRenderPipeline"
            "LightMode" = "UniversalForward"}

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float4 shadowCoord : TEXCOORD3; // ✅ 阴影坐标
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Metallic;
                float _Roughness;
            CBUFFER_END
            
            float3 FresnelSchlick(float cosTheta, float3 F0)
            {
                return F0 + (1.0 - F0) * pow(1.0 - cosTheta, 5.0);
            }

            float GeometrySchlickGGX(float Ndot)
            {
                float a = pow((_Roughness + 1) / 2, 2);
                float k = a / 2;
                float denom = (1.0 - k) * Ndot + k;
                return Ndot / denom;
            }

            float GeometrySmith(float NdotV, float NdotL)
            {
                float ggx1 = GeometrySchlickGGX(NdotV);
                float ggx2 = GeometrySchlickGGX(NdotL);
                return ggx1 * ggx2;
            }

            float DistributionGGX(float NdotH)
            {
                float a2 = pow(_Roughness, 2);
                float NdotH2 = pow(NdotH, 2);
                float denom = NdotH2 * (a2 - 1.0) + 1.0;
                denom = 3.1415926 * denom * denom;
                return a2 / denom;
            }

            float3 BRDF(float3 N, float3 L, float3 V, float3 albedo)
            {
                float NdotL = max(dot(N, L), 0.0);
                float NdotV = max(dot(N, V), 0.0);
                float3 H = normalize(V + L);
                float NdotH = max(dot(N, H), 0.0);

                float3 F0 = lerp(float3(0.04, 0.04, 0.04), albedo, _Metallic);
                float D = DistributionGGX(NdotH);
                float G = GeometrySmith(NdotV, NdotL);
                float3 F = FresnelSchlick(max(dot(H, V), 0.0), F0);

                float3 numerator = D * G * F;
                float denominator = 4.0 * NdotV * NdotL + 0.001;
                float3 specular = numerator / denominator;

                float3 kS = F;
                float3 kD = (1.0 - kS) * (1.0 - _Metallic);
                float3 diffuse = kD * albedo / 3.1415926;

                return (diffuse + specular) * NdotL;
            }

            //====================
            // 顶点
            //====================
            Varyings vert (Attributes v)
            {
                Varyings o;
                VertexPositionInputs posInputs = GetVertexPositionInputs(v.vertex);
                o.positionCS = posInputs.positionCS;
                o.positionWS = posInputs.positionWS;
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.viewDirWS = GetWorldSpaceNormalizeViewDir(o.positionWS);

                // ✅ 计算主光阴影坐标
                o.shadowCoord = TransformWorldToShadowCoord(o.positionWS);
                return o;
            }

            //====================
            // 片元
            //====================
            half4 frag (Varyings i) : SV_Target
            {
                Light light = GetMainLight(i.shadowCoord);

                float3 N = normalize(i.normalWS);
                float3 L = normalize(light.direction);
                float3 V = normalize(i.viewDirWS);
                float3 albedo = _BaseColor.rgb;

                // ✅ 采样阴影贴图
                float shadow = MainLightRealtimeShadow(i.shadowCoord);

                float3 brdf = BRDF(N, L, V, albedo);
                float3 ambient = SampleSH(N) * albedo * 0.2;

                float3 finalColor = (brdf * light.color * shadow * light.distanceAttenuation) + ambient;
                finalColor = pow(saturate(finalColor), 1.0 / 2.2);
                
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
