Shader "Custom/RimLighting"
{
    Properties
    {
        _RimColor("Rim Color", Color) = (0.0, 0.5, 0.5, 0.0)
        _RimPower("Rim Power", Range(0.5, 8.0)) = 3.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float3 normal : NORMAL;
			};
              
            struct v2f {
			    float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float3 normal : NORMAL;
            };
			
            sampler2D _MainTex;
			
            v2f vert(appdata v) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.viewDir = WorldSpaceViewDir(v.vertex);
				o.normal = v.normal;
                return o;
            }
			
			float4 _RimColor;
			float _RimPower;

            fixed4 frag(v2f i) : SV_Target 
            {
			    half rim = 1 - saturate(dot(normalize(i.viewDir), i.normal));
                fixed3 col = _RimColor.rgb * pow(rim, _RimPower);
                return fixed4(col, 1.0);
            }
			
            ENDCG
        }
    }
    FallBack "Diffuse"
}