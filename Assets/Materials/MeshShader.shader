// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Msz/MeshShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _WrapAmount ("Wrap Amount", Range (1.0, 0.5)) = 0.5
        _Shinness ("Shinness", Range (0.0, 100)) = 0.3
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
                float3 normal : NORMAL;
            };

            fixed4 _Color;
            fixed4 _ShineColor;
            uniform float _WrapAmount;
            uniform float _Shinness;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex	);
                o.uv = v.texcoord;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }
            


            fixed4 frag (v2f i) : SV_Target
            {
            	fixed4 lightDirection = normalize(_WorldSpaceLightPos0);
            	half diff = dot(lightDirection, i.normal) * _WrapAmount + (1 - _WrapAmount);
            	fixed4 diffCol = diff*_Color;

            	fixed4 eyePosition = normalize(float4(_WorldSpaceCameraPos,1) - i.position);

            	fixed4 halfVector = normalize(lightDirection + eyePosition);
            	fixed4 spec = pow( saturate( dot(i.normal,halfVector)), _Shinness)*_ShineColor;

                return diffCol + spec;
            }
            ENDCG
        }
    }
}