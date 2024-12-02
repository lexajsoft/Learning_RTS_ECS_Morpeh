Shader "Unlit/ProgressBarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorBackground ("Background", color) = (1,1,1,1)
        _ColorValue ("Value", color) = (1,1,1,1)
        _ColorBackgroundValue ("BackgroundValue", color) = (1,1,1,1)
        _BorderSizeWidth ("BorderSizeWidth",float) = 0
        _BorderSizeHeight ("BorderSizeHeight",float) = 0
        _Progress ("Progress", float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ColorBackground;
            float4 _ColorValue;
            float4 _ColorBackgroundValue;
            float _BorderSizeWidth;
            float _BorderSizeHeight;
            float _Progress;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // border
                if(i.uv.x - _BorderSizeWidth < 0 || i.uv.x + _BorderSizeWidth > 1)
                {
                    return _ColorBackground;
                }

                if(i.uv.y - _BorderSizeHeight < 0 || i.uv.y + _BorderSizeHeight > 1)
                {
                    return _ColorBackground;
                }

                // progress
                // максимальная ширина
                float maxWidth = 1 - _BorderSizeWidth * 2;
                float widthProgress = maxWidth * _Progress;
                
                if(i.uv.x  > _BorderSizeWidth + widthProgress )
                {
                    return _ColorBackgroundValue;
                }
                else
                {
                    return _ColorValue;
                }

                

                return _ColorValue;
            }
            ENDCG
        }
    }
}
