using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.DiffGenerators.Differences;
using System.Drawing;

namespace XNB2CP.DiffGenerators.Generators
{
    internal class ImageDifferenceGenerator
    {
        public static ImageDifference GetDifference(string needle, string haystack)
        {
            needle = Path.ChangeExtension(needle, "png");
            haystack = Path.ChangeExtension(haystack, "png");


            Bitmap needleImage = new Bitmap(Image.FromFile(needle));
            Bitmap haystackImage = new Bitmap(Image.FromFile(haystack));

            if (needleImage.Width > haystackImage.Width)
            {
                return new ImageDifference(true);
            }

            IList<Area> areas = new List<Area>();

            if (needleImage.Height > haystackImage.Height)
            {
                Area a = new Area(0, haystackImage.Height)
                {
                    Width = needleImage.Width,
                    Height = needleImage.Height - haystackImage.Height
                };
                areas.Add(a);
            }


            IList<Tuple<int, int>> differentPixels = FindDifferentPixels(needleImage, haystackImage);
            GetDifferentAreas(differentPixels, areas);

            int ppp = areas.Select(item => item.Width * item.Height).Aggregate(0, (acc, x) => acc + x);

            Console.WriteLine($"Total Number of pixels replaced: {ppp}");
            return new ImageDifference(areas);
        }

        private static void GetDifferentAreas(IList<Tuple<int, int>> differentPixels, IList<Area> areas)
        {
            Console.WriteLine($"Getting Areas - diff pixels count: {differentPixels.Count}");

            while (differentPixels.Count != 0)
            {
                Console.WriteLine($"Current diff pixels count: {differentPixels.Count}");
                Tuple<int, int> pixel = differentPixels[0];

                int pixelX = pixel.Item1;
                int pixelY = pixel.Item2;
                Area area = new Area(pixelX, pixelY);

                Console.WriteLine($"Starting at {pixelX}, {pixelY}");

                while (differentPixels.Contains(new Tuple<int, int>(pixelX, pixelY)))
                    pixelX++;
                //pixelX--;

                Console.WriteLine($"Expanded to {pixelX}, {pixelY}");

                while (differentPixels.Contains(new Tuple<int, int>(pixelX, pixelY)))
                    pixelY++;
               // pixelY--;

                Console.WriteLine($"Expanded to {pixelX}, {pixelY}");

                area.Width = Math.Max(1,pixelX - pixel.Item1);
                area.Height = Math.Max(1, pixelY - pixel.Item2);
                areas.Add(area);

                Console.WriteLine($"Found area x:{area.Left} y:{area.Top} w:{area.Width} h:{area.Height}");

                for (int i = 0; i < differentPixels.Count; i++)
                    if (AreaContainsPixel(area, differentPixels[i]))
                    {
                        differentPixels.RemoveAt(i);
                        i--;
                    }
            }
        }

        private static bool AreaContainsPixel(Area area, Tuple<int, int> pixel)
        {
            return pixel.Item1 >= area.Left && pixel.Item1 < area.Left + area.Width
                && pixel.Item2 >= area.Top && pixel.Item2 < area.Top + area.Height;
        }

        private static IList<Tuple<int, int>> FindDifferentPixels(Bitmap toExplore, Bitmap reference)
        {
            IList<Tuple<int, int>> differentPixels = new List<Tuple<int, int>>();
            for (int x = 0; x < toExplore.Width; x++)
                for (int y = 0; y < toExplore.Height; y++)
                {
                    Tuple<int, int> currPos = new Tuple<int, int>(x, y);
                    if (y < reference.Height) //different
                    {
                        if(x == 119 && y == 96)
                        {
                            Console.WriteLine($"{toExplore.GetPixel(x, y).R},{toExplore.GetPixel(x, y).G},{toExplore.GetPixel(x, y).B},{toExplore.GetPixel(x, y).A} - {reference.GetPixel(x, y).R},{reference.GetPixel(x, y).G},{reference.GetPixel(x, y).B},{reference.GetPixel(x, y).A}");
                            Console.WriteLine($"{AreEqual(toExplore.GetPixel(x, y), reference.GetPixel(x, y))}");
                        }
                        if (!AreEqual(toExplore.GetPixel(x, y), reference.GetPixel(x, y)))
                            differentPixels.Add(currPos);
                    }
                }

            return differentPixels;
        }

        private static bool AreEqual(Color a, Color b)
        {
            if(a.A < 3 && b.A < 3)
            {
                return true;
            }
                
            return a.ToArgb() == b.ToArgb();
        }

    }
}
