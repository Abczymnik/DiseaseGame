void Inject_float(float3 Input, out real3 Output) 
{
	Output = Input;
}

#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
	#if defined(SHADER_API_GLCORE) || defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_METAL) || defined(SHADER_API_VULKAN) || defined(SHADER_API_PSSL) || defined(SHADER_API_XBOXONE)
		uniform StructuredBuffer<float4x4> GrassMatrixBuffer;
	#endif
#endif

float InstanceScale;
int TextureLayer;
#if defined(_NORMAL)
	float3 TerrainNormal;
#endif

void setup()
{

#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

	#ifdef unity_ObjectToWorld
		#undef unity_ObjectToWorld
	#endif
	#ifdef unity_WorldToObject
		#undef unity_WorldToObject
	#endif
	unity_ObjectToWorld = GrassMatrixBuffer[unity_InstanceID];

//	We leave this in the setup func as this will quiet the compiler.
	InstanceScale = frac(unity_ObjectToWorld[3].w);
	TextureLayer = unity_ObjectToWorld[3].w - InstanceScale;
	InstanceScale *= 100.0f;
	#if defined(_NORMAL)
		TerrainNormal = unity_ObjectToWorld[3].xyz;
	#endif
	unity_ObjectToWorld[3] = float4(0, 0, 0, 1.0f);

// 	Not correct but good enough to get the wind direction in object space
	unity_WorldToObject = unity_ObjectToWorld;
	//unity_WorldToObject[3] = float4(0, 0, 0, 1.0f);
	unity_WorldToObject._14_24_34 = 1.0f / unity_WorldToObject._14_24_34;
	unity_WorldToObject._11_22_33 *= -1;
#endif
}