Shader "Study/03_Masking"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_MainTex2("Albedo (R)", 2D) = "white" {}
		_MainTex3("Albedo (G)", 2D) = "white" {}
		_MainTex4("Albedo (B)", 2D) = "white" {}

		_BumpMap("Albedo (BumpMap)", 2D) = "white"{}

		_MaskTex("Mask Tex", 2D) = "white" {}

		_Metallic("Metalllic", Range(0,1)) = 1
		_Smoothness("Smoothness",Range(0,1)) = 1

	}

		SubShader
	{
		CGPROGRAM
		#pragma surface surf Standard

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;
		sampler2D _MainTex4;

		sampler2D _BumpMap;

		sampler2D _MaskTex;

		half _Metallic;
		half _Smoothness;


		struct Input
		{
			float2 uv_MainTex;	//대문자
			float2 uv_BumpMap;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//o.Emission = c + m; //검은 부분 x
			//o.Emission = c * m; //검은 부분 o

			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			half4 m = tex2D(_MaskTex, IN.uv_MainTex);


			half4 c2 = tex2D(_MainTex2, IN.uv_MainTex);
			half4 c3 = tex2D(_MainTex3, IN.uv_MainTex);
			half4 c4 = tex2D(_MainTex4, IN.uv_MainTex);
			
			half3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			//lerp는 3자리수 자료형 반환
			half3 c2r = lerp(c2.rgb, m.rgb, 1 - m.r);
			half3 c3g = lerp(c3.rgb, c2r.rgb, 1 - m.g);
			half3 c4b = lerp(c4.rgb, c3g.rgb, 1 - m.b);


		
			//Textur Splatting

			//o.Emission = (c2r*c3g*c4b).rgb;
			//o.Emission = (c2r+c3g+c4b).rgb;
			//o.Emission = (c4b).rgb;

		/*	o.Albedo =
				c2.rgb * m.r +
				c3.rgb * m.g +
				c4.rgb * m.b +
				c * (1 - (m.r + m.g + m.b));*/
			o.Albedo = (c4b).rgb + c * (1 - (m.r + m.g + m.b));

			o.Metallic = _Metallic* m.g;
			o.Smoothness = _Smoothness * m.g;

			o.Normal = n;
		}

		ENDCG
	}
}