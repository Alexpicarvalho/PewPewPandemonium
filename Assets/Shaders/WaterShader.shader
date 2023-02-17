Shader "Custom/Unlit/WaterShader" {
Properties {
[HideInInspector] _BaseMap("Albedo", 2D) = "white" {}
_Depth("Depth", Float) = 9
[HDR]_ShallowColor("Shallow Color", Color) = (0.27, 0.58, 0.76, 0.57)
[HDR]_DeepColor("Deep Color", Color) = (0, 0.04, 0.17, 0.92)
[HDR]_FoamColor("Foam Color", Color) = (1, 1, 1, 1)
_Foam("Foam: Amount(x) Scale(y) Cutoff(z) Speed(w)", Vector) = (1, 120, 5, 1)
_Refraction("Refraction: Strength(x) Scale(y) Speed(z)", Vector) = (0.002, 40, 1)
_Wave("Wave: Velocity(x, y) Intensity(z)", Vector) = (1, 1, 0.2)
}

SubShader {
LOD 100

Tags {
"Queue" = "Transparent"
"RenderType" = "Transparent"
"RenderPipeline" = "UniversalPipeline"
"IgnoreProjector" = "True"
}

Pass {
Tags {
"LightMode" = "UniversalForward"
}

HLSLPROGRAM
#pragma vertex WaterVertex
#pragma fragment WaterFragment
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

#pragma multi_compile_instancing // GPU instancing

//////////////////////
// Depth and Opaque //
//////////////////////
TEXTURE2D_X(_CameraOpaqueTexture); SAMPLER(sampler_CameraOpaqueTexture); float4 _CameraOpaqueTexture_TexelSize;
TEXTURE2D_X(_CameraDepthTexture); SAMPLER(sampler_CameraDepthTexture);

float SampleSceneDepth(float2 UV) {
return SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(UV)).r;
}

float3 SampleSceneColor(float2 UV) {
return SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, UnityStereoTransformScreenSpaceTex(UV)).rgb;
}

////////////////////
// Gradient Noise // Google "gradient noise shader graph"
////////////////////
float2 GradientNoiseDir(float2 p) {
p = p % 289;
float x = (34 * p.x + 1) * p.x % 289 + p.y;
x = (34 * x + 1) * x % 289;
x = frac(x / 41) * 2 - 1;
return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

float GradientNoise(float2 p) {
float2 ip = floor(p);
float2 fp = frac(p);
float d00 = dot(GradientNoiseDir(ip), fp);
float d01 = dot(GradientNoiseDir(ip + float2(0, 1)), fp - float2(0, 1));
float d10 = dot(GradientNoiseDir(ip + float2(1, 0)), fp - float2(1, 0));
float d11 = dot(GradientNoiseDir(ip + float2(1, 1)), fp - float2(1, 1));
fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
}

///////////
// Water //
///////////
float2 WaterRefractedUV(float2 UV, float Strength, float Scale, float Speed, float2 ScreenUV) {
float2 tiledAndOffsettedUV = UV * Scale + (_Time.y * Speed);
float gNoiseRemapped = GradientNoise(tiledAndOffsettedUV) * 2 * Strength;
return ScreenUV + gNoiseRemapped;
}

float WaterDepthFade(float Depth, float4 ScreenPosition, float Distance) {
return saturate((Depth - ScreenPosition.w) / Distance);
}

struct Attributes {
float4 positionOS : POSITION; // vertex position in object space
float2 texCoord : TEXCOORD0;
UNITY_VERTEX_INPUT_INSTANCE_ID // GPU instancing
};

struct Varyings {
float4 positionCS : SV_POSITION; // vertex position in camera space
float2 texCoord : TEXCOORD0;
float4 screenPos : TEXCOORD1;
};

TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_ST;

float _Depth;
half4 _DepthColor;
half4 _ShallowColor;
half4 _DeepColor;
half4 _FoamColor;
float4 _Foam;
float3 _Refraction;
float3 _Wave;

Varyings WaterVertex(Attributes atb) {
UNITY_SETUP_INSTANCE_ID(atb); // GPU instancing
Varyings vriOut;
vriOut.texCoord = TRANSFORM_TEX(atb.texCoord, _BaseMap);

float3 positionWS = TransformObjectToWorld(atb.positionOS.xyz); // vertex position in world space
float3 displacement = float3(0, GradientNoise(positionWS.xz + _Time.y * _Wave.xy) * _Wave.z, 0);
positionWS += displacement;
vriOut.positionCS = mul(UNITY_MATRIX_VP, float4(positionWS, 1));
vriOut.screenPos = ComputeScreenPos(vriOut.positionCS);

return vriOut;
}

half4 WaterFragment(Varyings vri) : SV_Target {
float2 screenUV = vri.screenPos.xy / vri.screenPos.w;
float zEye = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);

half4 depthColor = lerp(_ShallowColor, _DeepColor, WaterDepthFade(zEye, vri.screenPos, _Depth));

float foam = WaterDepthFade(zEye, vri.screenPos, _Foam.x) * _Foam.z;
float2 foamUV = vri.texCoord * _Foam.y + (_Foam.w * _Time.y);
float gNoise = GradientNoise(foamUV) + 0.5;
half4 foamValue = step(foam, gNoise) * _FoamColor.a;

half4 waterColor = lerp(depthColor, _FoamColor, foamValue);

float2 refractedUV = WaterRefractedUV(vri.texCoord, _Refraction.x, _Refraction.y, _Refraction.z, screenUV);
half4 refractedSceneColor = half4(SampleSceneColor(refractedUV), 1);

return lerp(refractedSceneColor, waterColor, waterColor.a);
}
ENDHLSL
}
}
}