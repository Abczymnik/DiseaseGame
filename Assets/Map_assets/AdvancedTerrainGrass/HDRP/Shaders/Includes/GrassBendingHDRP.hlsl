// Global Inputs
CBUFFER_START(AtgGrass)
    float4 _AtgTerrainShiftSurface;
    float4 _AtgWindDirSize;
    float4 _AtgWindStrengthMultipliers;
    float4 _AtgSinTime;
    float4 _AtgGrassFadeProps;
    float4 _AtgGrassShadowFadeProps;
    float3 _AtgSurfaceCameraPosition;
CBUFFER_END
TEXTURE2D(_AtgWindRT); SAMPLER(sampler_AtgWindRT);


//	Simple random function
inline float nrand(float2 pos) {
	return frac(sin(dot(pos, half2(12.9898f, 78.233f))) * 43758.5453f);
}



void GrassBending_float
(
	float3 PositionOS,
	float3 NormalOS,
	float4 VertexColor,

	float4 WindMultiplier,
	bool SampleWindAtPivot,
	float WindLOD,
	float NormalDisplacement,

	float ScaleMode,

	float2 MinMaxScales,
	float4 HealthyColor,
	float4 DryColor,

	float Clip,
	float NormalToUpNormal,

	out real3 outPositionOS,
	out real3 outNormalOS,
	out real4 outInstanceColor
)
{
	outPositionOS = PositionOS;

	#if defined(SHADERGRAPH_PREVIEW)
		outNormalOS = NormalOS;
		outInstanceColor = 1;
	#else

	//	We leave this in the setup func as this will quiet the compiler.
		//float InstanceScale = 1.0f;
		//int TextureLayer = 0;
		// #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		// 	InstanceScale = frac(unity_ObjectToWorld[3].w);
		// 	TextureLayer = unity_ObjectToWorld[3].w - InstanceScale;
		// 	InstanceScale *= 100.0f;
		// 	//#if defined(_NORMAL)
		// 	//	terrainNormal = unity_ObjectToWorld[3].xyz;
		// 	//#endif
		// 	unity_ObjectToWorld[3] = float4(0, 0, 0, 1.0f);
		// #endif

		const float scale = InstanceScale;
		const float3 pivot = UNITY_MATRIX_M._m03_m13_m23;

        const float3 dist = 
    	#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING == 0)
			pivot - _WorldSpaceCameraPos.xyz;
		#else
			pivot;
		#endif

        const float SqrDist = dot(dist, dist);
    
    //  Calculate far fade factor
    	#if (SHADERPASS == SHADERPASS_SHADOWS)
            // TODO: Check why i can't revert this as well? Clip?
            float fade = 1.0f - saturate((SqrDist - _AtgGrassShadowFadeProps.x) * _AtgGrassShadowFadeProps.y);
        #elif (SHADERPASS == SHADERPASS_DEPTH_ONLY)
            float fade = saturate(( _AtgGrassFadeProps.x - SqrDist) * _AtgGrassFadeProps.y);
        #else
            float fade = saturate(( _AtgGrassFadeProps.x - SqrDist) * _AtgGrassFadeProps.y);
        #endif
    //  Cull based on far culling distance
        if (fade == 0.0f) {
            outPositionOS /= fade;
            return;
        }

	//	Get some random value per instance
		float random = nrand(  float2(scale, 1.0 - scale) );

	//  Calculate near fade factor / reversed!
        const float smallScaleClipping = saturate(( SqrDist - _AtgGrassFadeProps.z) * _AtgGrassFadeProps.w);
        float clip = (random < Clip)? 1 : 0;
        clip = 1.0f - smallScaleClipping * clip;
    //  Cull based on near culling distance
        if (clip == 0.0f) {
            outPositionOS /= clip;
            return;
        }
        fade *= clip;
    
    //  Apply fading
    	if(!ScaleMode) {
    		PositionOS.xyz = lerp(PositionOS.xyz, float3(0, 0, 0), (1.0 - fade).xxx);	
    	}
    	else {
    		PositionOS.xz = lerp(PositionOS.xz, float2(0, 0), (1.0 - fade).xx);
    	}
        
	
	//  Wind animation
		#if defined(_BENDINGMODE_BLUE)
	         #define ibendAmount VertexColor.b // * instanceColor.a
	         #define ivocclusion VertexColor.b
	    #else
	        #define ibendAmount VertexColor.a // * instanceColor.a
	        #define ivocclusion VertexColor.a
	    #endif
	    #define iphase VertexColor.rr

	    float3 positionWS = GetAbsolutePositionWS(TransformObjectToWorld(PositionOS.xyz));

	    float3 samplePosWS = (SampleWindAtPivot) ? GetAbsolutePositionWS(pivot) : positionWS;

	    half4 wind = SAMPLE_TEXTURE2D_LOD(_AtgWindRT, sampler_AtgWindRT, samplePosWS.xz * _AtgWindDirSize.w + iphase * WindMultiplier.z, WindLOD);
	    half3 windDir = _AtgWindDirSize.xyz;
	    windDir = TransformWorldToObjectDir(windDir);

	    float windStrength = ibendAmount * _AtgWindStrengthMultipliers.x * WindMultiplier.x;
	    wind.r = wind.r  *  (wind.g * 2.0f - 0.243f);
	    windStrength *= wind.r;

	    #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
	    	PositionOS.xz -= windDir.xz * windStrength; // minus!
	    #else 
	    	PositionOS.xz += windDir.xz * windStrength; // plus!
	    #endif

	//  Add small scale jitter (HZD)
	    float3 disp = sin( 4.0f * 2.650f * (positionWS.x + positionWS.y + positionWS.z + _Time.y)) * NormalOS * float3(1.0f, 0.35f, 1.0f);
	    PositionOS += disp * windStrength * WindMultiplier.y;

		outPositionOS = PositionOS;
		#if defined(_NORMAL)
			NormalOS = TerrainNormal;
		#endif
		outNormalOS = lerp(NormalOS, float3(0,1,0), NormalToUpNormal.xxx);

	//  Do something to the normal. Sign looks fine.
	    outNormalOS = outNormalOS + disp * PI * NormalDisplacement;

	//  Set color variation

		float normalizedScale = (scale - _MinMaxScales.x) * _MinMaxScales.y;
	    normalizedScale = saturate(normalizedScale);
	    #if defined(GRASSUSESTEXTUREARRAYS) && defined(_MIXMODE_RANDOM)
	        outInstanceColor = lerp(HealthyColor, DryColor, nrand(pivot.zx));
	    #else
	        outInstanceColor = lerp(HealthyColor, DryColor, normalizedScale);
	    #endif

	#endif
}