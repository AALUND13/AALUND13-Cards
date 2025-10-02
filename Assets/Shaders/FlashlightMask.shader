Shader "AALUND13/UI/FlashlightMask"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _LightCount ("Light Count", Float) = 1
        _GlobalFade("Global Fade", Range(0,1)) = 1
    }

    SubShader {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGBA
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform float4 _LightPosArray[32];
            uniform float _RadiusArray[32];
            uniform int _LightCount;
            uniform float _GlobalFade;

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv      : TEXCOORD0;
                float2 screenUV : TEXCOORD1;
            };

            struct appdata {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = TRANSFORM_TEX(v.texcoord, _MainTex);
                float4 screenPos = ComputeScreenPos(o.vertex);
                o.screenUV = screenPos.xy / screenPos.w;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = i.screenUV;

                float2 aspect = float2(_ScreenParams.x / _ScreenParams.y, 1.0);

                float alpha = 1.0;
                for (int j = 0; j < _LightCount; ++j) {
                    float2 pos = _LightPosArray[j].xy;
                    float2 diff = (uv - pos) * aspect;

                    float rad = _RadiusArray[j] > 0 ? _RadiusArray[j] : 0.2;
                    float d = length(diff);
                    float a = smoothstep(rad * 0.9, rad, d);
                    alpha = min(alpha, a);
                }

                return float4(0, 0, 0, alpha * _GlobalFade);
            }
            ENDCG
        }
    }
}
