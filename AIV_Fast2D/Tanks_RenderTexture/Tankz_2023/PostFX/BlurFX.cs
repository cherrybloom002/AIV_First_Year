using System;
using Aiv.Fast2D;

namespace Tankz_2023
{
    class BlurFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

uniform float pixW;
uniform float pixH;

void main() {
    vec4 tex_color = texture(tex, uv);
    
    vec2 uv_right = vec2(uv.x + pixW, uv.y);
    vec4 tex_right = texture(tex, uv_right);
    
    vec2 uv_left = vec2(uv.x - pixW, uv.y);
    vec4 tex_left = texture(tex, uv_left);

    vec2 uv_top = vec2(uv.x, uv.y + pixH);
    vec4 tex_top = texture(tex, uv_top);

    vec2 uv_down = vec2(uv.x, uv.y - pixH);
    vec4 tex_down = texture(tex, uv_down);

    vec2 uv_topr = vec2(uv.x + pixW, uv.y + pixH);
    vec4 tex_topr = texture(tex, uv_topr);
    
    vec2 uv_topl = vec2(uv.x - pixW, uv.y + pixH);
    vec4 tex_topl = texture(tex, uv_topl);

    vec2 uv_downr = vec2(uv.x + pixW, uv.y - pixH);
    vec4 tex_downr = texture(tex, uv_downr);
    
    vec2 uv_downl = vec2(uv.x - pixW, uv.y - pixH);
    vec4 tex_downl = texture(tex, uv_downl);
    
    out_color = (tex_color + tex_right + tex_left + tex_top + tex_down + tex_topr + tex_topl + tex_downr + tex_downl) / 9.f;
}
";
        public BlurFX() : base(fragmentShader) 
        {
            float pixelWidth = 1.0f / Game.Window.Width;
            float pixelHeight = 1.0f / Game.Window.Height;

            screenMesh.shader.SetUniform("pixW", pixelWidth);
            screenMesh.shader.SetUniform("pixH", pixelHeight);
        }
    }
}
