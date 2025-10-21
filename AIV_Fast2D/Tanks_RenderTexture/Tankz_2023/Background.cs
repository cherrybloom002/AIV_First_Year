using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2023
{
    class Background : I_Drawable
    {
        private Sprite playground;
        //private Sprite playground2;
        private Texture playgroundTexture;

        private Texture[] textures;
        private Sprite[] sprites;

        private int textureRepeat = 4;

        public DrawLayer Layer { get; protected set; }

        public Background()
        {
            Layer = DrawLayer.Background;

            int numTextures = 2;

            playgroundTexture = new Texture("Assets/bg_2.png");
            playgroundTexture.SetRepeatX(true);

            playground = new Sprite(Game.PixelsToUnits(playgroundTexture.Width * textureRepeat), Game.PixelsToUnits(playgroundTexture.Height));
            playground.position.Y = 4.6f;

            //playground2 = new Sprite(playground.Width, playground.Height);
            //playground2.position.X = playground.Width;
            //playground2.position.Y = playground.position.Y;


            textures = new Texture[numTextures];
            sprites = new Sprite[numTextures];//in order to have a clone

            float[] positionsY = new float[] { -2, 3.8f };

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = new Texture($"Assets/bg_{i}.png");
                textures[i].SetRepeatX(true);

                sprites[i] = new Sprite(Game.PixelsToUnits(textures[i].Width * textureRepeat), Game.PixelsToUnits(textures[i].Height));
                sprites[i].position.Y = positionsY[i];

                //creating clone
                //int cloneIndex = i + numTextures;

                //sprites[cloneIndex] = new Sprite(sprites[i].Width, sprites[i].Height);
                //sprites[cloneIndex].position.Y = sprites[i].position.Y;
                //sprites[cloneIndex].position.X = sprites[i].Width;//don't need to add position as long as sprite[i] is in x=0

                //setting cameras
                if(i < numTextures)
                {
                    sprites[i].Camera = CameraMgr.GetCamera($"Bg_{i}");
                    //sprites[cloneIndex].Camera = sprites[i].Camera;
                }
            }


            DrawMngr.AddItem(this);
        }

        public void Draw()
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].DrawTexture(textures[i % textures.Length],0,0,textures[i].Width * textureRepeat, textures[i].Height);
            }

            playground.DrawTexture(playgroundTexture,0,0, playgroundTexture.Width * textureRepeat , playgroundTexture.Height);
            //playground2.DrawTexture(playgroundTexture);

        }
    }
}
