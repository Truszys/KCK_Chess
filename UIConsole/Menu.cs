using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIConsole
{
    internal class Menu
    {
        public int BoxLeft { get; set; }
        public int BoxTop { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String Title { get; set; }
        public List<String> Options { get; set; }
        public int SelectedItem { get; set; }

        public Menu(string title, List<string> options, int selectedItem = 0)
        {
            Title = title;
            Options = options;
            SelectedItem = selectedItem;
            CalcDimentions();
            DrawMenu();
        }
        private void CalcDimentions()
        {
            var maxWidth = Title.Length;
            foreach (var option in Options)
            {
                if (option.Length > maxWidth) maxWidth = option.Length;
            }
            Width = maxWidth + 6;
            Height = 5 + 2 * Options.Count + 5;
            BoxLeft = (Console.WindowWidth - Width) / 2;
            BoxTop = (Console.WindowHeight - Height) / 2;
        }
        private void DrawMenu()
        {
            /**
             * chars ╔  ╗ ═ ╚  ╝ ╟ ╢ ║ ═  ─  */
            Console.ResetColor();
            Console.Clear();
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(BoxLeft, BoxTop + i);
                SetMenuBorderColors();
                if (i == 0 || i == Height - 1)
                {
                    Console.Write(i == 0 ? '╔' : '╚');
                    for (int j = 1; j < Width - 1; j++)
                    {
                        Console.Write('═');
                    }
                    Console.Write(i == 0 ? '╗' : '╝');
                }
                else
                {
                    Console.Write(i != 4 ? '║' : '╟');
                    for (int j = 1; j < Width - 1; j++)
                    {
                        if (i == 4)
                        {
                            Console.Write('─');
                        }
                        else
                        {
                            SetMenuContentColors();
                            Console.Write(' ');
                        }
                    }
                    SetMenuBorderColors();
                    Console.Write(i != 4 ? '║' : '╢');
                }
            }
            FillMenu();
        }
        private void FillMenu()
        {
            SetMenuContentColors();
            Console.SetCursorPosition(BoxLeft + (Width - Title.Length) / 2, BoxTop + 2);
            Console.Write(Title);
            for (int i = 0; i <= Options.Count; i++)
            {
                if (i < Options.Count)
                {
                    Console.SetCursorPosition(BoxLeft + (Width - Options[i].Length) / 2, BoxTop + 6 + i * 2);
                    Console.Write(Options[i]);
                }
                else
                {
                    Console.SetCursorPosition(BoxLeft + (Width - "Quit".Length) / 2, BoxTop + 6 + i * 2);
                    Console.Write("Quit");
                }
            }
        }
        public int HandleMenu()
        {
            int hover = ChangeHover();
            ConsoleKey key;
            while (true)
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (SelectedItem <= Options.Count)
                        {
                            SelectedItem++;
                        }
                        else
                        {
                            SelectedItem = 0;
                        }
                        hover = ChangeHover(hover);
                        break;
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (SelectedItem > 0)
                        {
                            SelectedItem--;
                        }
                        else
                        {
                            SelectedItem = Options.Count;
                        }
                        hover = ChangeHover(hover);
                        break;
                    case ConsoleKey.Enter:
                        return SelectedItem;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Backspace:
                        return Options.Count;
                    default:
                        break;
                }
            }
        }
        private int ChangeHover(int? oldHover = null)
        {
            SetMenuContentColors();
            if (oldHover != null)
            {
                var str = (int)oldHover < Options.Count ? Options[(int)oldHover] : "Quit";
                Console.SetCursorPosition(BoxLeft + (Width - str.Length) / 2, BoxTop + 6 + (int)oldHover * 2);
                Console.Write(str);
            }
            SetMenuContentHoverColors();
            var strHover = SelectedItem < Options.Count ? Options[SelectedItem] : "Quit";
            Console.SetCursorPosition(BoxLeft + (Width - strHover.Length) / 2, BoxTop + 6 + SelectedItem * 2);
            Console.Write(strHover);
            return SelectedItem;
        }






        private void SetMenuContentColors()
        {
            Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black;
        }
        private void SetMenuContentHoverColors()
        {
            Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.DarkBlue;
        }
        private void SetMenuBorderColors()
        {
            Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.DarkGray;
        }
    }
}
