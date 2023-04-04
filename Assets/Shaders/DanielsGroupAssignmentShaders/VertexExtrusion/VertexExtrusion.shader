Shader "Custom/VertexExtrusion"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _NormalScale("Scale", Range(-10, 10)) = 0
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
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};
			
			float4 _Color;
            sampler2D _MainTex;
			float4 _MainTex_ST;
			float _NormalScale;
			
            v2f vert(appdata v) 
            {
                v2f o;

				o.uv = v.uv;
				o.normal = v.normal;
                o.vertex = UnityObjectToClipPos(v.vertex + o.normal * _NormalScale);
				
				return o;
			}

            fixed4 frag(v2f i) : SV_Target 
            {
			    fixed4 col;

				col = tex2D(_MainTex, i.uv * _MainTex_ST.xy + _MainTex_ST.zw) * _Color;

				return col;
            }
			
            ENDCG
        }
    }
    FallBack "Diffuse"
}