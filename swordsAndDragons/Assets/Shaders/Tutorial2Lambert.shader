Shader "AndreTrainning/Tutorial2Lambert" {
	Properties {
		_Color("Color", Color) = (1.0,1.0,1.0,1.0) // Define the property for color
	}
	SubShader{
	//In order to the light to works in real time we need to pass the way we are calculating light as a tag
	
	Tags
	{
	"LightMode" = "ForwardBase" //We set the lighting mode to be forwards based 
	}
	
	Pass{
	
	
	CGPROGRAM
	// 1 - Pragmas - How the video card knows what to call
	#pragma vertex vert
	#pragma fragment frag
	
	//2 - User Defined Variables - defined by us
	uniform float4 _Color; //The variable we just created
	//3 - Unity Defined Variables - defined by unity
	uniform float4 _LightColor0;// The color of the light
	//Old unity variables needed - maybe also needed by max?
	
	//float4x4 _Object2World;//Matrix to be calculated the position of the vertex
	//float4x4 _World2Object;//Another matric to pass from world to object space
	//float4 _WorldSpaceLightPos0; //The position and orientation of the light 0
	
	//4 - Structs
	struct vertexInput{
		float4 vertex : POSITION; //We grab the vertex position and pass the POSITION semantic to it
		float3 normal : NORMAL; // we grab the vertex normal, its needed to calculate the light incidense on the object
		float4 col : COLOR; // we grab the vertex color to pass to the shader
	};
	struct vertexOutput{
		float4 pos : SV_POSITION; //We output as pos the position by using the SV_POSITION semantic
		float4 col : COLOR; //We output the vertex color
	};
	// 5 - Vertex Function
	vertexOutput vert(vertexInput v){
		vertexOutput o; // How we declare the entire function
		//We first need to convert the normals from world matrix to object space
						//WE HAVE TO HAVE THIS!!!!
		float3 normalDirection = normalize( mul(float4(v.normal,0.0), _World2Object).xyz ); // we multiply the normals by the 4x4 matrix, but since normal is a float3 we need to cast it as a
		//float4 so we add a 0.0 at the end, than we just pass .xyz to our float 3 normalDirection variable. 
		
		//normalize - makes everything into 0 to 1 range.
		//saturate - cuts everything that pass the 1 range and below 0
		
		//	CALCULATE LIGHT
		float3 lightDirection; // variable to holds the orientation of the light
		float atten = 1.0; // used to attenuate the light by the distance, not used in a directional light
		
		lightDirection = normalize(_WorldSpaceLightPos0.xyz); // normalize to reduce the artifacts by not having 0 values when rotating the light
		//Since we can have value of light that goes beyond the 1.0 range we need to clamp just what goes below 0, thats why we use the max function
		//float3 lambertLight = max(0.0, dot(lightDirection, normalDirection) ); // Just light no color
		
		float3 lambertLight = atten * _LightColor0.xyz * max(0.0, dot(lightDirection, normalDirection) ) + UNITY_LIGHTMODEL_AMBIENT.xyz; // Light with color of light component
		//We separate the color of the object from the light calculation in order to not tint the texture with gray when adding the ambient light in the end, its better to combine later
		float3 colorObjWithLight = lambertLight * _Color.rgb;
		
		//o.col = float4(v.normal, 1.0); //Passing the normal as a color
		//o.col = float4(lambertLight, 1.0); // The light without colors
		o.col = float4(colorObjWithLight, 1.0); // The light with object colors
		
		
		// --- We multiply the matrices by the unity matrix UNITY_MATRIX_MVP
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		
		
		return o;
	}
	
	// 6 - Fragment Program
	
	float4 frag(vertexOutput i):COLOR // We are passing the fragment as a color Semantics, we need the vertexOutput for this to work, and we call it as i
	{
	return i.col;// we return the i. color
	}
	
	ENDCG
	
	
	}
		
	} 
	//FallBack "Diffuse"
}
