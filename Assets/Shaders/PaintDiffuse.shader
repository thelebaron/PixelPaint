
Shader "Custom/PaintDiffuse" {
 
    Properties{
        _MainTex("Main Texture", 2D) = "white" {}
        _PaintMap("PaintMap", 2D) = "white" {} // texture to paint on
        _MainTexBias("Mip Bias (-1 to 1)", float) = -0.65

    }
 
        SubShader{
        Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
 
        Pass{
 
        Lighting Off
 
        CGPROGRAM
 
#pragma vertex vert
#pragma fragment frag fullforwardshadows
#pragma multi_compile_builtin
#pragma fragmentoption ARB_precision_hint_fastest // ARB_precision_hint_nicest
#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
//#pragma fullforwardshadows
 
 
#include "UnityCG.cginc"
#include "AutoLight.cginc"
 
    struct v2f {
        float4 pos : SV_POSITION;
        float2 uv0 : TEXCOORD0;
        float2 uv1 : TEXCOORD1;
        LIGHTING_COORDS(1,2)
 
    };
 
    struct appdata {
        float4 vertex : POSITION;
        float2 texcoord : TEXCOORD0;
        float2 texcoord1 : TEXCOORD1;
 
    };
 
    sampler2D _PaintMap;
    sampler2D _MainTex;
    float4 _MainTex_ST;
    half _MainTexBias;


    v2f vert(appdata v) {
        v2f o;
 
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
        TRANSFER_VERTEX_TO_FRAGMENT(o);
        o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw ;// lightmap uvs
        return o;
    }
 
    half4 frag(v2f o) : COLOR{

        float attenuation = LIGHT_ATTENUATION(o);
        
        //half4 main_color = tex2D(_MainTex, o.uv0); // main texture
        half4 main_color = tex2Dbias(_MainTex, half4(o.uv0.x, o.uv0.y, 0.0, _MainTexBias));
        //half4 paint = (tex2D(_PaintMap, o.uv1)); // painted on texture
        half4 paint = tex2Dbias(_PaintMap, half4(o.uv1.x, o.uv1.y, 0.0, _MainTexBias));
        main_color *= (paint * paint); // add paint to main;
        return main_color * attenuation ;
    }
        ENDCG
    }
    }
    FallBack "Legacy Shaders/Diffuse"
}