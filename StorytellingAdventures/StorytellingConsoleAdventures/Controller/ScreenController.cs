using System;

using System.Runtime.InteropServices;

namespace StorytellingConsoleAdventures.Controller
{
    static class ScreenController
    {
        /*
         * Code from: https://www.c-sharpcorner.com/code/448/code-to-auto-maximize-console-application-according-to-screen-width-in-c-sharp.aspx
         */
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;


        public static void GoToFullScreen()
        {
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
        }
    }
}
