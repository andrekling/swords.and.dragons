Shader "Andre/ToonShader2" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "white" {}
		_2Tex ("Second Map (RGB)", 2D) = "white" {}
		_RampTex ("Ramp (RGB)", 2D) = "white" {}
		
		_OutlineThikness ("Outliner Thikness", Range(-0.1,0.1)) = 0
		_OutlinerColor ("OutlineColor",Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Front
		Lighting Off
		
	 CGPROGRAM
      #pragma surface surf Lambert vertex:vert
     
     
       struct Input {
          float2 uv_MainTex;
      };
      float _OutlineThikness;
      void vert (inout appdata_full v) {
          v.vertex.xyz += v.normal * _OutlineThikness;
      }
      sampler2D _MainTex;
      fixed4 _OutlinerColor; 
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = _OutlinerColor;
      }
      ENDCG
		
		Cull Back
		Lighting On
		
		CGPROGRAM
		
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf BasicDiffuse
		
		struct SurfaceOutputAndre
{
    fixed3 Albedo;  // diffuse color
    fixed4 ShadowMap; // extra color
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
		float4 ramp = tex2D (_RampTex, uv_ramp);
		
		
		
		float4 col;
		float3 BlendColor;
		
		fixed sm1 = lerp(1,s.ShadowMap.b,ramp.b);
		fixed sm2 = lerp(1,s.ShadowMap.g,ramp.g);
		fixed sm3 = lerp(1,s.ShadowMap.r,ramp.r);
		fixed sm4 = lerp(0,s.ShadowMap.a,ramp.a);
		fixed sm5 = sm1 * sm2 * sm3 - 0.5;
		//BlendColor = lerp(s.ShadowMap.b,s.ShadowMap.g,ramp.b);
		//BlendColor = lerp(BlendColor,s.ShadowMap.b,ramp.g);
		//BlendColor = lerp(BlendColor,s.Albedo,ramp.b);
		
		
		//col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 2) * (ramp);
		
		//col.rgb =  (dot (s.Normal, lightDir))/2;
		//col.rgb = s.Albedo * BlendColor;
		//col.rgb = s.Albedo * sm5 + sm4;
		//col.rgb = _LightColor0.rgb * saturate(difLight * ( 2 * sm5));
		col.rgb = (difLight + 0.5) * sm5;
		//col.rgb = ramp;
		//col.rgb = sm5;
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
