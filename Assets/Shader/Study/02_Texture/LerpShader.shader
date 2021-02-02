Shader "Study/02_Texture_Lerp"
{
	Properties
	{
		//_MainTex는 기본으로 가져야 하는 것 같다.
		_MainTex("TexFrom", 2D) = "white" {}
		_MainTex2("TexTo", 2D) = "gray" {}
		_Amount("Amount", range(0,1)) = 0.5
	}
		SubShader
	{
		CGPROGRAM

		#pragma surface surf Standard

		sampler2D _MainTex;
		sampler2D _MainTex2;
		float _Amount;

		struct Input
		{
			//엔진 내에 있는걸 가져오기 때문에 다른 형태로 가져올 수 없다.
			//uv_TexMain 이런 식은 x
			float2 uv_MainTex;
			float2 uv_MainTex2;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half4 fromColor = tex2D(_MainTex, IN.uv_MainTex);
			half4 toColor = tex2D(_MainTex2, IN.uv_MainTex2);
			o.Emission = lerp(fromColor.rgb, toColor.rgb, 1-fromColor.a * _Amount);


			//fromColor.a : 0 = 검은색
			//fromColor.a : 1 = 색






			//풀어서 적으면 이런 느낌
			//half4 lerpColor = lerp(fromColor, toColor, _Amount);
			//o.Emission = lerpColor.rgb;

			//받아온 두 텍스쳐 컬러의 중간점을 찾아주는 거라고 보면 된다.
			//fromColor의 r,g,b 각각의 값과
			//toColor의 r,g,b 각각의 값을 비교해서 중간점을 알려준다.
			//_Amount 가 중간점을 설정하는 역할을 한다.
			
			//////선형보간



		}



		ENDCG
	}
}