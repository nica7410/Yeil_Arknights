Shader "Study/02_Texture_Water"
{
	Properties
	{
		
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
			float2 Pos = float2(-_SinTime.x, +_CosTime.x) * 0.5;
			half4 noise = tex2D(_NoiseTex, IN.uv_MainTex + Pos);
			half4 Color = tex2D(_MainTex, IN.uv_MainTex);
			half4 Color2 = tex2D(_MainTex, IN.uv_MainTex + noise);
			


			o.Emission = Color * Color2 * 1.5;
			o.Alpha = Color.a;
		}



		ENDCG
	}
}