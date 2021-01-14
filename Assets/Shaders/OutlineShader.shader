Shader "EduLabs/Outline" {
  Properties {
    [HDR] [MainColor]
    [PerRendererData] _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
    [PerRendererData] _OutlineWidth("Outline Width", Range(0, 1)) = 0
  }

  SubShader {
    Tags {
      "RenderType" = "Opaque"
      "RenderPipeline" = "UniversalPipeline"
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct appdata
    {
      float4 vertex : POSITION;
      float3 normal : NORMAL;
    };
    ENDCG

    Pass {
      Name "Stencil"
      Tags { "LightMode" = "UniversalForward" }

      ZWrite Off
      ZTest Always
      ColorMask 0

      Stencil {
        Ref 2
        ReadMask 2
        WriteMask 2
        Comp always
        Pass replace
        ZFail replace
      }
      
      CGPROGRAM
      #pragma vertex vert2
      #pragma fragment frag

      float4 vert2 (appdata v) : POSITION { return UnityObjectToClipPos(v.vertex); }
      half4 frag (void) : COLOR { return float4(0, 0, 0, 1); }

      ENDCG
    }


    Pass {
      Name "Outline"
      Tags { "LightMode" = "SRPDefaultUnlit" }
      ZWrite On
      ZTest Always

      Stencil {
        Ref 2
        ReadMask 2
        WriteMask 2
        Comp NotEqual
        Pass replace
      }

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      struct v2f
      {
        float4 pos : SV_POSITION;
      };
      
      float   _OutlineWidth;
      float4  _OutlineColor;

      v2f vert(appdata v)
      {
        v2f o;

        float3 normal = normalize(v.normal);
        float4 newPos = v.vertex + float4(normal, 0.0) * _OutlineWidth;

        o.pos = UnityObjectToClipPos(newPos);
        return o;
      }

      float4 frag(v2f input) : COLOR { return _OutlineColor; }
      
      ENDCG
    }
  }

  FallBack "Hidden/Universal Render Pipeline/FallbackError"
}