Shader "Custom/GradientShader"
{
    Properties
    {
        [HDR]_ColorTop ("Top Color", Color) = (1,1,1,1)
        [HDR]_ColorBottom ("Bottom Color", Color) = (0,0,1,1)
        _TimeSpeed("TimeSpeed",Range(0.0,1.0)) = 0.1
        _GridSize("GridSize",float) = 1
        _Thickness("Thickness",float) = 0.1
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
            float _TimeSpeed;
            float _GridSize;
            float _Thickness;

            float CalculateGrid(float2 uv, float gridSize, float thickness)
            {
                float2 gridUV = uv * gridSize;
                float2 gridPos = frac(gridUV);
                
                // 创建网格线
                float gridX = step(thickness, gridPos.x);
                float gridY = step(thickness, gridPos.y);
                
                return gridX * gridY;
            }

            float3 GetChangingColor(float time)
            {
                // 使用三角函数创建平滑的颜色循环
                float r = 0.5 + 0.5 * sin(time);
                float g = 0.5 + 0.5 * sin(time + 2.0943951); // 2π/3 相位差
                float b = 0.5 + 0.5 * sin(time + 4.1887902); // 4π/3 相位差
                
                return float3(r, g, b);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float grid = CalculateGrid(i.uv,_GridSize,_Thickness);
                half4 col = lerp(half4(GetChangingColor(_Time.y * _TimeSpeed),1),half4(0,0,0,1),grid);
                
                return col;
            }
            ENDHLSL
        }
    }
    
}