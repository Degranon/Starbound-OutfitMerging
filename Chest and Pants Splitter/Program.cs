using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Chest_and_Pants_Splitter
{
    class Program
    {
        private const int ENTER = 13;
        private const int FRAME_SIZE = 43;
        private const int PANTS_WIDTH = 387;
        private const int PANTS_HEIGHT = 258;
        private const int PANTS_OLD_HEIGHT = 301;
        private const int CHEST_WIDTH = 86;
        private const int CHEST_HEIGHT = 258;
        private const int SLEEVES_WIDTH = 387;
        private const int SLEEVES_HEIGHT = 301;

        private static Size chestSize = new Size(CHEST_WIDTH, CHEST_HEIGHT);
        private static Size pantsSize = new Size(PANTS_WIDTH, PANTS_HEIGHT);
        private static Size pantsOldSize = new Size(PANTS_WIDTH, PANTS_OLD_HEIGHT);
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

            if (!chestBitmap.Size.Equals(chestSize) || ! (pantsBitmap.Size.Equals(pantsSize) || pantsBitmap.Size.Equals(pantsOldSize)) )
            {
                Console.WriteLine("Incorrent size!");
                return null;
            }

            return ApplyMultingChestPants(chestBitmap, pantsBitmap);
        }

        private static Bitmap ApllyMultingSleeves(Bitmap frontSleeves, Bitmap backSleeves)
        {
            Bitmap result = new Bitmap(SLEEVES_WIDTH, SLEEVES_HEIGHT * 2);

            Superimpose(ref result, frontSleeves, 0, 0);
            Superimpose(ref result, backSleeves, 0, SLEEVES_HEIGHT);

            return result;
        }

        private static Bitmap ApplyMultingChestPants(Bitmap chest, Bitmap pants)
        {
            Bitmap result = new Bitmap(pants);

            Bitmap chestIdle = chest.Clone(new Rectangle(FRAME_SIZE, 0, FRAME_SIZE, FRAME_SIZE), chest.PixelFormat);
            Bitmap chestIdle2 = chest.Clone(new Rectangle(0, FRAME_SIZE, FRAME_SIZE, FRAME_SIZE), chest.PixelFormat);
            Bitmap chestIdle3 = chest.Clone(new Rectangle(FRAME_SIZE, FRAME_SIZE, FRAME_SIZE, FRAME_SIZE), chest.PixelFormat);

            Bitmap chestRun = chest.Clone(new Rectangle(FRAME_SIZE, FRAME_SIZE * 2, FRAME_SIZE, FRAME_SIZE), chest.PixelFormat);

            Bitmap chestDuck = chest.Clone(new Rectangle(FRAME_SIZE, FRAME_SIZE * 3, FRAME_SIZE, FRAME_SIZE), chest.PixelFormat);

            Bitmap chestClimb = chest.Clone(new Rectangle(FRAME_SIZE, FRAME_SIZE * 4, FRAME_SIZE, FRAME_SIZE), chest.PixelFormat);
            Bitmap chestSwim = chest.Clone(new Rectangle(FRAME_SIZE, FRAME_SIZE * 5, FRAME_SIZE, FRAME_SIZE), chest.PixelFormat);

            // Personality 1,5
            Superimpose(ref result, chestIdle, FRAME_SIZE, 0);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 5, 0);

            // Personality 2,4
            Superimpose(ref result, chestIdle2, FRAME_SIZE * 2, 0);
            Superimpose(ref result, chestIdle2, FRAME_SIZE * 4, 0);

            // Personality 3
            Superimpose(ref result, chestIdle3, FRAME_SIZE * 3, 0);

            // Duck
            Superimpose(ref result, chestDuck, 344, 0);

            // Sit
            Superimpose(ref result, chestIdle, 258, 1);

            // Walking
            Superimpose(ref result, chestIdle, FRAME_SIZE, FRAME_SIZE + 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 2, FRAME_SIZE + 2);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 3, FRAME_SIZE + 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 4, FRAME_SIZE);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 5, FRAME_SIZE + 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 6, FRAME_SIZE + 2);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 7, FRAME_SIZE + 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 8, FRAME_SIZE);

            // Running
            Superimpose(ref result, chestRun, FRAME_SIZE, FRAME_SIZE * 2);
            Superimpose(ref result, chestRun, FRAME_SIZE * 2, FRAME_SIZE * 2 - 1);
            Superimpose(ref result, chestRun, FRAME_SIZE * 3, FRAME_SIZE * 2);
            Superimpose(ref result, chestRun, FRAME_SIZE * 4, FRAME_SIZE * 2 + 1);
            Superimpose(ref result, chestRun, FRAME_SIZE * 5, FRAME_SIZE * 2);
            Superimpose(ref result, chestRun, FRAME_SIZE * 6, FRAME_SIZE * 2 - 1);
            Superimpose(ref result, chestRun, FRAME_SIZE * 7, FRAME_SIZE * 2);
            Superimpose(ref result, chestRun, FRAME_SIZE * 8, FRAME_SIZE * 2 + 1);

            // Jumping
            Superimpose(ref result, chestIdle, FRAME_SIZE, FRAME_SIZE * 3 - 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 2, FRAME_SIZE * 3 - 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 3, FRAME_SIZE * 3 - 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 4, FRAME_SIZE * 3 - 1);

            // Falling
            Superimpose(ref result, chestIdle, FRAME_SIZE * 5, FRAME_SIZE * 3 - 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 6, FRAME_SIZE * 3 - 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 7, FRAME_SIZE * 3 - 1);
            Superimpose(ref result, chestIdle, FRAME_SIZE * 8, FRAME_SIZE * 3 - 1);

            // Climbing
            Superimpose(ref result, chestClimb, FRAME_SIZE, FRAME_SIZE * 4);
            Superimpose(ref result, chestClimb, FRAME_SIZE * 2, FRAME_SIZE * 4);
            Superimpose(ref result, chestClimb, FRAME_SIZE * 3, FRAME_SIZE * 4);
            Superimpose(ref result, chestClimb, FRAME_SIZE * 4, FRAME_SIZE * 4);
            Superimpose(ref result, chestClimb, FRAME_SIZE * 5, FRAME_SIZE * 4);
            Superimpose(ref result, chestClimb, FRAME_SIZE * 6, FRAME_SIZE * 4);
            Superimpose(ref result, chestClimb, FRAME_SIZE * 7, FRAME_SIZE * 4);
            Superimpose(ref result, chestClimb, FRAME_SIZE * 8, FRAME_SIZE * 4);

            // Swimming
            Superimpose(ref result, chestSwim, FRAME_SIZE, FRAME_SIZE * 5);
            Superimpose(ref result, chestSwim, FRAME_SIZE * 4, FRAME_SIZE * 5);
            Superimpose(ref result, chestSwim, FRAME_SIZE * 5, FRAME_SIZE * 5 + 1);
            Superimpose(ref result, chestSwim, FRAME_SIZE * 6, FRAME_SIZE * 5 + 2);
            Superimpose(ref result, chestSwim, FRAME_SIZE * 7, FRAME_SIZE * 5 + 1);

            return result;
        }

        private static void Superimpose(ref Bitmap largeBmp, Bitmap smallBmp, int x, int y)
        {
            Graphics g = Graphics.FromImage(largeBmp);
            g.DrawImage(smallBmp, x, y, smallBmp.Width, smallBmp.Height);
        }
    }
}
