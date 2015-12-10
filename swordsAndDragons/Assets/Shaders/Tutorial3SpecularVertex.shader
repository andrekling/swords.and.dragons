Shader "AndreTrainning/Tutorial3SpecularVertex" {
	Properties {
		_Color("Color", Color) = (1.0,1.0,1.0,1.0)
		_SpecularColor("Specular Color", Color) = (1.0,1.0,1.0,1.0)
		_Shininess("Spec Power", float) = 10 //Unity calls spec power shininess for external lighting models
		
	}
	SubShader {
		Tags{ "LightMode" = "ForwardBase"} //1
		Pass{
	
			CGPROGRAM
			//Pragmas				
		#pragma vertex vert
		#pragma fragment frag

	//User defined Variables
	uniform float4 _Color;
	uniform float4 _SpecularColor;
	uniform float _Shininess;
	
	//Unity defined Variables
	uniform float4 _LightColor0;
	//float4x4 _object2world;
	//float4x4 _world2object;
	
	
	//Structs
	struct VertexInput
	{
	float4 vertex :POSITION;
	float3 normal :NORMAL;
	float4 col :COLOR;
	
	};
	struct VertexOutput
	 {
	 
	 float4 pos :SV_POSITION;
	 float4 col :COLOR;
	 
	 }; 
	
	//Vertex program
	VertexOutput vert(VertexInput v)
	{
	VertexOutput o;
	
	//Vectors
	float3 normalDirection = normalize(mul(float4(v.normal,0.0), _World2Object).xyz); //Vector pointing from the normal of object out
	//We must multiply the vertex normal, that is a float 3, therefore we need to cast it as a float4, so we can calculate the matrix of multiplying by _World2Object, but we only get the xyz components, and normalize it
	float3 viewDirection = normalize(float3(float4(_WorldSpaceCameraPos.xyz,0.0) - mul(_Object2World, v.vertex).xyz ));//Vector from camera pointing to object, Must be to objectSpace and not UnityMatrx_MVP
	//To get the camera position, we must subtract the world position of the camera, by the object world position, but since the matrix calculations are float4 we must add a component, but we declared as float3, so we cast as float3 and normalize it
	float3 lightDirection ; //Vector from light pointing to object
	float atten = 1.0; // Attenuation values for light, to be used in the future
	
	//LIGHTNING
	lightDirection = normalize(_WorldSpaceLightPos0.xyz); //We simple normalize the position in world space of the light, variable set by unity
	float3 diffReflection = atten * _LightColor0.xyz * max(0, dot(lightDirection, normalDirection)); 
	float3 specReflection = atten * _SpecularColor.rgb * max(0, dot(lightDirection, normalDirection)) * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)),_Shininess);//Multiply by light
	float3 lightFinal = diffReflection + specReflection + UNITY_LIGHTMODEL_AMBIENT;
	//o.col = float4(,1.0); //EMPTY COL - We add 1 to the alpha so it works with 4 values in the float4
	//o.col = float4(normalDirection,1.0); //Normal Component
	//o.col = float4(diffReflection,1.0); //Difuse Component
	//o.col = float4(specReflection,1.0); //Reflection Component
	o.col = float4(lightFinal * _Color,1.0); // Final light  
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);// Multiply the matrices
	return o;
	}
	
	
	//Fragment program
		float4 frag(VertexOutput i):COLOR
		{
		return i.col;
		
		}
		
				
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
