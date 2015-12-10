Shader "Andre/VertFragTest" {
 //UNITY VARIABLES 
 Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_Push ("Outline Thikness", Range(-1, 1)) = 0
	}   
 
 
 SubShader
   {
	  Pass 
	  {
	  
	  Tags{ "LightMode" = "ForwardBase"}
		 
		 CGPROGRAM
 
		 #pragma vertex vert             
		 #pragma fragment frag
		 
		 #include "UnityCG.cginc" 
		 //we add the unityCG include to have the provided code


//VARIABLES DECLARATIONS
uniform half4 _Color;
uniform float4 _LightColor0;

float _Push;
//Need to study why to use a uniform.
 
  
    //INPUTS
		 struct vertInput
		 {
			float4 pos : POSITION; // the vertex position
			float3 nor : NORMAL; //the vertex normal
		 };  
 
		 struct vertOutput
		 {
		 float4 pos : SV_POSITION; // output the vertex position to be transformed into local pos
		float4 col : COLOR;
		 };
 //VERTEX PASS
 
 half3 vertexOffsetObjectSpace(appdata_full v) {
	return v.normal.xyz += _Push;;				
}
 
		 vertOutput vert(vertInput input)
		 {
			vertOutput o;
						
			float4 normal = float4(input.nor, 0.0);
			float3 n = normalize(mul(normal, _World2Object));//get the normal in local pos
			float3 l = normalize(_WorldSpaceLightPos0);
			
			float3 NdotL = max(0.0, dot(n,l));// Lambert light calculation
			float3 a = UNITY_LIGHTMODEL_AMBIENT * _Color;
			
			float3 d = NdotL * _LightColor0 * _Color;// multiply the light by the color of light and the color par
			o.col = float4(d + a ,1.0); // we add the alpha to it
			o.pos = mul(UNITY_MATRIX_MVP, input.pos);
			
			return o;
		 }
 //FRAG PASS
		 half4 frag(vertOutput input) : COLOR
		 {
			return  saturate(input.col); 
		 }
		 ENDCG
	  }
   }
}