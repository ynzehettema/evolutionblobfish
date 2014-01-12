using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ludum_dare_24
{
#if WINDOWS || XBOX
    static class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);
        public static void messagebox(string what, string title)
        {

            MessageBox(new IntPtr(0), what, title, 0);
            return;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                messagebox("Error:\n\n" + e.ToString(), "Something... went wrong");
            }
        }
    }
#endif
}

