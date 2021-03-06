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
				float2 screenPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			uniform float4 _MaskTransform;
			uniform float _Amount;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

				float dist = distance(i.screenPos * _ScreenParams.xy, _MaskTransform.xy * _ScreenParams.xy);
				if (dist > _MaskTransform.z)
				{
					float amnt = col.r * 0.299 + col.g * 0.587 + col.b * 0.114;
					col.rgb = fixed3(amnt, amnt, amnt);
				}
				else if (dist > _MaskTransform.z - 10) {
					col.rgb = fixed3(1, 1, 1);
				}
				else {
					float colR = tex2D(_MainTex, float2(i.uv.x - _Amount, i.uv.y - _Amount)).r;
					float colG = tex2D(_MainTex, i.uv).g;
					float colB = tex2D(_MainTex, float2(i.uv.x + _Amount, i.uv.y + _Amount)).b;
					col.rgb = fixed3(colR, colG, colB);
				}

                return col;
            }
            ENDCG
        }
    }
}
