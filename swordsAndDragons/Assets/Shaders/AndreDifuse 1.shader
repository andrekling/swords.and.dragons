Shader "Andre/AndreDifuse2" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "white" {}
		_2Tex ("Second Map (RGB)", 2D) = "white" {}
		_RampTex ("Ramp (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
	
		
		
		CGPROGRAM
		
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf BasicDiffuse
		
		struct SurfaceOutputAndre
{
    fixed3 Albedo;  // diffuse color
    fixed3 ShadowMap; // extra color
    fixed3 Normal;  // tangent space normal, if written
    fixed3 Emission;
    half Specular;  // specular power in 0..1 range
    fixed Gloss;    // specular intensity
    fixed Alpha;    // alpha for transparencies
};
		
		uniform sampler2D _NormalMap;
		uniform sampler2D _2Tex;
		uniform sampler2D _RampTex;
		uniform sampler2D RText;
		
		
		
		inline float4 LightingBasicDiffuse (SurfaceOutputAndre s, fixed3 lightDir, fixed atten)
		{
		
		float difLight = max(0, dot (s.Normal, lightDir));
		float2 uv_ramp={difLight,difLight};
		float3 ramp = tex2D (_RampTex, uv_ramp).rgb;
		
		
		
		float4 col;
		float3 BlendColor;
		fixed _Color = (1);
		fixed sm1 = lerp(_Color,s.ShadowMap.r,ramp.r);
		fixed sm2 = lerp(_Color,s.ShadowMap.g,ramp.g);
		fixed sm3 = lerp(_Color,s.ShadowMap.b,ramp.b);
		fixed sm4 = sm1 * sm2 * sm3 - 0.5;
		//BlendColor = lerp(s.ShadowMap.b,s.ShadowMap.g,ramp.b);
		//BlendColor = lerp(BlendColor,s.ShadowMap.b,ramp.g);
		//BlendColor = lerp(BlendColor,s.Albedo,ramp.b);
		
		
		col.rgb = s.Albedo * _LightColor0.rgb * ((difLight + 0.5) * sm4);
		//col.rgb = ramp * s.Albedo;
		//col.rgb =  (dot (s.Normal, lightDir))/2;
		//col.rgb = s.Albedo * BlendColor;
		//col.rgb = s.Albedo * sm4;
		//col.rgb = s.ShadowMap.b;
		col.a = s.Alpha;
		//out uv_ramp;
		return col;
		}

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			
		};
		

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputAndre o) {
		//in uv_ramp;
		float3 normalMap = UnpackNormal(tex2D(_NormalMap,IN.uv_MainTex));
		
						
		o.Normal = normalMap.rgb;
		
		float4 BlendColor;
		fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;	
		float4 RText = tex2D(_2Tex, IN.uv_MainTex);
		//float4 GText = tex2D(_2Tex, IN.uv_MainTex);
		//float4 BText = tex2D(_2Tex, IN.uv_MainTex);
		
		BlendColor = lerp(RText.r,RText.g,c.g);
			
			
			//Trying to blend textures
			//o.Albedo = BlendColor.rgb;
			
			
			//Passing the rgb as the color
			o.Albedo = c.rgb;
			o.ShadowMap = RText;
			//o.Albedo = RText.rgb;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
