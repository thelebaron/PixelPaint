// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 
Shader "Forward/DiffuseNormalSpec" {
    Properties {
        _DiffuseTex ("Diffuse (RGB)", 2D) = "white" {}
        _NormalTex ("Normal (RGB)", 2D) = "bump" {}
        _SpecularTex ("Specular (R) Gloss (G)", 2D) = "gray" {}
    }
 
    SubShader {
 
        Pass {
            Name "ContentBase"
            Tags {"LightMode" = "ForwardBase"}
           
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fwdbase
                #pragma fragmentoption ARB_precision_hint_fastest
               
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"
               
                struct v2f
                {
                    float4  pos : SV_POSITION;
                    float2  uv : TEXCOORD0;
                    float3  lightDirT : TEXCOORD1;
                    float3  viewDirT : TEXCOORD2;
                    LIGHTING_COORDS(3,4)
                };
 
                v2f vert (appdata_tan v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord.xy;
                    TANGENT_SPACE_ROTATION;
                    o.lightDirT = mul(rotation, ObjSpaceLightDir(v.vertex));
                    o.viewDirT = mul(rotation, ObjSpaceViewDir(v.vertex));
                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }
               
                sampler2D _DiffuseTex;
                sampler2D _SpecularTex;
                sampler2D _NormalTex;
               
                float4 _LightColor0;
 
                float4 frag(v2f i) : COLOR
                {
                    float3 normal = normalize(tex2D(_NormalTex, i.uv).xyz * 2 - 1);
                    float NdotL = dot(normal, i.lightDirT);
                    float3 halfAngle = normalize(i.lightDirT + i.viewDirT);
                    float atten = LIGHT_ATTENUATION(i);

                    float4 result;
                    result.rgb = tex2D(_DiffuseTex, i.uv).rgb * NdotL * atten * _LightColor0 ;
                    result.a = 0;
                    return result;
                }
            ENDCG
        }
 
        Pass {
            Name "ContentAdd"
            Tags {"LightMode" = "ForwardAdd"}
            Blend One One
           
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fwdadd
                #pragma fragmentoption ARB_precision_hint_fastest
               
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"
               
                struct v2f
                {
                    float4  pos : SV_POSITION;
                    float2  uv : TEXCOORD0;
                    float3  lightDirT : TEXCOORD1;
                    float3  viewDirT : TEXCOORD2;
                    LIGHTING_COORDS(3,4)
                };
 
                v2f vert (appdata_tan v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord.xy;
                    TANGENT_SPACE_ROTATION;
                    o.lightDirT = mul(rotation, ObjSpaceLightDir(v.vertex));
                    o.viewDirT = mul(rotation, ObjSpaceViewDir(v.vertex));
                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }
               
                sampler2D _DiffuseTex;
                sampler2D _SpecularTex;
                sampler2D _NormalTex;
               
                float4 _LightColor0;
 
                float4 frag(v2f i) : COLOR
                {
                    float3 normal = normalize(tex2D(_NormalTex, i.uv).xyz * 2 - 1);
                    float NdotL = dot(normal, i.lightDirT);
                    float3 halfAngle = normalize(i.lightDirT + i.viewDirT);
                    float atten = LIGHT_ATTENUATION(i);

                    float4 result;
                    result.rgb = tex2D(_DiffuseTex, i.uv).rgb * NdotL * atten * _LightColor0;
                    result.a = 0;
                    return result;
                }
            ENDCG
        }
    }
}