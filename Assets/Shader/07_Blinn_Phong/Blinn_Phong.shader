Shader "Custom/Blinn_Phong"
{
    Properties
    {
        _MainTex("Albedo (MainTex)",2D) = "white"{}
        _BumpMap("BumpMap",2D) = "white" {}
    }
    SubShader
    {
      
        CGPROGRAM
    
        #pragma surface surf Phong noambient

      
        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

     
        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            half3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Normal = n.rgb;
        }

        float4 LightingPhong(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            //Half Vector
            half3 H = normalize(lightDir + viewDir);
            half spec = dot(H.rgb, s.Normal.rgb);
            spec = pow(spec, 100);
           
            return spec;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
