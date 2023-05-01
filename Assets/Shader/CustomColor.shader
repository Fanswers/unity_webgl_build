Shader "Custom/ColorShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Opaque"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // include UnityShaderVariables for built-in variables like UNITY_MATRIX_MVP
            #include "UnityShaderVariables.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            // fragment shader
            fixed4 frag ()
            {
                return _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
