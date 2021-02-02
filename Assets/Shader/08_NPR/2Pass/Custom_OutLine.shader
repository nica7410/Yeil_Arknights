Shader "Study/Custom_OutLine"
{
    Properties
    {
       _MainTex("Albedo (RGB)",2D) = "white"{}
       _Color("Color",Color) = (0,0,0,1)
    }
        SubShader
       {
          Tags{ "RenderType" = "Opaque"}
          cull front   //뒤집어서 그림, 내부가 보인다.
          //cull back      //뒤에만 그림, 원래 보던거
          //cull off      //다 그림, 보통 풀 같은거에 씀, 부하가 많이 걸림
          //1Pass
          CGPROGRAM
          #pragma surface surf Nolight vertex:vert noambient noshadow   //버텍스쉐이더 사용
           //뒤에 깔리기 때문에 조명과 엠비언트 처리를 할 필요가 없다.
           struct Input { float4 color:COLOR; };
           void vert(inout appdata_full v)
           {
               //v.vertex += 1;   //물체의 위치는 그대로, 정점의 위치만 움직임
               v.vertex.xyz += v.normal.xyz * 0.1;   //아웃라인 두께 or 크기
            }

            void surf(Input IN, inout SurfaceOutput o) {}

            float4 LightingNolight(SurfaceOutput s,
                              float3 lightDir,
                              float atten)
            {
               return float4(0, 0, 0, 1);   //아웃라인 색상
            }
            ENDCG

                //2PASS
                cull back
                CGPROGRAM
                #pragma surface surf Toon Lambert
                sampler2D _MainTex;
                half4 _Color;
                struct Input
                {
                   float2 uv_MainTex;
                };

                void surf(Input IN, inout SurfaceOutput o)
                {
                   half4 c = tex2D(_MainTex, IN.uv_MainTex);
                   o.Albedo = c.rgb;
                   o.Emission = _Color.rgb;
                }

                float4 LightingToon(SurfaceOutput s,
                               float3 lightDir,
                               float atten)
                {   //Half Lambert
                   float ndot1 = dot(s.Normal, lightDir) * 0.5 + 0.5;

                   //1단계 툰 쉐이딩
                   //if (ndot1 > 0.7) ndot1 = 1;
                   //else ndot1 = 0.1;

                   //5단계 툰 쉐이딩
                   /*ndot1 = ndot1 * 5;
                   ndot1 = ceil(ndot1) / 5;*/



                   return ndot1;
                }
                ENDCG
       }
}

//void vert(inout appdata_full v){}
//appdata_base   :position, normal, and one texture coordinate
//appdata_tan   :position, tangent, normal and one texture coordinate
//appdata_full   :position, tangent, normal, four texture coordinate and color