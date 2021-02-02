Shader "Custom/ColorShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
    }
        SubShader
    {
        LOD 200
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        //#pragma surface surf Lambert
        #pragma target 3.0

        struct Input
        {
            fixed4 c;
        };

        fixed4 _Color;


        void surf (Input IN, inout SurfaceOutputStandard o)
        //void surf (Input IN, inout SurfaceOutput)
        {
            o.Albedo = _Color.rgb;
        }
        ENDCG
    }

}
