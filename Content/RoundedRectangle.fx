#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 Size;
float Radius;

struct PixelInput
{
    float4 Position : TEXCOORD;
};

//Signed Distance Field function
//Returns how far a pixel is from the edge of my rounded rectangle shape
float roundedRectSDF(float2 centerPosition, float2 size, float radius)
{
    return length(max(abs(centerPosition) - (size / 2) + radius, 0)) - radius;
}
sampler2D TextureSampler : register(s0)
{
    Texture = (Texture);
};

float4 MainPS(float4 position : SV_Position, float4 color : COLOR0, float2 TextureCoordinates : TEXCOORD0) : COLOR0
{
    float4 col = tex2D(TextureSampler, TextureCoordinates) * color;
    
    float2 pixelPos = float2(TextureCoordinates * Size);

    // Calculate distance to edge
    float distance = roundedRectSDF(pixelPos - (Size / 2.0f), Size, Radius);

    //discard pixels that are outside our rounded rectangle shape
    clip(0.01 - distance);
	
    return col;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};