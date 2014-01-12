using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

namespace ludum_dare_24
{
    class FontText
    {
        public static SpriteFont font;
        public static string[] TextIndex = new string[1000];
        public static int[,] TextPosition = new int[1000, 2];
        public static Color[] TextColor = new Color[1000];
        public static string[] name = new string[1000];
        public static int textsingame = 0;
        public static int[] time = new int[1000];
        public static bool[] DoesMoveToScore = new bool[1000];

        public static void AddText(string text, string textname, int x, int y, Color color, int Time)
        {
            for (int a = 0; a < 1000; a++)
            {
                if (name[a] == textname)
                {
                    TextIndex[a] = text;
                    TextPosition[a, 0] = x;
                    TextPosition[a, 1] = y;
                    TextColor[a] = color;
                    name[a] = textname;
                    time[a] = Time;
                    DoesMoveToScore[a] = false;
                    if (Time == -1)
                    {
                        time[a] = 600;
                        DoesMoveToScore[a] = true;
                    }
                    goto next;
                }
                if (name[a] == " ")
                {
                    TextIndex[a] = text;
                    TextPosition[a, 0] = x;
                    TextPosition[a, 1] = y;
                    TextColor[a] = color;
                    name[a] = textname;
                    time[a] = Time;
                    DoesMoveToScore[a] = false;
                    if (Time == -1)
                    {
                        time[a] = 600;
                        DoesMoveToScore[a] = true;
                    }
                    goto next;
                }
            }
            TextIndex[textsingame] = text;
            TextPosition[textsingame, 0] = x;
            TextPosition[textsingame, 1] = y;
            TextColor[textsingame] = color;
            name[textsingame] = textname;
            time[textsingame] = Time;
            DoesMoveToScore[textsingame] = false;
            textsingame++;
            if (Time == -1)
            {
                time[textsingame - 1] = 600;
                DoesMoveToScore[textsingame - 1] = true;
            }
        next:

            return;
        }

        public static void RemoveText(string textname)
        {
            for (int a = 0; a < 1000; a++)
            {
                if (name[a] == textname)
                {
                    name[a] = " ";
                    TextIndex[a] = " ";
                }
            }
        }

        public static void Draw(SpriteBatch thespritebatch)
        {

            for (int a = 0; a < textsingame; a++)
            {
                if (name[a].Contains("scorestuff"))
                {
                    TextPosition[a, 1]--;
                }
                if (DoesMoveToScore[a] == true)
                {
                    for (int b = 0; b < textsingame; b++)
                    {
                        if (name[b] == "Score")
                        {
                            if (TextPosition[a, 0] > TextPosition[b, 0])
                            {
                                TextPosition[a, 0] -= 2;
                            }
                            if (TextPosition[a, 1] > TextPosition[b, 1] + 20)
                            {
                                TextPosition[a, 1] -= 2;
                            }
                            if (TextPosition[a, 0] < TextPosition[b, 0])
                            {
                                TextPosition[a, 0] += 2;
                            }
                            if (TextPosition[a, 1] < TextPosition[b, 1] + 20)
                            {
                                TextPosition[a, 1] += 2;
                            }
                            if (TextPosition[a, 0] > TextPosition[b, 0] - 2 &&
                                TextPosition[a, 0] < TextPosition[b, 0] + 2 &&
                                TextPosition[a, 1] < TextPosition[b, 1] + 22 &&
                                TextPosition[a, 1] > TextPosition[b, 1] + 18)
                            {
                                string[] split = TextIndex[a].Split('+');
                                if (split.Length > 1 && split[1] != null && split[1] != " ")
                                {
                                    //Game1.totalscore += Convert.ToInt32(split[1]);
                                }
                                RemoveText(name[a]);
                            }
                        }
                    }
                }
                if (FontText.time[a] > 1)
                {
                    FontText.time[a]--;
                }
                if (FontText.time[a] > 1 || FontText.time[a] == 0)
                {
                    thespritebatch.DrawString(FontText.font, FontText.TextIndex[a], new Vector2(FontText.TextPosition[a, 0], FontText.TextPosition[a, 1]), FontText.TextColor[a]);
                }
                if (FontText.time[a] == 1)
                {
                    FontText.RemoveText(FontText.name[a]);
                }
            }

        }
    }
}
