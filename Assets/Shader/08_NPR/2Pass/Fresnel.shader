Shader "Study/Fresnel"
{
	Properties
	{
		_MainTex("Albedo (RGB)",2D) = "white"{}
	}
		SubShader
	{
		CGPROGRAM
		#pragma surface surf Toon Lambert noambient
		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		float4 LightingToon(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
		{
			float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;
			if (ndotl > 0.7) ndotl = 1;
			else if (ndotl > 0.4) ndotl = 0.5;
			else ndotl = 0.1;
			
			float rim = abs(dot(s.Normal, viewDir));
			if (rim > 0.3) rim = 1;
			else rim = -1;
			float4 final;
			final.rgb = s.Albedo * ndotl * rim * _LightColor0.rgb;
			final.a = s.Alpha;
			return final;
		}
		ENDCG
		
	}
}