using System;
using Aiv.Fast2D;

namespace Tankz_2023
{
    class NegativeFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

void main() {
    vec4 tex_color = texture(tex, uv);
    out_color = 1.f - tex_color; //vec4(1.f, 1.f, 1.f, 1.f) - tex_color;
}
";
    public NegativeFX() : base(fragmentShader) { }
    }
}
