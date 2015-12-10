Shader "AndreTrainning/Tutorial1Flat" { //Name of the shader
	Properties {
	_Color ("Color" , Color) = (1.0,1.0,1.0,1.0) // We have the variable name, with an underscore to be changed in unity, than we set a name to display in "", than set the type of controller, and pass the value. It can be float, range, color, vector, cube...	
	}
	SubShader {// The shader itself, we could have multiple ones for each type of machine that will run it, xbox, pc, vita, mobile...
		Pass { // The render pass, kinda like layers, but every time is a draw call pass, used to blend results in case we have multiple pass, we use this for the outline vertex shader
			CGPROGRAM // Line to start writing the cg code
	//Pragmas    - Struction to make the things to work, we need to pass the name of our vertex and fragment function to be quicked off
	#pragma vertex vert
	#pragma fragment frag
	
	//User Defined Variables   - Where we define our variables
	uniform float4 _Color; // Same name as the property, we use the uniform to define the variable and give an initial value good to keep the habit of using it
	
	//Base Input Structs    - Structs so we can comunicate with unity and the vertex 
	
	struct vertexInput {
		float4 vertex : POSITION; // We basically declare a variable vertex of type float4, and we use a semantic ( : POSITION), we basically pass the position information of the vertex to this variable,
		// there is many types of semantics, TEXCOORD, NORMAL, .... 
	};//<--- VERY important this ; in the struct definition
	struct vertexOutput { // What we output from the vertex function to be used by the fragment function, we have to pass at least the vertex position
		float4 pos : SV_POSITION; // We pass the vertex position, it makes some matrix vodoo with the camera to be properly displayed, will rewrite better later, right now stick to it, we need the SV_ cuz they changed unity to use it
	};// <-- THIS one too
	
	
	//Vertex Functions    - the code thats executed by every vertex, than we do the fragment function
	
	vertexOutput vert(vertexInput v){ // We call the struct we defined, by doing so we exectue the struct vertexInput and give it a name of v, 
		vertexOutput o; // We need to declare it like this because its a whole class and not just a variable, so we cant do float4 o. but we need to pass the struct and the name we want to call it
		//WE ALWAYS NEED THE FOLLOWING LINE IN EVERY SHADER
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);// This is the vodoo mentioned, is basically translating the object vertex position into a unity matrix so it can define its position in the world,
		//we use the mul function to multiply between different matrices, used to change spaces, into world space, object space, tangent space, camera space....
		return o; // Than we return its value
	
	}
	
	//Fragment Functions    - the code thats executed by every pixel
	
		float4 frag( vertexOutput i) : COLOR // We define a vertexoutput struct called frag as a float4 because we are using the semantics :COLOR, thats what will be sent to the pixels (fragments)
		{
		return _Color; //it will return the color variable we define in the beggining
		}
				ENDCG	//Line where we end the cg code
		
		}
		
				
	} 
	//fallback commented out during shader development
	FallBack "Diffuse" //The subshade that will run in case it cant run the shader
}
 