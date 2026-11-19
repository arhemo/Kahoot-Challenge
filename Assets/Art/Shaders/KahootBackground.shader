// Fondo de las pantallas: degradado morado con franjas diagonales que se mueven despacio y una vineta suave.
Shader "Kahoot/UIBackground"
{
    Properties
    {
        // Los Image de la UI siempre buscan _MainTex; sin esta propiedad Unity avisa.
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _TopColor ("Top color", Color) = (0.30, 0.10, 0.62, 1)
        _BottomColor ("Bottom color", Color) = (0.10, 0.03, 0.26, 1)
        _StripeStrength ("Stripe strength", Range(0, 0.1)) = 0.03
        _StripeSpeed ("Stripe speed", Range(0, 1)) = 0.08
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

        Cull Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _TopColor;
            fixed4 _BottomColor;
            float _StripeStrength;
            float _StripeSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Posicion en pantalla de 0 a 1 (asi funciona aunque la imagen no tenga sprite)
                float2 uv = i.pos.xy / _ScreenParams.xy;
                uv.y = 1 - uv.y;

                fixed4 col = lerp(_BottomColor, _TopColor, uv.y);

                // Franjas diagonales suaves
                float stripes = sin((uv.x * 5.0 + uv.y * 3.0 - _Time.y * _StripeSpeed) * 6.2831) * 0.5 + 0.5;
                col.rgb += stripes * _StripeStrength;

                // Vineta
                float2 d = uv - 0.5;
                col.rgb *= 1.0 - dot(d, d) * 0.7;

                col.a = 1;
                return col;
            }
            ENDCG
        }
    }
}
