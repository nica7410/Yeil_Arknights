Shader "Custom/HoloGram"
{
    Properties
    {
        _MainTex("Albedo (MainTex)",2D)= "white"{}
        _RimPower("RimPower", Range(0,50)) = 1
        _BumpMap("BumpMap",2D) = "white" {}
        _RimColor("Albedo(RimColor)", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags {
            "RanderType" = "Transparent"
            "Queue" = "Transparent"
        }
        CGPROGRAM
    
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        sampler2D _BumpMap;
        half _RimPower;
        half4 _RimColor;
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
            float3 worldPos;

        };
        void surf (Input IN, inout SurfaceOutput o)
        {
            //float4 c = tex2D(_MainTex, IN.uv_MainTex);
            //o.Albedo = c.rgb;
           float3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
           o.Normal = n.rgb;
       
            float rim = saturate(dot(o.Normal, IN.viewDir));
            //Emission 
            //o.Emission = pow(1-rim, _RimPower) * _RimColor.rgb;
            //o.Emission = IN.worldPos.g;
            o.Emission = pow(frac(IN.worldPos.r *3 - _Time.y),30);


            //Alpha 
            o.Alpha = pow(1 - rim, _RimPower);
            
            //깜빡거리게 하기
            //o.Alpha = rim * sin(_Time.y);
            //o.Alpha = rim * (sin(_Time.y) * 0.5 + 0.5); //레벨링 디자인에 많이 사용
            //o.Alpha = rim * abs(sin(_Time.y)); // 몹죽을때 많이 사용  //abs는 절대값
            //o.Alpha = 1;
            

            /*sutruct input 
            float3 viewDir : 바라보는 방향
            float4 color:COLOR : 버텍스 컬러
            float4 screenPos : 스크린 공간에서의 위치
            float3 worldPos : 월드 공간에서의 위치
            float3 worldNormal : 월드 노멀 벡터
            float3 worldRefl : 월드 반사 벡터
            */
        }
        ENDCG
    }
    FallBack "Diffuse"
}
