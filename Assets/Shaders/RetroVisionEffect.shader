Shader "AAC/RetroVisionEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTexTint ("Texture Tint", Color) = (1,1,1,1)

        _NoiseTex ("Noise Texture", 2D) = "white" {}

        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.5

        _VignetteInternsity ("Vignette Internsity", Range(0, 5)) = 1

        _SubStripesntensity ("Substripes Intensity", Range(0, 5)) = 0.05

        _StripesIntensity ("Stripes Intensity", Range(0, 1)) = 0.5
        _StripesSpeed ("Stripes Speed", Range(0, 3)) = 1
        _StripeDensity ("Stripe Density", Range(0.1, 10.0)) = 4.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTexTint;

            float4 _MainTex_TexelSize;

            sampler2D _NoiseTex;
            
            float _NoiseIntensity;

            float _VignetteInternsity;

            float _SubStripesntensity;

            float _StripesIntensity;
            float _StripesSpeed;
            float _StripeDensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float getNoise (float2 p) 
            {
                float t = _Time.y;
                float2 uv = frac(float2(1.0, 2.0 * cos(t)) * t * 8.0 + p);
                float s = tex2D(_NoiseTex, uv).x;
                return s * s;
            }

            float ramp (float y, float start, float end) 
            {
                float inside = step(start, y) - step(end, y);
                float fact = (y - start) / (end - start) * inside;
                return (1.0 - fact) * inside;
            }

            float getStripes(float2 uv)
            {
                float t = _Time.y * _StripesSpeed;
                float noi = getNoise(uv * float2(0.5, 1.0) + float2(1.0, 3.0));
                float stripePos = fmod(uv.y * _StripeDensity + t / 2.0 + sin(t + sin(t * 0.63)), 1.0);

                return ramp(stripePos, 0.5, 0.6) * noi;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float4 col = tex2D(_MainTex, uv);
                col.rgb *= _MainTexTint.rgb;
                
                float vignette = (1.0 - _VignetteInternsity * (uv.x - 0.5) * (uv.x - 0.5)) * 
                                 (1.0 - _VignetteInternsity * (uv.y - 0.5) * (uv.y - 0.5));
                vignette = saturate(vignette);


                col *= lerp(0.9, 1.2, fmod(uv.y * 30.0 + _Time.y, 1.0)) * _SubStripesntensity;
                col += getStripes(uv) * _StripesIntensity;
                col += pow(getNoise(uv * 2.0), 1.2) * _NoiseIntensity;
                col *= vignette;

                return col;
            }

            ENDHLSL
        }
    }
}
