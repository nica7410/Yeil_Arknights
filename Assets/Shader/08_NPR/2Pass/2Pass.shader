Shader "Study/09_Outline2Pass"
{
	Properties
	{
		_MainTex("Albedo (RGB)",2D) = "white"{}

	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque"}
		cull front	//뒤집어서 그림, 내부가 보인다.
		//cull back		//뒤에만 그림, 원래 보던거
		//cull off		//다 그림, 보통 풀 같은거에 씀, 부하가 많이 걸림
		//1Pass
		CGPROGRAM
		#pragma surface surf Nolight vertex:vert noambient noshadow	//버텍스쉐이더 사용
		//뒤에 깔리기 때문에 조명과 엠비언트 처리를 할 필요가 없다.
		struct Input { float4 color:COLOR; };
		void vert(inout appdata_full v)
		{
			//v.vertex += 1;	//물체의 위치는 그대로, 정점의 위치만 움직임
			v.vertex.xyz += v.normal.xyz * 0.01; //위치잡아줌
		}

		void surf(Input IN, inout SurfaceOutput o) {}

		float4 LightingNolight(SurfaceOutput s,
								float3 lightDir,
								float atten)
		{
			return float4(0, 0, 0, 1);
		}
		ENDCG

			//2PASS
			cull back
			CGPROGRAM
			#pragma surface surf Toon Lambert
			sampler2D _MainTex;
			struct Input
			{
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
			}

			float4 LightingToon(SurfaceOutput s,
								float3 lightDir,
								float atten)
			{	//Half Lambert
				float ndot1 = dot(s.Normal, lightDir) * 0.5 + 0.5;

				//if (ndot1 > 0.2)ndot1 = 1;
				//else ndot1 = 0.5;

				ndot1 = ndot1 * 5;
				ndot1 = ceil(ndot1) / 5;
				return ndot1;
			}
			ENDCG
	}
}