Shader "Unlit/TexManage"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #define colorPres 

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed DeresNumber(float num)
            {
                num = floor(num * 10)/10;
                return num;
            }

            fixed4 BreakColorRes(float4 col)
            {
                return fixed4(DeresNumber(col.x), DeresNumber(col.y), DeresNumber(col.z), col.a);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, fixed2(i.uv.x, 1 - i.uv.y));
                return BreakColorRes(col);
            }
            ENDCG
        }
    }
}
