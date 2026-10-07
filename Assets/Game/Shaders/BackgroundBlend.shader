Shader "Game/BackgroundBlend"
{
    Properties
    {
        _MainTex ("Current background", 2D) = "white" {}
        _NextTex ("Next background", 2D) = "white" {}
        _Blend ("Transition", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Cull Off ZWrite On
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex, _NextTex;
            float4 _MainTex_ST;
            float _Blend;
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 vertex : SV_POSITION; float2 uv : TEXCOORD0; };
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                return lerp(tex2D(_MainTex, i.uv), tex2D(_NextTex, i.uv), _Blend);
            }
            ENDCG
        }
    }
}
