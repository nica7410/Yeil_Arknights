Shader "Study/02_Texture_Fire"
{
	Properties
	{
		//_MainTex는 기본으로 가져야 하는 것 같다.
		_MainTex("Albedo RGB", 2D) = "white" {}
		_MainTex2("Albedo RGB", 2D) = "white" {}
		_NoiseTex("Albedo RGB", 2D) = "white" {}
	}
		SubShader
	{

		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		CGPROGRAM
		#pragma surface surf Standard alpha:fade

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _NoiseTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//half4 noise = tex2D(_NoiseTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y -= _Time.y)) * 0.2;
			//half4 noise = tex2D(_NoiseTex, IN.uv_MainTex) * 0.2;
			//half4 TexColor = tex2D(_MainTex, IN.uv_MainTex);

			//half2 noiseUV = IN.uv_MainTex * noise.r;

			//half4 TexColor2 = tex2D(_MainTex2, noiseUV);
			//half4 TexColor2 = tex2D(_MainTex2, float2(IN.uv_MainTex.x, IN.uv_MainTex.y -= _Time.y) + noise);

			//o.Emission = TexColor.rgb * TexColor2.rgb * 2;
			//o.Alpha = TexColor.a * TexColor2.a;

			//_Time(t/20, t, t*2, t*3)
			//_SinTIme(t/8, t/4, t/2, t)
			//_CosTime(t/8, t/4, t/2, t)
			//unity_DeltaTime(dt, 1/dt, smoothDt, 1/smoothDt)	
	
			half4 TexColor = tex2D(_MainTex, IN.uv_MainTex);
			half4 TexColor2 = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y -= _Time.y));
			o.Emission = TexColor * TexColor2 * 2;
			o.Alpha = TexColor.a * TexColor2.a;


		}



		ENDCG
	}
}