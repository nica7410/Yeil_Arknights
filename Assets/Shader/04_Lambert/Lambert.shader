Shader "Custom/Lamberts"
{
    Properties
    {
        _MainTex("Aldedo (MainTex)",2D) = "white"{}
        _BumpMap("BumpMap",2D) = "bump"{}
        _LambertPower("LambertPower", Range(1,10))=3
    }
    SubShader
    {
       
        CGPROGRAM
        
        #pragma surface surf Example noambient
       
        sampler2D _MainTex;
        sampler2D _BumpMap;
        float _LambertPower;
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            /*
                half3 Albedo;   //기본색상
                half3 Normal;   //
                half3 Emission;
                half Specular;
                half Gloss; 
                half Alpha;
            */
            //o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
            float4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Alpha = c.a;
        }
        float4 LightingExample(SurfaceOutput s, float3 lightDir, float atten)
        {
            //float ndotl = dot(s.Normal, lightDir);
            float4 ndotl = saturate(dot(s.Normal, lightDir)*0.5+0.5);
            ndotl = pow(ndotl, _LambertPower);
            //float ndotl =  max(0,dot(s.Normal, lightDir));
            float4 final;
            final.rgb = ndotl.rgb * s.Albedo * _LightColor0.rgb *atten;
            final.a = s.Alpha;
            return final;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
