Shader "Study/Texture"
{
    Properties
    {
        //white , black , gray
        _MainTex ("MainTex", 2D) = "black" {}
    }
     SubShader
    {
    CGPROGRAM

        #pragma surface surf Standard
        sampler2D _MainTex;

        struct Input
        {
            //지정된 변수명 사용
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            //tex2D(샘플러, uv)Albedo
            o.Emission = tex2D(_MainTex, IN.uv_MainTex).rgb*0.3;
            
        }
     ENDCG
    }
    FallBack "Diffuse"
}
