Shader "Hidden/ProBuilder/UnlitColor" 
{
	Properties
	{
		_Color ("Color Tint", Color) = (1,1,1,1)   
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white"
	}

	Category
	{
		Lighting Off
		ZWrite On
	//	ZTest Off
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha

	//	AlphaTest Greater 0.001
		Tags {"Queue"="Transparent" }

		SubShader
		{
			Pass
			{
				SetTexture [_MainTex]
				{
					ConstantColor [_Color]
					Combine Texture * constant
				}
			}
		}
	}
}