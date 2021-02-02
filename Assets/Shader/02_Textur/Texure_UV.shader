Shader "Custom/Texure_UV"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Speed("Speed", Range(0,200)) = 0
    }
        SubShader
    {
        CGPROGRAM
        #pragma surface surf Standard
        sampler2D _MainTex;
        float _Speed;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input In, inout SurfaceOutputStandard o)
        {
            //half4 color = tex2D(_MainTex, In.uv_MainTex);
            //half4 color = tex2D(_MainTex, float2 (In.uv_MainTex.x + _Time.y, In.uv_MainTex.y));
            //half4 color = tex2D(_MainTex, float2 (In.uv_MainTex.x + _SinTime.w, In.uv_MainTex.y)); //Sin
            half4 color = tex2D(_MainTex, float2 (In.uv_MainTex.x + (_SinTime.y* _Speed), In.uv_MainTex.y));

            half4 uvcolor = In.uv_MainTex.x;
            //o.Emission = color.rgb;
            //o.Emission = In.uv_MainTex.y;
            //o.Emission = color * uvcolor;
            //o.Emission = float3(In.uv_MainTex.x, In.uv_MainTex.y,0);
            o.Emission = color;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
