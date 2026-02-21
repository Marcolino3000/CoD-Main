Shader "Sprites/Custom/LineBoil"
                        {
                            Properties
                            {
                                [MainTexture] _MainTex("Sprite Texture", 2D) = "white" {}
                                [MainColor] _Color("Tint", Color) = (1,1,1,1)
                                [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
                                [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
                                [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
                                _NoiseScale ("Noise Scale", Range(0, 100)) = 1
                                _NoiseSnap ("Noise Snap", Range(0, 10)) = 1
                            }
                        
                            SubShader
                            {
                                Tags
                                {
                                    "Queue" = "Transparent"
                                    "IgnoreProjector" = "True"
                                    "RenderType" = "Transparent"
                                    "PreviewType" = "Plane"
                                    "CanUseSpriteAtlas" = "True"
                                    "RenderPipeline" = "UniversalPipeline"
                                }
                        
                                Cull Off
                                Lighting Off
                                ZWrite Off
                                Blend One OneMinusSrcAlpha
                        
                                Pass
                                {
                                    HLSLPROGRAM
                                    #pragma vertex vert
                                    #pragma fragment frag
                                    #pragma multi_compile_local _ PIXELSNAP_ON
                        
                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        
                                    TEXTURE2D(_MainTex);
                                    SAMPLER(sampler_MainTex);
                        
                                    CBUFFER_START(UnityPerMaterial)
                                        half4 _Color;
                                        half4 _RendererColor;
                                        float4 _MainTex_ST;
                                        float4 _Flip;
                                        float _NoiseScale;
                                        float _NoiseSnap;
                                    CBUFFER_END
                        
                                    struct Attributes
                                    {
                                        float4 positionOS : POSITION;
                                        float2 uv : TEXCOORD0;
                                        float4 color : COLOR;
                                    };
                        
                                    struct Varyings
                                    {
                                        float4 positionHCS : SV_POSITION;
                                        float2 uv : TEXCOORD0;
                                        half4 color : COLOR;
                                    };

                                    float rand(float2 co)
                                    {
                                        return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
                                    }

                                    inline float snap (float x, float snap)
                                    {
                                        return snap * round(x / snap);
                                    }
                        
                                    Varyings vert(Attributes IN)
                                    {
                                        Varyings OUT;
                        
                                        float4 flippedPos = IN.positionOS * _Flip;
                                        OUT.positionHCS = TransformObjectToHClip(flippedPos.xyz);

                                        float time = snap (_Time.y, _NoiseSnap);
                                        float2 noise = rand(OUT.positionHCS.xyz + float3(time, 0.0, 0.0)).xy * _NoiseScale;
                                        OUT.positionHCS.xy += noise;
                                        
                                        #if defined(PIXELSNAP_ON)
                                        OUT.positionHCS = floor(OUT.positionHCS * 0.5) * 2.0;
                                        #endif
                        
                                        OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                                        OUT.color = IN.color * _Color * _RendererColor;
                        
                                        return OUT;
                                    }
                        
                                    half4 frag(Varyings IN) : SV_Target
                                    {
                                        half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;
                                        c.rgb *= c.a;
                                        return c;
                                    }
                                    ENDHLSL
                                }
                            }
                        
                            Fallback "Sprites/Default"
                        }