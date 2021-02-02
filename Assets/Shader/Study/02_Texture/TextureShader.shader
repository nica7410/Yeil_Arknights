Shader "Study/02_Texture"
{
	Properties
	{
		//white, black, gray
		_MainTex("MainTex", 2D) = "white" {}
		
		
	}

		SubShader
	{
		CGPROGRAM

		#pragma surface surf Standard

		sampler2D _MainTex;
		half4 _Color;


		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//tex 2D(샘플러, UV)			
			
			float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
			//o.Emission = (MainTex.r + MainTex.g + MainTex.b) / 3;
			
			//o.Emission = (MainTex.r * 0.2989 + MainTex.g * 0.5870 + MainTex.b * 0.1140);
			o.Emission = MainTex.rgb;
			//GrayScale 값을 메인텍스쳐 rgb에 곱해줬음
			//더 이쁘다는데 크게 차이는 모르겠다.		

		}
		ENDCG
	}
}