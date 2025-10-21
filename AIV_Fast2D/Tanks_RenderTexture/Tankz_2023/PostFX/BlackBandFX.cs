using System;
using Aiv.Fast2D;

namespace Tankz_2023
{
    class BlackBandFX : PostProcessingEffect
    {   //GLSL: OpenGL Shading Language
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;


void main() {
    vec4 tex_color = texture(tex, uv);
    
    vec4 color = vec4(1.f, 1.f, 1.f, 1.f);
    if (uv.x <= 0.5f) {
        color = vec4(1.f, 0.f, 0.f, 0.5f);
    }
    
    //out_color = color * tex_color;

    out_color = mix(color, tex_color, 0.8f);
}


//void main() {
//    vec4 tex_color = texture(tex, uv);
  
//    if (uv.y < 0.1f || uv.y > 0.9f) {
//        out_color = vec4(0.f, 0.f, 0.f, 0.f);     
//    } else {
//        out_color = tex_color;
//    }
 
    


//}
";
        public BlackBandFX() : base(fragmentShader) { }
    }
}
