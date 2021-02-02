Shader "project/UIBackGradation"
{
    Properties
    {
       _MainTex("MainTex", 2D) = "black" {}
       _SubTex("SubTex",2D) = "black"{}
       _Lerptest("LerpRange", Range(0,1)) = 0

    }
        SubShader
       {
          Tags {
            "RenderType" = "Transparent"
            "Queue" = "Transparent" //이거 차이점 느끼면 좋은거 쓰도록

         }
           zwrite On //불투명하게 만들꺼기에 off
           Blend SrcAlpha OneMinusSrcAlpha

           CGPROGRAM
           #pragma surface surf Lambert addshadow alpha:fade

           //#pragma shader_feature REDIFY_ON
          #include "UnityCG.cginc"


       sampler2D _MainTex;
       sampler2D _SubTex;
       half _Lerptest;

       struct Input
       {
          float2 uv_MainTex;
          float2 uv_SubTex;
       };

       void surf(Input IN, inout SurfaceOutput o)
       {
          fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
          fixed4 sub = tex2D(_SubTex, IN.uv_SubTex);

          o.Emission = c.rgb;
          o.Albedo = c.rgb;

          if (c.a == 1)
          {
              c.a = lerp(1, _Lerptest, IN.uv_MainTex.x);
          }
          else {
              //c.a = sub.a;
          }
          o.Alpha = c.a;


          //o.Alpha = lerp(1, _Lerptest, IN.uv_MainTex.x);
       }
       ENDCG
       }
           // FallBack "UI/Default"
            //FallBack "Legacy Shaders/Transparent/VerexLit"
            //CustomEditor "CustomShaderGUI"
}