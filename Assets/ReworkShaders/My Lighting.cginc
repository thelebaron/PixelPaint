//lighting include

#pragma vertex vert
#pragma fragment frag fullforwardshadows
#pragma multi_compile_builtin
#pragma multi_compile_fwdbase
#pragma fragmentoption ARB_precision_hint_fastest // ARB_precision_hint_nicest
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "UnityStandardBRDF.cginc"
#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "UnityPBSLighting.cginc"

//#pragma fullforwardshadows


#include "UnityCG.cginc"
#include "AutoLight.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    float2 uv0 : TEXCOORD0;
    float2 uv1 : TEXCOORD1;
    float3 normal : TEXCOORD2;
    float3 worldPos : TEXCOORD3;
    LIGHTING_COORDS(4,5)

};

struct appdata {
    float4 position : POSITION;
    float2 texcoord : TEXCOORD0;
    float2 texcoord1 : TEXCOORD1;
    float3 normal : NORMAL;
};

sampler2D _PaintMap;
sampler2D _MainTex;
float4 _MainTex_ST;
half _MainTexBias;


v2f vert(appdata v) {
    v2f o;
    o.pos = UnityObjectToClipPos(v.position);
    o.worldPos = mul(unity_ObjectToWorld, v.position);
    o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
    TRANSFER_VERTEX_TO_FRAGMENT(o);
    o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw ;// lightmap uvs
    
    o.normal = UnityObjectToWorldNormal(v.normal); //conv to world space
    return o;
}


half4 frag(v2f i) : SV_TARGET{ //COLOR

    float attenuation = LIGHT_ATTENUATION(i);
    half4 albedo = tex2Dbias(_MainTex, half4(i.uv0.x, i.uv0.y, 0.0, _MainTexBias));
    half4 paint = tex2Dbias(_PaintMap, half4(i.uv1.x, i.uv1.y, 0.0, _MainTexBias));
    albedo *= (paint * paint); // add paint to main;

    float3 lightDir = _WorldSpaceLightPos0.xyz;
    float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
    float3 lightColor = _LightColor0.rgb;
    float3 diffuse = lightColor * DotClamped(lightDir, i.normal);
    diffuse = albedo * lightColor * DotClamped(lightDir, i.normal);
    return half4(diffuse, 1);

    //return albedo * attenuation ;
}
    

#endif