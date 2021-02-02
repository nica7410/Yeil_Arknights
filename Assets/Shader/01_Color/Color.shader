Shader "Study/Color"
{
    //속성
    Properties
    {
        //코드상에서 사용할 변수명 ("인스펙터에 표시될 이름", 타입) = 초기화/초기값
         //_Color ("Color", Color) = (1,1,1,1)
        _ColorR("ColorR", Range(0,1)) = 0
        _ColorG("ColorG", Range(0,1)) = 0
        _ColorB("ColorB", Range(0,1)) = 0
        _Metallic("Metallic", Range(0,1)) = 1
        _Glossiness("Glossiness",Range(0,1)) = 1
    }
        //실제 셰이더가 작성되는 부분
    SubShader
    {
        //CG를 사용하는 코드
        CGPROGRAM
        //Surface Shader 함수명 함수 타입
        #pragma surface surf Standard

        //#pragma surface surf Lambert

        //#pragma target 3.0

        //sampler2D _MainTex;
        
        //Surface에서사용되는 구조체
        //정해진 형태의 자료만 전달 받을 수 있으며,
        //변수명이 일치해야된다.
        //인풋 구조체 선언 비어있으면 안된다.
        struct Input
        {
            float4 c :COLOR; 
        };

    //기본사양
    //struct SurfaceOutputStandard
    //{
    //    fixed3 Albedo;  //반사되는 색
    //    fixed3 Normal;  //법선
    //    half3 Emission; //발산되는 색
    //    half Metallic;  //0 = 메탈 영향 없음 , 1=메탈
    //    half Smoothness;//거칠게 , 1= 부드럽게
    //    half Occlusion; //차폐로 인한 환경광의 영향도 (기본 1)
    //    fixed Alpha;    //투명도 
    //};
        // 전역 변수설정 변수명은 위에 프로퍼티에서 지정한 변수명과 같아야된다.
        fixed4 _Color;
        float _ColorR;
        float _ColorG;
        float _ColorB;
        half _Metallic;
        half _Glossiness;
        

        //파라메터 명을 제외한 함수의 형태는 고정
        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            //float half fixed 
            //Nvidia 기준 half가 제일빠름
            //1이랑 가까워질수록 밝아지고 0이랑 가까워질수록 어두워짐
            //o.Albedo = _Color.rgb;
            //o.Emission = float3(1, 0, 0) + float3(0, 1, 0); //노랑
            //o.Emission = float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5); //흰색
            //o.Emission = float3(1, 0, 0)* float3(0, 1, 0); //검은색
            //o.Emission = float3(0.5, 0.5, 0.5) * float3(0.5, 0.5, 0.5); //회색
            //o.Emission = float3(1, 0.5, 0.5) * float3(0.5, 0.5, 0.5); //연한 빨강색
            
            //스위즐링(Swizzling) : color.bgr , color.bbb , color.g , color.grb;
            //float4 color = float4(1, 0, 0, 1); //
            //o.Emission = color.gbr; //파랑색

            float4 color = float4(_ColorR, _ColorG, _ColorB, 1);
            o.Albedo = color.rgb;
            o.Metallic = _Metallic; //광택
            o.Smoothness = _Glossiness; //부드러움
        }
        //컴퓨터 연산을 처리해주는것을 GPU로 전달해주는것들 용어
        //Compute Programming
        //CUDA
        ENDCG
    }
    FallBack "Diffuse"
}
