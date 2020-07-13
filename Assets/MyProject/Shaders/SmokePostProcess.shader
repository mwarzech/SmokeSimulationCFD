Shader "Custom/SmokePostProcess"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DensTex ("DensTexture", 3D) = "white" {}
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// Provided by our script
			uniform float4x4 _FrustumCornersES;
			uniform sampler2D _MainTex;
			uniform sampler3D _DensTex;
			uniform float4 _MainTex_TexelSize;
			uniform float3 _CameraWS;
			uniform float4x4 _CameraInvViewMatrix;
			uniform sampler2D _CameraDepthTexture;
			fixed4 _LightColor0;

			uniform int3 _GridSize;
			uniform float _GridScale;
			uniform float _GridRadius;
			uniform float4x4 _WorldToObject;
			
			// Output of vertex shader / input to fragment shader
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 ray : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;

				// Index passed via custom blit function in RaymarchGeneric.cs
				half index = v.vertex.z;
				v.vertex.z = 0.0;//0.1;

				if (v.uv.x == 0)
				{
					if (v.uv.y == 0)
					{
						index = 3.0;
					}
					else
					{
						index = 0.0;
					}
				}
				else
				{
					if (v.uv.y == 0)
					{
						index = 2.0;
					}
					else
					{
						index = 1.0;
					}
				}

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
#endif

				// Get the eyespace view ray (normalized)
				o.ray = _FrustumCornersES[(int)index].xyz;

				o.ray /= abs(o.ray.z);
				// Transform the ray from eyespace to worldspace
				// Note: _CameraInvViewMatrix was provided by the script
				o.ray = mul(_CameraInvViewMatrix, o.ray);
				return o;
			}


			float densAtPoint(float3 pos)
			{
				float3 objPos = mul(_WorldToObject, float4(pos, 1)).xyz;
				if (length(objPos) > _GridRadius) return -1;
				float3 objPoint = objPos * _GridScale + float3(_GridSize*0.5);
				if (objPoint.x < 0 || objPoint.x >= _GridSize.x ||
					objPoint.y < 0 || objPoint.y >= _GridSize.y ||
					objPoint.z < 0 || objPoint.z >= _GridSize.z)
					return 0;
				return tex3D(_DensTex, float3(objPoint.x / _GridSize.x, objPoint.y / _GridSize.y, objPoint.z / _GridSize.z));
			}

#define MAX_ITER 80
#define STEP 0.5
#define DENS_STREN 3
			float castRay(float3 ray, float depth)
			{
				float result = 0;
				float currentDepth = 0;
				float3 currentPos = _CameraWS;

				float3 objPos = mul(_WorldToObject, float4(currentPos, 1)).xyz;
				float3 objRay = mul(_WorldToObject, float4(ray, 0)).xyz;
				float L = length(objPos);
				if (L > _GridRadius)
				{
					float t_ca = dot(-objPos, objRay);
					if (t_ca <= 0) return result;
					float d_sqr = L * L - t_ca * t_ca;
					float r_sqr = _GridRadius * _GridRadius;
					if (d_sqr > r_sqr) return result;
					float t_hc = sqrt(r_sqr - d_sqr);
					currentDepth = (t_ca - t_hc);
				}

				for (uint i = 0; i < MAX_ITER; ++i)
				{
					if (currentDepth > depth) return (result*DENS_STREN);
					currentDepth += STEP;
					currentPos = _CameraWS + currentDepth * ray;
					float dens = densAtPoint(currentPos);
					if(dens < 0) return (result*DENS_STREN);
					result += dens * STEP;
				}

				return (result*DENS_STREN);
			}

			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, i.uv).r);
				depth *= length(i.ray.xyz);
				//float3 worldRay = normalize(mul(unity_ObjectToWorld, float4(i.ray, 0)).xyz);
				float dens = min(1, castRay(normalize(i.ray), depth) / 10.0);

				col = lerp(col, fixed4(0.1, 0.1, 0.1, 1), dens);
				return col;
			}
			ENDCG
		}
	}
}
