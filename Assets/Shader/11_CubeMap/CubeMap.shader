Shader "Custom/CubeMap"
{
    Properties
    {
        _MainTex("Albedo", 2D) = "white" {}
        _BumpMap("BumpMap",2D) = "white" {}
        _CubeMap ("CubeMap", Cube) ="" {}
        _MaskMap("MaskMap", 2D) = "white" {}
        //_MaskAlpha("MaskAlpha",Range(0,1)) = 1
    }
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert
        sampler2D _MainTex;
        sampler2D _BumpMap;
        samplerCUBE _CubeMap;
        sampler2D _MaskMap;

        half _MaskAlpha;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_MaskMap;
            float3 worldRefl;

            INTERNAL_DATA
        };
        void surf(Input IN, inout SurfaceOutput o)
        {
            half3 c = tex2D(_MainTex, IN.uv_MainTex);
            half3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            half4 cube = texCUBE(_CubeMap, WorldReflectionVector(IN, o.Normal));
            float4 m = tex2D(_MaskMap, IN.uv_MaskMap);
            
            o.Normal = n.rgb;
            o.Albedo = c.rgb * (1- m.r);
            o.Emission = cube.rgb * m.r;
            //o.Alpha = c.a;


        }
        ENDCG
    }
}
