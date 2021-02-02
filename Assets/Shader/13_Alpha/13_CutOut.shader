﻿Shader "Custom/13_AlphaBleending"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Cutoff("Alpha cutoff",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags
        { 
            "RenderType"="TransparentCutout"
            "Queue" = "AlphaTest"
        }
        cull off
        zwrite off

        CGPROGRAM
        #pragma surface surf Lambert alphaTest:fade

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
FallBack "Transparent/Cutout/Diffuse"
}
