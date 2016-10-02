using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMP
{
static class song_control
    {
        static int current_song, max_songs,song_index;
        public static int nextSong(bool rand_check)
        {
            Random rnd = new Random();
            if (rand_check== true)
            {
                if (max_songs == 1)
                {
                    song_index = 0;
                }
                else
                {
                    do
                    {
                        song_index = rnd.Next(0, max_songs);
                    } while (song_index == current_song);
                }
                return song_index;
            }
            else
            {
                if (current_song == max_songs)
                {
                    song_index = 0;
                    return song_index;
                }
                else
                {
                    song_index++;
                    return song_index;
                }
            }
        }
        public static int song_info(int n_songs, int c_song)
        {
            max_songs = n_songs;
            current_song = c_song;
            return 0;
        }
        
    }
}
