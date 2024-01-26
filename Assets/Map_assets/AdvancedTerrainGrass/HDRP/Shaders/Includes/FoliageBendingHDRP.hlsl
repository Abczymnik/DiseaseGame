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

float4 SmoothCurve( float4 x ) {   
	return x * x *( 3.0f - 2.0f * x );   
}
float4 TriangleWave( float4 x ) {   
    return abs( frac( x + 0.5f ) * 2.0f - 1.0f );   
}
float4 SmoothTriangleWave( float4 x ) {   
    return SmoothCurve( TriangleWave( x ) );   
}

    
void FoliageBending_float
(
	float3 PositionOS,
	float3 NormalOS,
	float4 VertexColor,

	float4 WindMultiplier,
	bool SampleWindAtPivot,
	float WindLOD,
	float Stretchiness,
	float NormalDisplacement,

	float2 MinMaxScales,
	float4 HealthyColor,
	float4 DryColor,

	float Clip,
	float NormalToUpNormal,

	float Time,

	out real3 outPositionOS,
	out real3 outNormalOS,
	out real4 outInstanceColor
)
{
	outPositionOS = PositionOS;
	outNormalOS = NormalOS;
	outInstanceColor = 1;


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
        PositionOS.xyz = lerp(PositionOS.xyz, float3(0, 0, 0), (1.0 - fade).xxx);
	
	//  Wind animation
		#define ibendAmount VertexColor.a
		#define iphase VertexColor.rr

		float originalLength = length(PositionOS);

		float3 positionWS = GetAbsolutePositionWS(TransformObjectToWorld(PositionOS.xyz));

		float3 samplePosWS = (SampleWindAtPivot) ? GetAbsolutePositionWS(pivot) : positionWS;

	    float4 wind = SAMPLE_TEXTURE2D_LOD(_AtgWindRT, sampler_AtgWindRT, samplePosWS.xz * _AtgWindDirSize.w + iphase * WindMultiplier.z, WindLOD);
	    
	    float3 windDir = _AtgWindDirSize.xyz;
	    windDir = TransformWorldToObjectDir(windDir);

	    float windStrength = ibendAmount * _AtgWindStrengthMultipliers.y * WindMultiplier.x;
	    wind.r = wind.r  *  (wind.g * 2.0f - 0.243f);
	    windStrength *= wind.r;

	    const float fDetailAmp = 0.1;
		const float fBranchAmp = 0.3;
		float2 variations = abs(frac( float2(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].y)));
		float fObjPhase = dot(variations, float2(1,1) );

		float3 offset = 0;

	//	Primary bending
		offset = VertexColor.a * windDir * (wind.r * WindMultiplier.x);

		float2 vWavesIn = Time.xx + float2(0, fObjPhase  +  (VertexColor.r) ); // + instanceColor.a) );
		float4 vWaves = frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0;
		vWaves = SmoothTriangleWave( vWaves );
		float2 vWavesSum = vWaves.xz + vWaves.yw;
	//	Edge Flutter
		float3 bend = VertexColor.g * fDetailAmp * NormalOS.xyz;
	//  Secondary bending
//		offset += ((vWavesSum.xyx * bend)  + (VertexColor.b * fBranchAmp * windDir * vWavesSum.y * _WindMultiplier.y)) * wind.r;
// wind.r bugs?
		offset += ( 
			vWavesSum.xyx * bend 
			+ VertexColor.b * fBranchAmp * windDir * vWavesSum.y * WindMultiplier.y
		) * windStrength;


//offset *= 10 * wind.r;

	//	Apply Wind Animation
		#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
			outPositionOS -= offset;
			outNormalOS.xz -= offset.xz * PI * NormalDisplacement;
		#else 
			outPositionOS += offset;
			outNormalOS.xz += offset.xz * PI * NormalDisplacement;
		#endif

	//	Preserve length
		outPositionOS = lerp(normalize(outPositionOS) * originalLength, outPositionOS, Stretchiness.xxx);

	#endif
}