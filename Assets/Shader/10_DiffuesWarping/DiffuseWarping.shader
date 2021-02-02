Shader "Custom/DiffuseWarping"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _RampTex("RampTex", 2D) = "ramp" {}
    }
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Warping Lambert noambient
        sampler2D _MainTex;
        sampler2D _RampTex;
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_RampTex;
        };
        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
        }
        float4 LightingWarping(SurfaceOutput s, float3 lightDir, float atten)
        {
            float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;
            float4 ramp = tex2D(_RampTex, float2(ndotl, 0.5));
            return ramp;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
