// Shader : 쉐이더, 셰이더
// "카테고리/셰이더 명"
Shader "Study/01_Color"
{
	//속성
	Properties
	{
		//코드상에서 사용할 변수명
		//("인스펙터에서 보여질 이름 "", 타입) = 초기화, 초기값
		//_Color("Color", Color) = (1,1,1,1)
		_ColorR("Color R",  Range(0, 1)) = 1
		_ColorG("Color G",  Range(0, 1)) = 1
		_ColorB("Color B",  Range(0, 1)) = 1
		_Metallic("Metallic", Range(0, 1)) = 1
		_Glossiness("Smoothness", Range(0, 1)) = 1

	}

		SubShader
	{
		CGPROGRAM

		//Surface Shader 함수명, 함수타입
		#pragma surface surf Standard

		// Surface에서 사용되는 구조체는 정해진 형태의 자료만 전달 받을 수 있으며,
		// 변수명이 일치해야 한다.

		struct Input
		{
			float2 uv_MainTex;
		};

	//struct SurfaceOutputstandard
	//	{
	//	fixed3 Albedo;		// 반사되는 색		3D느낌
	//	fixed3 Normal;		// 법선
	//	half3 Emission;		// 발산되는 색		2D느낌
	//	half Metallic;		// 0 = 메탈 영향 x
	//	half Smoothness;	// 0 = 거칠게
	//	half Occlusion;		// 차폐로 인한 환경광의 영향도 (기본 1)
	//	fixed Alpha;		// 투명도

	//	};

	//전역 변수
	half _ColorR;
	half _ColorG;
	half _ColorB;
	half _Metallic;
	half _Glossiness;

	//변수명을 제외한 함수의 형태는 고정
	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		//o.Albedo = _Color.rgb;		//조명연산 포함 색 출력
		//o.Emission = _Color.rgb;		//조명연산 제외 색 출력
		//o.Emission = float3(0.5, 0.5, 0.5) * float3(0.5, 0.5 ,0.5);

		
		//Swizzling

		//color.bgr		순서 바꾸기
		//color.bbb		같은 것만 넣기
		//color.g		전부 다 같은 값이 들어감 (회색, 흰색, 검은색) 중 하나가 나오겠네요.

		//float4 color = float4(1, 0, 0, 1);
		//o.Emission = color.grb;	// = (0, 1, 0)	초록색이 나옴
		//o.Emission = color.gbr; // = (0, 0, 1)	파란색이 나옴
		//o.Emission = color.r;

		//float c1 = 1;
		//float2 c2 = float2(0.5, 0);
		//float3 c3 = float3(c1, c2);			//1, 0.5, 0
		//이런 식으로 섞는 것도 가능


		o.Albedo = fixed3(_ColorR, _ColorG, _ColorB);
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;


	}
	ENDCG
	}
}