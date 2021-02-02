Shader "Study/Texture_Effect"
{
    Properties
    {
        _MainTex("Albedo (RGB)",2D)="white"{}
        _MainTex2("Albedo (RGB)",2D) = "white" {}
    }
    SubShader
    {
        Tags{
              "RenderType" = "Transparent"
              "Queue" = "Transparent"
            }
        CGPROGRAM
       
        #pragma surface surf Standard alpha:fade
         sampler2D _MainTex;
         sampler2D _MainTex2;

        struct Input
        {
            float2 uv_MainTex;
        };

   

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half4 color = tex2D(_MainTex, IN.uv_MainTex);
            half4 color2 = tex2D(_MainTex2, IN.uv_MainTex);
            o.Emission = color;
            o.Alpha = color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
