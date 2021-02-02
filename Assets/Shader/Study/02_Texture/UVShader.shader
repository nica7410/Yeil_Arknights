Shader "Study/01_Texture_UV"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white"{}
		_Speed("AnimeSpeed", Range(0,10)) = 1
	}

		SubShader
	{
		CGPROGRAM
		#pragma surface surf Standard

		sampler2D _MainTex;
		float _Speed;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half4 TexColor = tex2D(_MainTex, float2(IN.uv_MainTex.x + _SinTime.z * _Speed, IN.uv_MainTex.y + _CosTime.z * _Speed));
			//o.Emission = float3 (IN.uv_MainTex.x, IN.uv_MainTex.y, 0);



			o.Emission = TexColor;

			//_Time(t/20, t, t*2, t*3)
			//_SinTIme(t/8, t/4, t/2, t)
			//_CosTime(t/8, t/4, t/2, t)
			//unity_DeltaTime(dt, 1/dt, smoothDt, 1/smoothDt)

		}





		ENDCG
	}
}