Shader "Hidden/fxShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_vDisp("Vector Displacement", 2D) = "white" {}
		_time("effecttime", Float) = 0
		_inten("effect multiplier", Float) = 1
		_yImpact("y ss location", Float) = 0.5
		_xImpact("x ss location", Float) = 0.5
		_timescale("Scales the time scale", Float) = 5
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
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			uniform sampler2D _vDisp;
			float _xImpact;
			float _yImpact;
			float _inten;
			float _time;
			float _timescale;

			float4 frag(v2f i) : COLOR
			{
				float4 normal = tex2D(_vDisp, i.uv);
				float2 disp = normal.rg * 2 - 1;
				float distance = sqrt(pow(0.5 - i.uv.x, 2) + pow(0.5 - i.uv.y, 2));
				float swave = 0.9*sin(distance * 3.1415926535);
				//i.uv -= swave.rr * 0.1 * sin(3.1415926535 * _time);
				i.uv += disp * _inten * sin(3.1415926535 * _timescale *_time);
				i.uv = saturate(i.uv);

				float4 c = tex2D(_MainTex, i.uv);
				return c;
		}
		ENDCG
	}
	}
}
