Shader "Custom/Water"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Normal", 2D) = "bump" {}
        _NormalStrength ("Normal Strength", Range(0,1)) = 1.0
        // два дирекшена в одном векторе
        _Direction12 ("Direction12", Vector) = (1.0, 0, 0.70710678118654752440084436210485, 0.70710678118654752440084436210485)
        _Direction34 ("Direction34", Vector) = (1.0, 0, 0.70710678118654752440084436210485, 0.70710678118654752440084436210485)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        half _Glossiness;
        half _Metallic;
        float _NormalStrength;
        fixed4 _Color;
        float4 _Direction12;
        float4 _Direction34;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float3 get_normal(Input IN, float2 direction, float2 offset) {
            return UnpackNormal(
                lerp(
                    float4(0.5, 0.5, 1, 1),
                    tex2D(_BumpMap, IN.uv_BumpMap + _Time.x * direction + offset),
                    _NormalStrength
                )
            );
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Normal = normalize((
                get_normal(IN, _Direction12.xy, float2(0.00, 0.00)) +
                get_normal(IN, _Direction12.zw, float2(0.25, 0.25)) +
                get_normal(IN, _Direction34.xy, float2(0.50, 0.50)) +
                get_normal(IN, _Direction34.zw, float2(0.75, 0.75))
            ) * 0.25f);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
