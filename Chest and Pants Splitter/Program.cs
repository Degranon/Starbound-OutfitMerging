using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Chest_and_Pants_Splitter
{
    class Program
    {
        private const int ENTER = 13;
        private const int PANTS_WIDTH = 387;
        private const int PANTS_HEIGHT = 258;
        private const int CHEST_WIDTH = 86;
        private const int CHEST_HEIGHT = 258;
        private const int SLEEVES_WIDTH = 387;
        private const int SLEEVES_HEIGHT = 301;

        private static Size chestSize = new Size(CHEST_WIDTH, CHEST_HEIGHT);
        private static Size pantsSize = new Size(PANTS_WIDTH, PANTS_HEIGHT);
        private static Size sleevesSize = new Size(SLEEVES_WIDTH, SLEEVES_HEIGHT);

        static void Main(string[] args)
        {
            bool done = false;
            string mode = null;

            Bitmap result = null;

            if (args.Length != 2)
            {
                Console.WriteLine("Incorrect usage: path1 path2");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Are we blending chest and pants or sleeves? Press the respective button");
            Console.WriteLine("[1] Chest and pants");
            Console.WriteLine("[2] Sleeves");

            try
            {
                while (!done)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.D1:
                            {
                                result = ChestAndPants(args[0], args[1]);
                                mode = "chestpants";
                                done = true;
                                break;
                            }
                        case ConsoleKey.D2:
                            {
                                result = Sleeves(args[0], args[1]);
                                mode = "sleeves";
                                done = true;
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("It's either 1 or 2, babe");
                                break;
                            }
                    }
                }


                if (result == null)
                {
                    Console.WriteLine("An error has occured");
                }
                else
                {
                    string name = mode + DateTime.Now.ToString(" MM.dd h.mm.ss") + ".png";
                    result.Save(name);
                    Console.WriteLine("Done saving, check \"" + name + "\"");
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Couldn't open the file");
            }
            Console.ReadKey(true);
            return;
        }

        private static Bitmap Sleeves(string firstPath, string secondPath)
        {
            Bitmap frontSleeves;
            Bitmap backSleeves;

            Console.WriteLine("Is the order correct?");
            Console.WriteLine("front sleeve image: " + firstPath);
            Console.WriteLine("back sleeve image: " + secondPath);
            Console.WriteLine("Press Enter if it is, press any key otherwise");


            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                frontSleeves = new Bitmap(firstPath);
                backSleeves = new Bitmap(secondPath);
            }
            else
            {
                frontSleeves = new Bitmap(secondPath);
                backSleeves = new Bitmap(firstPath);
            }

            if (!frontSleeves.Size.Equals(sleevesSize) || !backSleeves.Size.Equals(sleevesSize))
            {
                Console.WriteLine("Incorrent size!");
                return null;
            }

            return ApllyMultingSleeves(frontSleeves, backSleeves);
        }

        private static Bitmap ChestAndPants(string firstPath, string secondPath)
        {
            Bitmap chestBitmap;
            Bitmap pantsBitmap;

            Console.WriteLine("Is the order correct?");
            Console.WriteLine("Chest image: " + firstPath);
            Console.WriteLine("Pants image: " + secondPath);
            Console.WriteLine("Press Enter if it is, press any key otherwise");

            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                chestBitmap = new Bitmap(firstPath);
                pantsBitmap = new Bitmap(secondPath);
            }
            else
            {
                chestBitmap = new Bitmap(secondPath);
                pantsBitmap = new Bitmap(firstPath);
            }

            if (!chestBitmap.Size.Equals(chestSize) || !pantsBitmap.Size.Equals(pantsSize))
            {
                Console.WriteLine("Incorrent size!");
                return null;
            }

            return ApplyMultingChestPants(chestBitmap, pantsBitmap);
        }

        private static Bitmap ApllyMultingSleeves(Bitmap frontSleeves, Bitmap backSleeves)
        {
            Bitmap result = new Bitmap(386, 602);

            Superimpose(ref result, frontSleeves, 0, 0);
            Superimpose(ref result, backSleeves, 0, 301);

            return result;
        }

        private static Bitmap ApplyMultingChestPants(Bitmap chest, Bitmap pants)
        {
            Bitmap result = new Bitmap(pants);

            Bitmap chestIdle = chest.Clone(new Rectangle(43, 0, 43, 43), chest.PixelFormat);
            Bitmap chestIdle2 = chest.Clone(new Rectangle(0, 43, 43, 43), chest.PixelFormat);
            Bitmap chestIdle3 = chest.Clone(new Rectangle(43, 43, 43, 43), chest.PixelFormat);

            Bitmap chestRun = chest.Clone(new Rectangle(43, 86, 43, 43), chest.PixelFormat);

            Bitmap chestDuck = chest.Clone(new Rectangle(43, 129, 43, 43), chest.PixelFormat);

            Bitmap chestClimb = chest.Clone(new Rectangle(43, 172, 43, 43), chest.PixelFormat);
            Bitmap chestSwim = chest.Clone(new Rectangle(43, 215, 43, 43), chest.PixelFormat);

            // Personality 1,5
            Superimpose(ref result, chestIdle, 43, 0);
            Superimpose(ref result, chestIdle, 215, 0);

            // Personality 2,4
            Superimpose(ref result, chestIdle2, 86, 0);
            Superimpose(ref result, chestIdle2, 172, 0);

            // Personality 3
            Superimpose(ref result, chestIdle3, 129, 0);

            // Duck
            Superimpose(ref result, chestDuck, 344, 0);

            // Sit
            Superimpose(ref result, chestIdle, 258, 1);

            // Walking
            Superimpose(ref result, chestIdle, 43, 44);
            Superimpose(ref result, chestIdle, 86, 45);
            Superimpose(ref result, chestIdle, 129, 44);
            Superimpose(ref result, chestIdle, 172, 43);
            Superimpose(ref result, chestIdle, 215, 44);
            Superimpose(ref result, chestIdle, 258, 45);
            Superimpose(ref result, chestIdle, 301, 44);
            Superimpose(ref result, chestIdle, 344, 43);

            // Running
            Superimpose(ref result, chestRun, 43, 86);
            Superimpose(ref result, chestRun, 86, 85);
            Superimpose(ref result, chestRun, 129, 86);
            Superimpose(ref result, chestRun, 172, 87);
            Superimpose(ref result, chestRun, 215, 86);
            Superimpose(ref result, chestRun, 258, 85);
            Superimpose(ref result, chestRun, 301, 86);
            Superimpose(ref result, chestRun, 344, 87);

            // Jumping
            Superimpose(ref result, chestIdle, 43, 128);
            Superimpose(ref result, chestIdle, 86, 128);
            Superimpose(ref result, chestIdle, 129, 128);
            Superimpose(ref result, chestIdle, 172, 128);

            // Falling
            Superimpose(ref result, chestIdle, 215, 128);
            Superimpose(ref result, chestIdle, 258, 128);
            Superimpose(ref result, chestIdle, 301, 128);
            Superimpose(ref result, chestIdle, 344, 128);

            // Climbing
            Superimpose(ref result, chestClimb, 43, 172);
            Superimpose(ref result, chestClimb, 86, 172);
            Superimpose(ref result, chestClimb, 129, 172);
            Superimpose(ref result, chestClimb, 172, 172);
            Superimpose(ref result, chestClimb, 215, 172);
            Superimpose(ref result, chestClimb, 258, 172);
            Superimpose(ref result, chestClimb, 301, 172);
            Superimpose(ref result, chestClimb, 344, 172);

            // Swimming
            Superimpose(ref result, chestSwim, 43, 215);
            Superimpose(ref result, chestSwim, 172, 215);
            Superimpose(ref result, chestSwim, 215, 216);
            Superimpose(ref result, chestSwim, 258, 217);
            Superimpose(ref result, chestSwim, 301, 216);

            return result;
        }

        private static void Superimpose(ref Bitmap largeBmp, Bitmap smallBmp, int x, int y)
        {
            Graphics g = Graphics.FromImage(largeBmp);
            g.DrawImage(smallBmp, x, y, smallBmp.Width, smallBmp.Height);
        }
    }
}
