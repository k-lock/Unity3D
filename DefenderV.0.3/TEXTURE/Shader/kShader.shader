Shader "kShader/Basic" {
	
	Properties { 
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	Category {
		Tags {"RenderType"="Transparent" "Queue"="Transparent"}
		Lighting Off
		
		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
		}
		
		SubShader {
			Pass {
				
				ZWrite Off
				Cull Off
				
				Blend SrcAlpha OneMinusSrcAlpha 
				
				//SetTexture [_MainTex] { Combine texture * primary double, primary }
			}
		}
	}
}