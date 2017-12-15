using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace MNISTLoader
{
    public class MnistLoader
    {
        private string itemPath;
        private int itemAmount;
        private bool isAmountSpecified;
        private int imageWidth;
        private int imageHeigth;



        public MnistLoader()
        {

        }

        #region Label loading
        /// <summary>
        /// Load labels from a specified .gz file
        /// </summary>
        /// <param name="path">The labels file to load from</param>
        /// <returns></returns>
        public double[][] LoadLabels(string path)
        {
            isAmountSpecified = false;
            itemPath = path;
            return labelLoad();
        }

        /// <summary>
        /// Load the labels from a file asked from the user upon calling this method
        /// </summary>
        /// <returns></returns>
        public double[][] LoadLabels()
        {
            isAmountSpecified = false;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MNIST files (*.gz*)|*.gz*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                 itemPath = ofd.FileName;
            }


            return labelLoad();
        }

        /// <summary>
        /// Load specified amount of items from a specified file
        /// </summary>
        /// <param name="path">The label file to load from</param>
        /// <param name="amount">The amount of items to load</param>
        /// <returns></returns>
        public double[][] LoadLabels(string path, int amount)
        {
            isAmountSpecified = true;
            itemAmount = amount;
            itemPath = path;

            return labelLoad();
        }

        /// <summary>
        /// Load specified amount of items from a file asked from the user upon calling this method
        /// </summary>
        /// <param name="amount">The amount of items to load</param>
        /// <returns></returns>
        public double[][] LoadLabels(int amount)
        {
            isAmountSpecified = true;
            itemAmount = amount;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = " MNIST files (*.gz*)|*.gz*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
               itemPath = ofd.FileName;
            }

            return labelLoad();
        }


        private double[][] labelLoad()
        {
            double[][] labels;
        
                using (BinaryReader br = new BinaryReader(new FileStream(itemPath, FileMode.Open)))
                {
                    int magicNumber = br.ReadInt32();
                    int nOfItems = br.ReadInt32();

                    if (!isAmountSpecified) itemAmount = nOfItems;

                    labels = new double[itemAmount][];
                    for (int i = 0; i < itemAmount; i++) labels[i][0] = br.ReadByte();


                }

                return labels;

        }
        #endregion

        #region Image loading
        /// <summary>
        /// Load images from a specified .gz file
        /// </summary>
        /// <param name="path">The file to load from</param>
        /// <returns></returns>
        public double[][] LoadImages(string path)
        {
            isAmountSpecified = false;
            itemPath = path;

            return imageLoad();
        }

        /// <summary>
        /// Load the labels from a file asked from the user upon calling this method
        /// </summary>
        /// <returns></returns>
        public double[][] LoadImages()
        {
            isAmountSpecified = false;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MNIST files (*.gz*)|*.gz*";

            if (ofd.ShowDialog() == DialogResult.OK) itemPath = ofd.FileName;

            return imageLoad();
        }

        /// <summary>
        /// Load specified amount of images from a specified file
        /// </summary>
        /// <param name="path">The file to load from</param>
        /// <param name="amount">The amount of images to load</param>
        /// <returns></returns>
        public double[][] LoadImages(string path, int amount)
        {
            isAmountSpecified = true;
            itemPath = path;
            itemAmount = amount;

            return imageLoad();
        }

        /// <summary>
        /// Load spcified amount of items from a file asked from the user upon calling this method
        /// </summary>
        /// <param name="amount">The amount of images to load</param>
        /// <returns></returns>
        public double[][] LoadImages(int amount)
        {
            isAmountSpecified = true;
            itemAmount = amount;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MNIST files (*.gz*)|*.gz*";

            if (ofd.ShowDialog() == DialogResult.OK) itemPath = ofd.FileName;

            return imageLoad();
        }

        private double[][] imageLoad()
        {
            double[][] images;
            using (BinaryReader br = new BinaryReader(new FileStream(itemPath, FileMode.Open)))
            {
                int magicNumber = br.ReadInt32();
                int nOfImages = br.ReadInt32();
                imageWidth = br.ReadInt32();
                imageHeigth = br.ReadInt32();

                if (!isAmountSpecified) itemAmount = nOfImages;

                images = new double[itemAmount][];
                for(int i = 0; i < itemAmount; i++)
                {
                    for(int j = 0; j < (imageWidth * imageHeigth); j++)
                    {
                        images[i][j] = br.ReadByte();
                    }
                }

            }
            return images;
        }


        #endregion

        #region Visualizer

        /// <summary>
        /// Forms a BitmapImage from the given pixels
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public BitmapImage FormImage(double[] pixels)
        {
            Bitmap bmap = ToBitMap(pixels);

            return BitmapToImageSource(bmap);
        }

        private Bitmap ToBitMap(double[] pixels)
        {
            int pixel = 0;
            Bitmap bmap = new Bitmap(imageWidth, imageHeigth);
            for(int i = 0; i < imageHeigth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    int colour = 255 - (int)pixels[pixel];
                    bmap.SetPixel(j, y, Color.FromArgb(colour, colour, colour));
                    pixel++;
                }
            }
            return bmap;
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
#endregion

    }
}
