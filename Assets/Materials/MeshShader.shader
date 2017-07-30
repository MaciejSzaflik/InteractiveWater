Shader "Msz/MeshShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _WrapAmount ("Wrap Amount", Range (1.0, 0.5)) = 0.5
        _Shinness ("Shinness", Range (0.0, 2)) = 0.3
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
                fixed4 diff : COLOR0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            fixed4 _Color;
            fixed4 _ShineColor;
            uniform float _WrapAmount;
            uniform float _Shinness;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }
            


            fixed4 frag (v2f i) : SV_Target
            {
            	half nl = dot(i.normal, _WorldSpaceLightPos0.xyz) * _WrapAmount + (1 - _WrapAmount);
            	fixed4 sl = 1 - (dot(i.normal, _WorldSpaceCameraPos.xyz) * _ShineColor)*_Shinness;
                fixed4 col = _Color*nl + sl;
                return col;
            }
            ENDCG
        }
    }
}