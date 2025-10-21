using System;
using Aiv.Fast2D;

namespace Tankz_2023
{
    class SepiaFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

void main() {
    vec4 tex_color = texture(tex, uv);
    
/*
    float gray = (tex_color.r + tex_color.g + tex_color.b) / 3.f;
    out_color = vec4(gray * 0.5f, gray * 0.6f, gray * 0.2f, 1.f);
*/  

    float gray = dot(tex_color.rgb, vec3(0.299f, 0.587f, 0.114f));
    out_color = vec4(gray, gray * 0.95f, gray * 0.82f, tex_color.a);
}
";
        public SepiaFX() : base(fragmentShader) { }
    }
}
