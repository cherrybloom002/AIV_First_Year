using System;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2023
{
    class WobbleMouseFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

uniform float time;
uniform vec3 mouse;

void main() {
    vec2 uv_current = uv;    
    
    vec2 mouse_diff = mouse.xy - uv.xy;
    float mouse_dist = length(mouse_diff);    
    float ray = 1.f - clamp(mouse_dist * 10.f, 0.f, 1.f); 

    // WAVE FORM: y = A * sin(B * (x + C) ) + D
    float A = 1.f / 100.f * ray; //amplitude
    float B = 30.f;        //frequency
    float C = time / 100.f;        //phase
    
    uv_current.x += A * sin( B * (uv_current.y + C) );
    vec4 tex_color = texture(tex, uv_current);
    
    out_color = tex_color;
}
";
        public WobbleMouseFX() : base(fragmentShader) {
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

            Vector2 mouse = window.MousePosition;

            mouse.X /= window.OrthoWidth;
            mouse.Y =  1.0f - mouse.Y / window.OrthoHeight;

            screenMesh.shader.SetUniform("mouse", new Vector3(mouse));
        }

        private float time;
        private float speed;
    }
}
