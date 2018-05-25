// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.
 
Shader "Custom/Surf_Paint" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    [HDR]_Emissive ("Emissive Color", Color) = (0,0,0,0)
    _isEmissive("Intensity",  Range(0, 10)) = 0 
    _PaintMap("PaintMap", 2D) = "white" {} // texture to paint on
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150
    Cull off
 
CGPROGRAM
#pragma surface surf Lambert noforwardadd
 
sampler2D _MainTex;
sampler2D _PaintMap;
fixed4 _Emissive;
half _isEmissive;
 
struct Input {
    float2 uv_MainTex;
    float2 uv2_PaintTex;
};
 
void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    half4 paint = tex2D(_PaintMap, IN.uv2_PaintTex);
    c *= (paint * paint);
    //c *= _Glow;
    o.Albedo = c.rgb;
    o.Alpha = c.a;
    o.Emission = _isEmissive * c.a * _Emissive;
    //o.Emission = -c.a;
    //fixed4 = max(0,_Glow)
}
ENDCG
}
 
Fallback "Mobile/Diffuse"
}
 