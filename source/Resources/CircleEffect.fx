struct VS_S {
    float4 Position: POSITION;
    float4 Color: COLOR0;
    float4 Position3D: TEXCOORD0;
};

float4x4 ProjectionMatrix;
float4 CircleColor;
float Radius;
float Border;
bool zEnabled;

VS_S VS(VS_S input) {
    VS_S output = (VS_S) 0;

    output.Position = mul(input.Position, ProjectionMatrix);
    output.Color = input.Color;
    output.Position3D = input.Position;
    return output;
}

float4 PS(VS_S input): COLOR {
    VS_S output = (VS_S) 0;
    output = input;
    float4 v = output.Position3D;
    float distance = Radius - sqrt(v.x * v.x + v.z * v.z); // Distance to the circle arc.

    output.Color.x = CircleColor.x;
    output.Color.y = CircleColor.y;
    output.Color.z = CircleColor.z;

    if (distance < Border && distance > -Border) {
        output.Color.w = (CircleColor.w - CircleColor.w * abs(distance * 1.75 / Border));
    } else {
        output.Color.w = 0;
    }

    if (Border < 1 && distance >= 0) {
        output.Color.w = CircleColor.w;
    }
    return output.Color;
}

technique Main {
    pass P0 {
        ZEnable = zEnabled;
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS();
    }
}