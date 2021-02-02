Shader "Custom/13_AlphaBleending"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        { 
            "RenderType"="Transparent"
            "Queue" = "Transparent"
        }
        cull off
        zwrite off

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
FallBack "Legacy Shaders/Transparent/VerexLit"
}


/*
불투명(Opaque)
반투명(Transparent)
반투명끼리는 뒤에서 부터
알파블렌딩을 사용할때는 
zwrite 사용하지 않음
*/