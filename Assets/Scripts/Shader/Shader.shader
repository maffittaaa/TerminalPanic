Shader "Unlit/Shader"
{
     Properties
    {
        _OutlineThickness ("Outline Thickness", Float) = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" }
        LOD 100

        Pass
        {
            Name "Outline"
            Cull Front 
            ZWrite On
            ZTest LEqual
            ColorMask RGB
            Blend Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                worldPos += norm * _OutlineThickness;
                o.pos = UnityWorldToClipPos(float4(worldPos, 1.0));
                return o; 
            }

            float4 frag(v2f i) : SV_Target
            {
                return float4(1, 0.514151, 0.514151, 1);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
