using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    enum DrawLayer { Background, Middleground, Playground, Foreground, GUI, Last }

    static class DrawMngr
    {
        private static List<I_Drawable>[] items;
        private static RenderTexture sceneTexture;
        private static Sprite scene;

        private static Dictionary<string, PostProcessingEffect> postFX;


        static DrawMngr()
        {
            items = new List<I_Drawable>[(int)DrawLayer.Last];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new List<I_Drawable>();
            }

            sceneTexture = new RenderTexture(Game.Window.Width, Game.Window.Height);
            scene = new Sprite(Game.Window.OrthoWidth, Game.Window.OrthoHeight);
            scene.Camera = CameraMgr.GetCamera("GUI");

            postFX = new Dictionary<string, PostProcessingEffect>();
        }

        public static void AddItem(I_Drawable item)
        {
            items[(int)item.Layer].Add(item);
        }

        public static void RemoveItem(I_Drawable item)
        {
            items[(int)item.Layer].Remove(item);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Clear();
            }

            postFX.Clear();
        }

        public static void AddFX(string fxName, PostProcessingEffect fx)
        {
            postFX.Add(fxName, fx);
        }

        public static void RemoveFX(string fxName)
        {
            postFX.Remove(fxName);
        }

        private static void ApplyPostFX()
        {
            foreach (var item in postFX)
            {
                sceneTexture.ApplyPostProcessingEffect(item.Value);
            }
        }

        public static void Draw()
        {
            //start rendering on render texture
            Game.Window.RenderTo(sceneTexture);

            for (int i = 0; i < items.Length; i++)
            {//array iteration

                if((DrawLayer)i == DrawLayer.GUI)
                {
                    ApplyPostFX();
                    //start rendering on screen
                    Game.Window.RenderTo(null);
                    //draw scene with applied post FX
                    scene.DrawTexture(sceneTexture);
                }

                for (int j = 0; j < items[i].Count; j++)
                {
                    items[i][j].Draw();
                }
            }
        }
    }
}
