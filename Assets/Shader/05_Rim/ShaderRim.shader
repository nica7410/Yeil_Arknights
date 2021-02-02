Shader "Custom/ShaderRim"
{
    Properties
    {
        _MainTex("Albedo (MainTex)",2D)= "white"{}
        _RimPower("RimPower", Range(0,50)) = 1
        _BumpMap("BumpMap",2D) = "white" {}
        _RimColor("Albedo(RimColor)", Color) = (0,0,0,1)
    }
    SubShader
    {
       
        CGPROGRAM
    
        #pragma surface surf Lambert noabient

        sampler2D _MainTex;
        sampler2D _BumpMap;
        half _RimPower;
        half4 _RimColor;
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
        };
        void surf (Input IN, inout SurfaceOutput o)
        {
            float4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            float3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Normal = n.rgb;
       
            float rim = saturate(dot(o.Normal, IN.viewDir));
            o.Emission = pow(1-rim, _RimPower) * _RimColor.rgb;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
