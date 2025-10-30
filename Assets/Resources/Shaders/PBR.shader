Shader "Unlit/PBR_WithShadow"
{
    Properties
    {
        [HDR]_BaseColor("Color",Color) = (1,1,1,1)
        _Metallic("Metallic",Range(0,1)) = 0.5
        _Roughness("Roughness",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"

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
                LIGHTING_COORDS(3, 4)
            };

            float4 _BaseColor;
            float _Metallic;
            float _Roughness;
            
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
            
            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = UnityObjectToClipPos(v.vertex);
                o.positionWS = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normalWS = UnityObjectToWorldNormal(v.normal);
                o.viewDirWS = normalize(_WorldSpaceCameraPos.xyz - o.positionWS);
                
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float3 N = normalize(i.normalWS);
                float3 L = normalize(_WorldSpaceLightPos0.xyz);
                float3 V = normalize(i.viewDirWS);
                float3 albedo = _BaseColor.rgb;
                
                float shadow = LIGHT_ATTENUATION(i);

                float3 brdf = BRDF(N, L, V, albedo);
                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo * 0.1;

                float3 finalColor = (brdf * _LightColor0.rgb * shadow) + ambient;
                finalColor = pow(saturate(finalColor), 1.0 / 2.2);
                
                return half4(finalColor, 1.0);
            }
            ENDCG
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = UnityObjectToClipPos(v.positionOS);
                return o;
            }

            half4 frag(Varyings i) : SV_TARGET
            {
                return 0;
            }
            ENDCG
        }
    }
}