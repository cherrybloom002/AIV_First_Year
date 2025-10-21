using System;
using Aiv.Fast2D;

namespace Tankz_2023
{
    class WobbleFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

uniform float time;

void main() {
    vec2 uv_current = uv;    

    // WAVE FORM: y = A * sin(B * (x + C) ) + D
    
    float A = 1.f / 100.f; //amplitude
    float B = 30.f;        //frequency
    float C = time / 100.f;        //phase
    
    uv_current.x += A * sin( B * (uv_current.y + C) );
    vec4 tex_color = texture(tex, uv_current);
    
    out_color = tex_color;
}
";
        public WobbleFX() : base(fragmentShader) {
            speed = 5.0f;
        }

        public void SetSpeed(float aSpeed)
        {
            speed = aSpeed;
        }

        public override void Update(Window window)
        {
            time += window.DeltaTime * speed;
            screenMesh.shader.SetUniform("time", time);
        }

        private float time;
        private float speed;
    }
}
