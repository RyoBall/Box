Shader "Custom/GradientShader"
{
    Properties
    {
        [HDR]_ColorTop ("Top Color", Color) = (1,1,1,1)
        [HDR]_ColorBottom ("Bottom Color", Color) = (0,0,1,1)
        _Tilt ("Tilt", Range(0.0, 1.0)) = 0
        _TimeSpeed("TimeSpeed",Range(0.0,1.0)) = 0.1
    }

    SubShader
    {
        Tags {
            "RenderType" = "Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            fixed4 _ColorTop;
            fixed4 _ColorBottom;
            float _Tilt;
            float _TimeSpeed;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float x = fmod(i.uv.x + _Time.y * _TimeSpeed, 1.0);
                float y = fmod(i.uv.y + _Time.y * _TimeSpeed, 1.0);
                float lerpValue = lerp(x, y, _Tilt);

                // 在内置渲染管线中，颜色通常不需要手动进行Gamma/Linear转换
                // Unity会自动处理，所以直接使用lerp
                fixed4 result = lerp(_ColorBottom, _ColorTop, lerpValue);
                return result;
            }
            ENDCG
        }
    }
    
    Fallback "Diffuse"
}