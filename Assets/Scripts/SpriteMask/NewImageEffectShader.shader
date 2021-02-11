// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TeeNik/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		//_MaskTransform("MaskTransform", Vector) = (0,0,0,0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				fixed4 worldPos : WORLDPOS;
				float2 screenPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			uniform float4 _MaskTransform;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				float dist = distance(i.screenPos / _ScreenParams.xy, _MaskTransform.xy);
				//if (dist > .5)
				//{
				//	float amnt = col.r * 0.299 + col.g * 0.587 + col.b * 0.114;
				//	col.rgb = fixed3(amnt, amnt, amnt);
				//}
				//col.rgb = float3(i.screenPos, 0);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
				//
				//float dist = distance(projected.xy, _MaskTransform.xy);
				//if (dist > _MaskTransform.z)
				//{
				//	float amnt = col.r * 0.299 + col.g * 0.587 + col.b * 0.114;
				//	col.rgb = fixed3(amnt, amnt, amnt);
				//}
				//col.rgb = float3(i.uv, 0);

				//col.rgb = float3(i.screenPos , 0);

                return col;
            }
            ENDCG
        }
    }
}
