Shader "AndreTrainning/Tutorial7NormalMap" {
	Properties
	{
		_Color("Color Tint",Color) = (1.0,1.0,1.0,1.0)
		_MainTexture("Difuse",2D) = "White" {}//Textures are 2d, and the name must come in between "" before the {}
		_BumpMap("Bump",2D) = "Bump" {}//This will hold the normal map
		_BumpDepth("Bump Depth", Range (-2.0,2.0)) = 1.0
		_SpecularColor("Specular Color",Color) =(1.0,1.0,1.0,1.0)
		_Shininess("SpecularPower", float) = 1.0 //Spec power is called shininess in unity
		//FORGOTTEN ONES
		_RimColor("Rim Color", Color) = (1.0,1.0,1.0,1.0)
		_RimPower("Rim Power", Range(0.1, 10.0)) = 3.0
		
		}
	SubShader{
		Pass{
		TAGS{"LightMode" = "ForwardBase"}//The tag starts and ends in this line, its light mode and not lighting 

		CGPROGRAM //Called CGPROGRAM to start cg
		#pragma vertex vert
		#pragma fragment frag

		//User Defined Variables
		uniform float4 _Color;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;// ST - Super Transformations
		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;// ST - Super Transformations
		uniform float4 _SpecularColor;
		uniform float _Shininess; // Spec power is called shininsess in unity
		uniform float _BumpDepth;

		uniform float4 _RimColor;
		uniform float _RimPower;

		//Unity Defined Variables
		uniform float4 _LightColor0; // Its LightColor, and not ColorLight

		//Struct
		struct vertexInput
		{
		float4 vertex : POSITION;
		float4 texcoord : TEXCOORD0; // Its texcoord not difuse
		float3 normal : NORMAL;
		//for normal mapping we also need to pass a tangent, in order to rotate the normals based on the normal map
		float4 tangent : TANGENT;

		};
		struct vertexOutput
		{
		float4 pos : SV_POSITION;// SV_POSITION SV for SAMPLER VERTEX
		float4 tex : TEXCOORD0;
		float4 posWorld : TEXCOORD1;
		float3 normalWorld : TEXCOORD2;
		float3 tangentWorld : TEXCOORD3;
		float3 binormalWorld : TEXCOORD4;
		};
		//Vertex Shader
		vertexOutput vert(vertexInput v) {
		vertexOutput o;// We call a vertex output and not an input, and than we define the things for o

		//We must calculate the World Normal, Binormal and Tangent
		o.normalWorld = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz);
		o.tangentWorld = normalize(mul(_Object2World, v.tangent).xyz);//We are passing as a float 3 instead of 4
		o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w);//Binormal is a 90 rotation between the Normal and the Tangent, so we get the 3 axis in order to rotate the normal by pixel or fragment


		o.posWorld = mul(_Object2World, v.vertex);// We get the world position, byt multipling the position by the _Object2World variable, its a 4x4 matrix
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // v.vertex, not just vertex

		o.tex = v.texcoord; // we pass the texture coordinates as the tex component.

		return o;
		}


		//Pixel Shader
		//We need to frist calculate the texture maps before the lightining, since we will use a map


		float4 frag(vertexOutput i) :COLOR
		{

		float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz); //To get the direction the camera is facing, we subtract its position from the object and normalize
		float3 lightDirection;
		float atten;

		if (_WorldSpaceLightPos0.w == 0.0) {// If the w component its 0, it means its a spot light or a directional light
		atten = 1.0;
		lightDirection = normalize(_WorldSpaceLightPos0.xyz);
		}
		else {
		float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz; //we get the direction the light is facing
		float distance = length(fragmentToLightSource); // length from light to object
		atten = 1 / distance;
		lightDirection = normalize(fragmentToLightSource);

		}

		//Texture map

		float4 tex = tex2D(_MainTexture, i.tex.xy * _MainTexture_ST.xy + _MainTexture_ST.zw); // we declare texture and we pass the variable as a sampler 2d, and we unwrap it by passing the uv coordinates, multiplied by the ..._RT.xy and add the .zw this way the tiling works
		float4 texN = tex2D(_BumpMap, i.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);

		//UnPack Normals Function
		float3 localCoords = float3(2.0 * texN.ag - float2(1.0, 1.0), 0.0);//Unity stores the normal map in the alpha and green channel to save space, thats y we need to doble the texN.ag, xy, and subtract 1 from each, this way we fall in the -1 and 1 range, instead of 0 to 1
		//option1 = localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords);
		//option2 = locaclCoord.z = 1.0;
		localCoords.z = _BumpDepth;

		//normal transpose matrix
		//We now create a matrix to be used to transform the normal by multiplying it by the matrix
		float3x3 local2WorldTranspose = float3x3 (
			i.tangentWorld,
			i.binormalWorld,
			i.normalWorld
			);
		//Calculate normal direction - so now we will calculate the new normal direction
		float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose)); //For the new normal, we must multiply the world transpose matrix by the local coords stored in the normal map.


		//Lighting

		float3 diffuseReflection = atten * _LightColor0.rgb * max(0, dot(lightDirection, normalDirection));
		float3 specularReflection = diffuseReflection * _SpecularColor.rgb * pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);

		//Rim Lighting
		float rim = 1 - saturate(dot(viewDirection, normalDirection));
		float3 rimLighting = saturate(dot(normalDirection, lightDirection)) * _RimColor.xyz * _LightColor0.xyz * pow(rim, _RimPower);// we use the difuse lighting to mask the rim 		
		//float3 lightFinal =  UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseReflection + specularReflection + rimLighting; // With ambient light
		float3 lightFinal = diffuseReflection + specularReflection + rimLighting;



		return float4(tex.rgb * lightFinal * _Color.xyz, 1.0); // we return a float 4, so we multiply the texture by the light final and the main tint, we add a 1.0 in the end so its a float 4
		}
		ENDCG
			} 

			Pass {
			TAGS{ "LightMode" = "ForwardAdd" }//Its a ForwardAdd since its a second pass
				Blend One One
				CGPROGRAM //Called CGPROGRAM to start cg
#pragma vertex vert
#pragma fragment frag

						  //User Defined Variables
				uniform float4 _Color;
			uniform sampler2D _MainTexture;
			uniform float4 _MainTexture_ST;// ST - Super Transformations
			uniform sampler2D _BumpMap;
			uniform float4 _BumpMap_ST;// ST - Super Transformations
			uniform float4 _SpecularColor;
			uniform float _Shininess; // Spec power is called shininsess in unity
			uniform float _BumpDepth;

			uniform float4 _RimColor;
			uniform float _RimPower;

			//Unity Defined Variables
			uniform float4 _LightColor0; // Its LightColor, and not ColorLight

										 //Struct
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0; // Its texcoord not difuse
				float3 normal : NORMAL;
				//for normal mapping we also need to pass a tangent, in order to rotate the normals based on the normal map
				float4 tangent : TANGENT;

			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;// SV_POSITION SV for SAMPLER VERTEX
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalWorld : TEXCOORD2;
				float3 tangentWorld : TEXCOORD3;
				float3 binormalWorld : TEXCOORD4;
			};
			//Vertex Shader
			vertexOutput vert(vertexInput v) {
				vertexOutput o;// We call a vertex output and not an input, and than we define the things for o

							   //We must calculate the World Normal, Binormal and Tangent
				o.normalWorld = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz);
				o.tangentWorld = normalize(mul(_Object2World, v.tangent).xyz);//We are passing as a float 3 instead of 4
				o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w);//Binormal is a 90 rotation between the Normal and the Tangent, so we get the 3 axis in order to rotate the normal by pixel or fragment


				o.posWorld = mul(_Object2World, v.vertex);// We get the world position, byt multipling the position by the _Object2World variable, its a 4x4 matrix
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // v.vertex, not just vertex

				o.tex = v.texcoord; // we pass the texture coordinates as the tex component.

				return o;
			}


			//Pixel Shader
			//We need to frist calculate the texture maps before the lightining, since we will use a map


			float4 frag(vertexOutput i) :COLOR
			{

				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz); //To get the direction the camera is facing, we subtract its position from the object and normalize
				float3 lightDirection;
				float atten;

				if (_WorldSpaceLightPos0.w == 0.0) {// If the w component its 0, it means its a spot light or a directional light
					atten = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else {
					float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz; //we get the direction the light is facing
					float distance = length(fragmentToLightSource); // length from light to object
					atten = 1 / distance;
					lightDirection = normalize(fragmentToLightSource);

				}

				//Texture map

				//float4 tex = tex2D(_MainTexture, i.tex.xy * _MainTexture_ST.xy + _MainTexture_ST.zw); // we declare texture and we pass the variable as a sampler 2d, and we unwrap it by passing the uv coordinates, multiplied by the ..._RT.xy and add the .zw this way the tiling works
				float4 texN = tex2D(_BumpMap, i.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);

				//UnPack Normals Function
				float3 localCoords = float3(2.0 * texN.ag - float2(1.0, 1.0), 0.0);//Unity stores the normal map in the alpha and green channel to save space, thats y we need to doble the texN.ag, xy, and subtract 1 from each, this way we fall in the -1 and 1 range, instead of 0 to 1
																				   //option1 = localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords);
																				   //option2 = locaclCoord.z = 1.0;
				localCoords.z = _BumpDepth;

				//normal transpose matrix
				//We now create a matrix to be used to transform the normal by multiplying it by the matrix
				float3x3 local2WorldTranspose = float3x3 (
					i.tangentWorld,
					i.binormalWorld,
					i.normalWorld
					);
				//Calculate normal direction - so now we will calculate the new normal direction
				float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose)); //For the new normal, we must multiply the world transpose matrix by the local coords stored in the normal map.


																							//Lighting

				float3 diffuseReflection = atten * _LightColor0.rgb * max(0, dot(lightDirection, normalDirection));
				float3 specularReflection = diffuseReflection * _SpecularColor.rgb * pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);

				//Rim Lighting
				float rim = 1 - saturate(dot(viewDirection, normalDirection));
				float3 rimLighting = saturate(dot(normalDirection, lightDirection)) * _RimColor.xyz * _LightColor0.xyz * pow(rim, _RimPower);// we use the difuse lighting to mask the rim 		
																																			 //float3 lightFinal =  UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseReflection + specularReflection + rimLighting; // With ambient light
				float3 lightFinal = diffuseReflection + specularReflection + rimLighting;



				return float4(lightFinal * _Color.xyz, 1.0); // we return a float 4, so we multiply the texture by the light final and the main tint, we add a 1.0 in the end so its a float 4
			}
				ENDCG
				
		}	
	}
	Fallback "Specular"//In " "
}