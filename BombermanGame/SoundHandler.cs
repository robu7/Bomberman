using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace BombermanGame
{
    static class SoundHandler
    {
        private static SoundPlayer themeMusic = new SoundPlayer(Properties.Resources.theme1);

        public static void PlayTheme() { themeMusic.Play(); }
        public static void StopTheme() { themeMusic.Stop(); }



        public static void PlayExplosionSound() {
            SoundPlayer themeMusic = new SoundPlayer(Properties.Resources.theme1);
        } 


    }
}
