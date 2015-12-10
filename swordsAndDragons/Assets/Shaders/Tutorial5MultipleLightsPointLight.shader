Shader "AndreTrainning/Tutorial5MultipleLightsPointLight " {
	Properties {
 		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
		_SpecularColor ("Specular Color", Color) = (1.0,1.0,1.0,1.0)
		_Shininess ("Specular Power", float) = 10
		_RimColor ("Rim Color", Color) = (1.0,1.0,1.0,1.0)
		_RimPower ("Rim Power", float) = 1.0
		_RimStrenght ("Rim Strenght", float) = 1.0
	}
	SubShader {
		
		PASS{
		TAGS {"LightMode"="ForwardBase"	} // This is the lighting way we have to set to the first pass to lit the object.- different effect if used forwardAdd
		
		CGPROGRAM
		//Pragmas
		#pragma vertex vert
		#pragma fragment frag
		//User Defined Variables
		uniform float4 _Color;
		uniform float4 _SpecularColor;
		uniform float _Shininess;
		uniform float4 _RimColor;
		uniform float _RimPower; // the value we will elevate the rim to the power of
		uniform float _RimStrenght; // we will multiply everything by this value, since when we increase the value, it gets smaller in the sides
		//Unity Defined Variables
		uniform float4 _LightColor0;
		
		//Base Inputs Struct declarations
		
		struct vertexInput
		{
		float4 vertex : POSITION; //NAMED VERTEX
		float3 normal : NORMAL;
		
		};
		 
		struct vertexOutput
		{
		float4 pos : SV_POSITION;
		//We pass the following values so we can calculate the specualr in the fragment program, we pass than as texcoord, we can go up to 8 if im not wrong
		float4 posWorld : TEXCOORD0;
		float3 normalDir : TEXCOORD1;
		float4 col : COLOR;
		};
		
		//Vertex Program
		vertexOutput vert(vertexInput v){
		vertexOutput o;
		
		
		
		o.posWorld = mul(_Object2World, v.vertex); //we set the .posWorld to the position of the vertex in world space
		o.normalDir = normalize(mul(float4 (v.normal,0.0), _World2Object).xyz ) ; // we pass the normal of the vertex in object space, buy we first normalize it
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // for position, we must multiply the vertex position, by the unity mvp matrix
				
		return o; // we pass o to the fragment program
		
		}
		
		//Fragment Program
		float4 frag(vertexOutput i):COLOR 
		{
		//VECTORS DEFINITION
		float3 normalDirection = i.normalDir; // we define a float3 normalDirection, and set it to the object normal dir
		float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz); // a view direction, that is the position of the camera in world space, minus the object position, this way we know the distance to the camera and if its facing the object, normalized.
		float3 lightDirection;
		float atten;
		
		if(_WorldSpaceLightPos0.w  == 0.0){//for directional Lights - if and else are heavy to calculate, avoid using it
		
		atten = 1.0;
		lightDirection = normalize (_WorldSpaceLightPos0.xyz);// for lightDirection, we simply normalize the position of the light0 in worldspace
		}
		else{
		float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz; // We get the light distance of the light to the fragment - facing the object
		float distance = length(fragmentToLightSource); // we get the distance from the light to the object
		atten = 1/distance; //we decrease the attenuation as the light gets closer of the object
		lightDirection = normalize(fragmentToLightSource); // we normalize this so we get a 1.0 vector
		
		}
		
		//LIGHTING
		float3 diffuseReflection = atten * _LightColor0.rgb  * max(0, dot (lightDirection, normalDirection));
		float3 specularReflection = atten * _LightColor0.rgb * max(0, dot (lightDirection, normalDirection)) * pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
		
		//Rim Lighting
		float rim = _RimStrenght * (1 - saturate(dot(normalize(viewDirection), normalDirection)));// for rim, we do a dot product of the view direction by the normal direction, we clamp it with a saturate and subtract by minus 1 to invert its value. so its brighter at the edge and dark inside face
		float3 rimLighting = atten * _LightColor0.rgb * _RimColor * saturate(dot(normalDirection, lightDirection)) * pow (rim, _RimPower); //after that we multiply by the rim color, the attenuation value, the light color, and we also mask out the part thats not being iluminated with the dot of light dir by normal dir, than we elevate to the power of rim
		float3 lightFinal = rimLighting + diffuseReflection + specularReflection +  UNITY_LIGHTMODEL_AMBIENT; // finally we add all the lights toggeter also, the unity lightmodel ambient
		
		return float4(lightFinal * _Color.xyz, 1.0); 
		}
																	
		ENDCG
		}
		PASS{
		TAGS {"LightMode"="ForwardAdd"	} // This is the lighting way we have to set to the first pass to lit the object.- different effect if used forwardAdd
		Blend One One
		CGPROGRAM
		//Pragmas
		#pragma vertex vert
		#pragma fragment frag
		//User Defined Variables
		uniform float4 _Color;
		uniform float4 _SpecularColor;
		uniform float _Shininess;
		uniform float4 _RimColor;
		uniform float _RimPower; // the value we will elevate the rim to the power of
		uniform float _RimStrenght; // we will multiply everything by this value, since when we increase the value, it gets smaller in the sides
		//Unity Defined Variables
		uniform float4 _LightColor0;
		
		//Base Inputs Struct declarations
		
		struct vertexInput
		{
		float4 vertex : POSITION; //NAMED VERTEX
		float3 normal : NORMAL;
		
		};
		 
		struct vertexOutput
		{
		float4 pos : SV_POSITION;
		//We pass the following values so we can calculate the specualr in the fragment program, we pass than as texcoord, we can go up to 8 if im not wrong
		float4 posWorld : TEXCOORD0;
		float3 normalDir : TEXCOORD1;
		float4 col : COLOR;
		};
		
		//Vertex Program
		vertexOutput vert(vertexInput v){
		vertexOutput o;
		
		
		
		o.posWorld = mul(_Object2World, v.vertex); //we set the .posWorld to the position of the vertex in world space
		o.normalDir = normalize(mul(float4 (v.normal,0.0), _World2Object).xyz ) ; // we pass the normal of the vertex in object space, buy we first normalize it
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // for position, we must multiply the vertex position, by the unity mvp matrix
				
		return o; // we pass o to the fragment program
		
		}
		
		//Fragment Program
		float4 frag(vertexOutput i):COLOR 
		{
		//VECTORS DEFINITION
		float3 normalDirection = i.normalDir; // we define a float3 normalDirection, and set it to the object normal dir
		float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz); // a view direction, that is the position of the camera in world space, minus the object position, this way we know the distance to the camera and if its facing the object, normalized.
		float3 lightDirection;
		float atten;
		
		if(_WorldSpaceLightPos0.w  == 0.0){//for directional Lights - if and else are heavy to calculate, avoid using it
		
		atten = 1.0;
		lightDirection = normalize (_WorldSpaceLightPos0.xyz);// for lightDirection, we simply normalize the position of the light0 in worldspace
		}
		else{
		float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz; // We get the light facing the object
		float distance = length(fragmentToLightSource); // we get the distance from the light to the object
		atten = 1/distance; //we decrease the attenuation as the light gets closer of the object
		lightDirection = normalize(fragmentToLightSource); // we normalize this so we get a 1.0 vector
		
		}
		
		//LIGHTING
		float3 diffuseReflection = atten * _LightColor0.rgb  * max(0, dot (lightDirection, normalDirection));
		float3 specularReflection = atten * _LightColor0.rgb * max(0, dot (lightDirection, normalDirection)) * pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
		
		//Rim Lighting
		float rim = _RimStrenght * (1 - saturate(dot(normalize(viewDirection), normalDirection)));// for rim, we do a dot product of the view direction by the normal direction, we clamp it with a saturate and subtract by minus 1 to invert its value. so its brighter at the edge and dark inside face
		float3 rimLighting = atten * _LightColor0.rgb * _RimColor * saturate(dot(normalDirection, lightDirection)) * pow (rim, _RimPower); //after that we multiply by the rim color, the attenuation value, the light color, and we also mask out the part thats not being iluminated with the dot of light dir by normal dir, than we elevate to the power of rim
		float3 lightFinal = rimLighting + diffuseReflection + specularReflection +  UNITY_LIGHTMODEL_AMBIENT; // finally we add all the lights toggeter also, the unity lightmodel ambient
		
		return float4(lightFinal * _Color.xyz, 1.0); 
		}
																	
		ENDCG
		}
		
	} 
	FallBack "Diffuse"
}
