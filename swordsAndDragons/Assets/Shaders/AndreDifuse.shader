Shader "Andre/AndreDifuse" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RampTex ("Ramp (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf BasicDiffuse
		
		uniform sampler2D _RampTex;
		inline float4 LightingBasicDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
		float difLight = max(0, dot (s.Normal, lightDir));
		float3 ramp = tex2D (_RampTex, float2(difLight,difLight)).rgb;
		
		
		float4 col;
		col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 2) * (ramp);
		col.a = s.Alpha;
		return col;
		}

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
