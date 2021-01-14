Shader "EduLabs/Silhouette" {
  Properties {
    [MainTexture] [PerRendererData] [NoScaleOffset]
    _BaseMap("Albedo", 2D) = "white" {}
    [MainColor] [PerRendererData]
    _BaseColor("Color", Color) = (1,1,1,1)
  }

  SubShader {
    Tags {
      "Queue" = "Transparent"
      "IgnoreProjector" = "True"
      "RenderType" = "Transparent"
      "RenderPipeline"   = "UniversalPipeline"
    }

    Pass {
      Name "Silhouette"
      Tags { "LightMode" = "SRPDefaultUnlit" }

      ZWrite Off
      Cull Back
      Blend SrcAlpha OneMinusSrcAlpha

      Stencil {
        Ref 4
        ReadMask 4
        WriteMask 4
        Comp NotEqual
        Pass replace
      }

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #include "UnityCG.cginc"

      struct appdata
      {
        float2 uv : TEXCOORD0;
        float4 vertex : POSITION;
      };

      struct v2f
      {
        float2 uv : TEXCOORD0;
        float4 pos : POSITION;
      };

      v2f vert(appdata v) {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        return o;
      }

      float4  _BaseColor;
      sampler2D _BaseMap;

      float4 frag(v2f i) : COLOR { return tex2D(_BaseMap, i.uv) * _BaseColor; }
      
      ENDCG
    }
  }

  FallBack "Hidden/Universal Render Pipeline/FallbackError"
}