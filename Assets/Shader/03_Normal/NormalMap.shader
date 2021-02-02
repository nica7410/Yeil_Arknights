Shader "Custom/NormalMap"
{
    Properties
    {
        _MainTex("Albedo (MainTex)", 2D) ="white"{}
        _BumpMap("Albedo (BumpMap)", 2D) = "white"{}
        //오클루젼 , 차폐 
        _Occlusion("Occlusion",2D) = "white" {}
    }
        SubShader
    {
        CGPROGRAM
        #pragma surface surf Standard
        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _Occlusion;

        struct Input 
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            half3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            //Occlusion Color
            half4 oc = tex2D(_Occlusion, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Normal = n;
            o.Occlusion = oc.r *0.5f;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
