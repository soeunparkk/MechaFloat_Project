// Made with Amplify Shader Editor v1.9.3.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Smoke_Shader_03"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_Texture02("Texture 02", 2D) = "white" {}
		[HDR]_Light_Color("Light_Color", Color) = (1,1,1,0)
		_Power("Power", Float) = 0.08
		_Speed("Speed", Vector) = (0.1,0,0,0)
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Emissive("Emissive", Float) = 1

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			//This is a late directive
			
			uniform float4 _Main_Color;
			uniform float4 _Light_Color;
			uniform float _Emissive;
			uniform sampler2D _Texture02;
			uniform float2 _Speed;
			uniform float _Power;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				float3 ase_worldPos = mul(unity_ObjectToWorld, float4( (IN.vertex).xyz, 1 )).xyz;
				OUT.ase_texcoord1.xyz = ase_worldPos;
				
				OUT.ase_texcoord2 = IN.ase_texcoord1;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				OUT.ase_texcoord1.w = 0;
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 texCoord256 = IN.texcoord.xy * float2( 2,2 ) + float2( -1,-1 );
				float dotResult257 = dot( texCoord256 , texCoord256 );
				float3 appendResult262 = (float3(texCoord256 , sqrt( saturate( ( 1.0 - dotResult257 ) ) )));
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 worldSpaceLightDir = UnityWorldSpaceLightDir(ase_worldPos);
				float3 worldToViewDir263 = mul( UNITY_MATRIX_V, float4( worldSpaceLightDir, 0 ) ).xyz;
				float dotResult264 = dot( appendResult262 , worldToViewDir263 );
				float4 lerpResult268 = lerp( _Main_Color , _Light_Color , saturate( dotResult264 ));
				float2 texCoord197 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 texCoord255 = IN.ase_texcoord2;
				texCoord255.xy = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner196 = ( 1.0 * _Time.y * _Speed + ( texCoord197 * texCoord255.z ));
				float4 tex2DNode193 = tex2D( _Texture02, panner196 );
				float2 temp_output_34_0_g6 = ( texCoord197 - float2( 0.5,0.5 ) );
				float2 break39_g6 = temp_output_34_0_g6;
				float2 appendResult50_g6 = (float2(( 1.0 * ( length( temp_output_34_0_g6 ) * 2.0 ) ) , ( ( atan2( break39_g6.x , break39_g6.y ) * ( 1.0 / 6.28318548202515 ) ) * 1.0 )));
				float2 break53_g6 = appendResult50_g6;
				float4 appendResult53 = (float4(( ( lerpResult268 * IN.color ) * _Emissive ).rgb , ( IN.color.a * saturate( ( pow( tex2DNode193.r , _Power ) - break53_g6.x ) ) )));
				
				fixed4 c = appendResult53;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19302
Node;AmplifyShaderEditor.TextureCoordinatesNode;256;717.7021,-436.2901;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;-1,-1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;257;944.1418,-359.9843;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;258;1064.181,-354.1268;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;259;1214.27,-348.3205;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;197;1208.506,83.71779;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;255;1243.816,238.9998;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SqrtOpNode;260;1365.727,-342.2721;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;261;1332.432,-205.4646;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector2Node;233;1551.152,190.6832;Inherit;False;Property;_Speed;Speed;3;0;Create;True;0;0;0;False;0;False;0.1,0;0,-0.6;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;254;1465.415,46.19974;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;262;1501.387,-432.5116;Inherit;True;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TransformDirectionNode;263;1575.02,-204.3243;Inherit;False;World;View;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;196;1744,112;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;264;1780.432,-381.4645;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;193;1920,96;Inherit;True;Property;_Texture02;Texture 02;0;0;Create;True;0;0;0;False;0;False;-1;06791f8814ebc2248be4da904464d875;3ed92d7c939119a4e942c85dffe84931;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;208;2384,176;Inherit;False;Property;_Power;Power;2;0;Create;True;0;0;0;False;0;False;0.08;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;265;2056.374,-381.7892;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;266;2007.546,-605.1667;Inherit;False;Property;_Main_Color;Main_Color;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1.105882,1.027451,1.027451,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;267;1792.315,-605.0734;Inherit;False;Property;_Light_Color;Light_Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;3.521538,3.392476,3.15279,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;245;2560,254.7852;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;250;1536.182,326.5508;Inherit;False;Polar Coordinates;-1;;6;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;3;FLOAT2;0;FLOAT;55;FLOAT;56
Node;AmplifyShaderEditor.VertexColorNode;252;2496.961,-10.46972;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;268;2528.009,-132.8737;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;198;2727.997,349.2287;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;249;2755.045,195.0791;Inherit;False;Property;_Emissive;Emissive;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;247;2900.993,341.4901;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;269;2735.894,-12.16308;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;248;2909.314,40.80925;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;253;3034.253,255;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;224;2224,112;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;53;3195.033,272.915;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;115;3360,272;Float;False;True;-1;2;ASEMaterialInspector;0;10;Smoke_Shader_03;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;257;0;256;0
WireConnection;257;1;256;0
WireConnection;258;0;257;0
WireConnection;259;0;258;0
WireConnection;260;0;259;0
WireConnection;254;0;197;0
WireConnection;254;1;255;3
WireConnection;262;0;256;0
WireConnection;262;2;260;0
WireConnection;263;0;261;0
WireConnection;196;0;254;0
WireConnection;196;2;233;0
WireConnection;264;0;262;0
WireConnection;264;1;263;0
WireConnection;193;1;196;0
WireConnection;265;0;264;0
WireConnection;245;0;193;1
WireConnection;245;1;208;0
WireConnection;250;1;197;0
WireConnection;268;0;266;0
WireConnection;268;1;267;0
WireConnection;268;2;265;0
WireConnection;198;0;245;0
WireConnection;198;1;250;55
WireConnection;247;0;198;0
WireConnection;269;0;268;0
WireConnection;269;1;252;0
WireConnection;248;0;269;0
WireConnection;248;1;249;0
WireConnection;253;0;252;4
WireConnection;253;1;247;0
WireConnection;224;0;193;1
WireConnection;53;0;248;0
WireConnection;53;3;253;0
WireConnection;115;0;53;0
ASEEND*/
//CHKSM=C745E6E70108308EA79DC2365267983C1ACC8665