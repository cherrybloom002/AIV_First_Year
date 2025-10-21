using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    static class BulletMngr
    {
        private static Queue<Bullet>[] bullets;

        public static void Init()
        {
            int queueSize = 16;

            bullets = new Queue<Bullet>[(int)BulletType.LAST];

            //int a = 5;
            //Type t = a.GetType();//get type from instance
            //Type tt = typeof(int);//get type from class name/type

            Type[] bulletTypes = new Type[(int)BulletType.LAST];
            bulletTypes[0] = typeof(StdBullet);
            bulletTypes[1] = typeof(Rocket);

            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Queue<Bullet>(queueSize);

                for (int j = 0; j < queueSize; j++)
                {
                    Bullet b = (Bullet)Activator.CreateInstance(bulletTypes[i]);
                    bullets[i].Enqueue(b);
                }


                //switch ((BulletType)i)
                //{
                //    case BulletType.StdBullet:

                //        for (int q = 0; q < queueSize; q++)
                //        {
                //            bullets[i].Enqueue(new StdBullet());
                //        }

                //        break;
                //    case BulletType.Rocket:

                //        for (int q = 0; q < queueSize; q++)
                //        {
                //            bullets[i].Enqueue(new Rocket());
                //        }

                //        break;
                //}
            }
        }

        public static Bullet GetBullet(BulletType type)
        {
            int index = (int)type;

            if (bullets[index].Count > 0)
            {
                Bullet bullet = bullets[index].Dequeue();
                bullet.Reset();

                UpdateMngr.AddItem(bullet);
                DrawMngr.AddItem(bullet);

                return bullet;
            }

            return null;
        }

        public static void RestoreBullet(Bullet bullet)
        {
            bullet.IsActive = false;
            bullets[(int)bullet.Type].Enqueue(bullet);

            UpdateMngr.RemoveItem(bullet);
            DrawMngr.RemoveItem(bullet);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].Clear();
            }
        }
    }
}
