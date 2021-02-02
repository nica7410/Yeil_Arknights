Shader "Study/Texture_Lerp"
{
    Properties
    {
        //_MainTex는 기본으로 가져야 함
        _MainTex("TexFrom",2D) = "white" {}
        _MainTex2("TexTo",2D) = "gray" {}
        _Amount("Amount",Range(0,1)) = 0
        
    }
    SubShader
    {
        CGPROGRAM 

        #pragma surface surf Standard

        sampler2D _MainTex;
        sampler2D _MainTex2;
        half _Amount;

        struct Input 
        {
            float2 uv_MainTex;
            float2 uv_MainTex2;
        };
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            half4 fromColor = tex2D(_MainTex, IN.uv_MainTex);
            half4 toColor = tex2D(_MainTex2, IN.uv_MainTex2);
            //Alpha Blending
            //Linear Interpolation
            //half4 lerpColor = lerp(fromColor, toColor, _Amount);
            half4 lerpColor = lerp(fromColor, toColor, fromColor.a);
            o.Emission = lerpColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
