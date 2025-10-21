using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Audio;

namespace Tankz_2023
{
    enum SpecialFX { Explosion_1, LAST }

    static class GfxMngr
    {
        private static Dictionary<string, Texture> textures;
        private static Dictionary<string, AudioClip> clips;

        private static List<GameObject>[] specialFX;

        static GfxMngr()
        {
            textures = new Dictionary<string, Texture>();
            clips = new Dictionary<string, AudioClip>();
        }

        public static void InitFX()
        {
            specialFX = new List<GameObject>[(int)SpecialFX.LAST];

            specialFX[(int)SpecialFX.Explosion_1] = new List<GameObject>();
            specialFX[(int)SpecialFX.Explosion_1].Add(new Explosion());

        }

        public static Texture AddTexture(string name, string path)
        {
            Texture t = new Texture(path);

            if (t != null)
            {
                textures[name] = t;
            }

            return t;
        }

        public static AudioClip AddClip(string name, string path)
        {
            AudioClip c = new AudioClip(path);

            if (c != null)
            {
                clips[name] = c;
            }

            return c;
        }

        public static Texture GetTexture(string name)
        {
            Texture t = null;

            if (textures.ContainsKey(name))
            {
                t = textures[name];
            }

            return t;
        }

        public static AudioClip GetClip(String name)
        {
            AudioClip c = null;

            if (clips.ContainsKey(name))
            {
                c = clips[name];
            }

            return c;
        }

        public static void ClearAll()
        {
            textures.Clear();
            clips.Clear();
        }

        public static GameObject GetSpecialFX(SpecialFX type)
        {
            GameObject fx = null;
            int listIndex = (int)type;

            for (int i = 0; i < specialFX[listIndex].Count; i++)
            {
                if (!specialFX[listIndex][i].IsActive)
                {
                    return specialFX[listIndex][i];
                }
            }

            return fx;
        }
    }
}
