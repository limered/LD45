Shader "Hidden/CRTDiffuse"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;

			fixed4 frag(v2f_img i) : COLOR
			{
				float2 uv = i.uv;
				uv *= 1.0 - uv.yx;
				float vig = uv.x*uv.y * 25;
				vig = pow(vig, 0.75);
				fixed4 base = tex2D(_MainTex, i.uv);
				fixed4 vign = fixed4(vig, vig, vig, vig);
				return base * vign;
            }
            ENDCG
        }
    }
}
