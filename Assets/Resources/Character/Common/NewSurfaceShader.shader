Shader "Custom/Outline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range (.002, 0.03)) = .005
    }
    SubShader
    {
        Tags {"Queue" = "Overlay" }
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }

            Cull Front

            ZWrite On
            ZTest LEqual
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert (appdata v)
            {
                // just make a copy of incoming vertex data but scaled according to normal direction
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 norm = mul((float3x3) unity_ObjectToWorld, v.normal);
                o.pos.xy += norm.xy * o.pos.w * _OutlineWidth;
                o.color = _OutlineColor;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

