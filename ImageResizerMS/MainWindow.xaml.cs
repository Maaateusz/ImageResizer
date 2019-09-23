using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
//using System.Windows;
//using System.Windows.Interop;
//using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageResizerMS
{
    public partial class MainWindow : System.Windows.Window
    {
        private int CustomHeight = 1080;
        private int CustomWidth = 1920;
        private bool AspecRatio = true;
        private int Quality = 90;
        private List<string> SelectedImagesPath;
        private string savePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region resize functions
        public Bitmap ResizeImage1(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public Image ResizeImage2(int newWidth, int newHeight, string stPhotoPath)
        {
            Image imgPhoto = Image.FromFile(stPhotoPath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }

        public Bitmap ResizeImage2_Transparent2White(Bitmap bitmap, int newWidth, int newHeight)
        {
            int sourceWidth = bitmap.Width;
            int sourceHeight = bitmap.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(bitmap.HorizontalResolution,
                         bitmap.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.White);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(bitmap,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }

        private Image ResizeImage3(Image img, double maxWidth, double maxHeight)
        {
            double resizeWidth = img.Width;
            double resizeHeight = img.Height;

            double aspect = resizeWidth / resizeHeight;

            if (resizeWidth > maxWidth)
            {
                resizeWidth = maxWidth;
                resizeHeight = resizeWidth / aspect;
            }
            if (resizeHeight > maxHeight)
            {
                aspect = resizeWidth / resizeHeight;
                resizeHeight = maxHeight;
                resizeWidth = resizeHeight * aspect;
            }
            Image img2 = img;
            /*img.Width = resizeWidth;
            img.Height = resizeHeight;*/
            return img2;
        }

        public Bitmap ResizeBitmap(Bitmap bitmap, int heigth, int width, Boolean keepAspectRatio, Boolean getCenter)
        {
            int newheigth = heigth;
            System.Drawing.Image FullsizeImage = bitmap;
            if (keepAspectRatio || getCenter)
            {
                int bmpY = 0;
                double resize = (double)FullsizeImage.Width / (double)width;//get the resize vector
                if (getCenter)
                {
                    bmpY = (int)((FullsizeImage.Height - (heigth * resize)) / 2);// gives the Y value of the part that will be cut off, to show only the part in the center
                    Rectangle section = new Rectangle(new Point(0, bmpY), new Size(FullsizeImage.Width, (int)(heigth * resize)));// create the section to cut of the original image
                                                                                                                                 //System.Console.WriteLine("the section that will be cut off: " + section.Size.ToString() + " the Y value is minimized by: " + bmpY);
                    Bitmap orImg = new Bitmap((Bitmap)FullsizeImage);//for the correct effect convert image to bitmap.
                    FullsizeImage.Dispose();//clear the original image
                    using (Bitmap tempImg = new Bitmap(section.Width, section.Height))
                    {
                        Graphics cutImg = Graphics.FromImage(tempImg);//              set the file to save the new image to.
                        cutImg.DrawImage(orImg, 0, 0, section, GraphicsUnit.Pixel);// cut the image and save it to tempImg
                        FullsizeImage = tempImg;//save the tempImg as FullsizeImage for resizing later
                        orImg.Dispose();
                        cutImg.Dispose();
                        return new Bitmap(FullsizeImage.GetThumbnailImage(width, heigth, null, IntPtr.Zero));
                    }
                }
                else newheigth = (int)(FullsizeImage.Height / resize);//  set the new heigth of the current image
            }//return the image resized to the given heigth and width
            return new Bitmap(FullsizeImage.GetThumbnailImage(width, newheigth, null, IntPtr.Zero));
        }

        public Bitmap Resize(Bitmap bitmap)
        {
            float width = CustomWidth;
            float height = CustomHeight;
            var brush = new SolidBrush(System.Drawing.Color.Black);
            var image = new Bitmap(bitmap);
            float scale = Math.Min(width / image.Width, height / image.Height);

            var bmp = new Bitmap((int)width, (int)height);
            var graph = Graphics.FromImage(bmp);

            // uncomment for higher quality output
            //graph.InterpolationMode = InterpolationMode.High;
            //graph.CompositingQuality = CompositingQuality.HighQuality;
            //graph.SmoothingMode = SmoothingMode.AntiAlias;

            var scaleWidth = (int)(image.Width * scale);
            var scaleHeight = (int)(image.Height * scale);

            //graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
            graph.DrawImage(image, ((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight);

            return bmp;
        }

        public Bitmap Resize2(Bitmap bitmap)
        {
            Bitmap resized = new Bitmap(bitmap, new Size(CustomWidth, CustomHeight));
            return resized;
        }
        #endregion

        #region convert functions
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            return new Bitmap(bitmapImage.StreamSource);
        }

        private Bitmap BitmapImage2Bitmap2(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        private BitmapSource Bitmap2BitmapSource(Bitmap bitmap)
        {
            BitmapSource i = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                           bitmap.GetHbitmap(),
                           IntPtr.Zero,
                           System.Windows.Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            return (BitmapSource)i;
        }
        #endregion

        #region save images
        private void SaveImage(Image img)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                img.Save(dialog.FileName);
                //img.Save(dialog.FileName, img.RawFormat); 
            }
        }

        private void SaveBitmap(Bitmap bitmap)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            //dialog.InitialDirectory = @"c:\temp\";
            //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (dialog.ShowDialog() == true)
            {
                using (Bitmap bmp1 = new Bitmap(bitmap))
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmp1.Save(dialog.FileName + "1.jpg", jpgEncoder, myEncoderParameters);

                    /*myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmp1.Save(dialog.FileName + "2.jpg", jpgEncoder, myEncoderParameters);

                    myEncoderParameter = new EncoderParameter(myEncoder, 0L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmp1.Save(dialog.FileName + "3.jpg", jpgEncoder, myEncoderParameters);*/
                }
                //bitmap.Save(dialog.FileName);
            }
        }

        private string SaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            //dialog.InitialDirectory = @"c:\temp\";
            //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            else return null;
        }

        private void SaveMultiBitmap(List<Bitmap> bitmapImages, int quality)
        {
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality); //90L
            myEncoderParameters.Param[0] = myEncoderParameter;
            int i = 0;
            foreach (Bitmap bitmap in bitmapImages)
            {
                bitmap.Save(savePath + i + ".jpg", jpgEncoder, myEncoderParameters);
                bitmap.Dispose();
                i++;
            }
            myEncoderParameter.Dispose();
            myEncoderParameters.Dispose();
        }
        #endregion

        private void BtnResizeImages1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomHeight = Convert.ToInt32(textBoxHeight.Text);
            CustomWidth = Convert.ToInt32(textBoxWidth.Text);
            AspecRatio = checkBoxRatio.IsChecked.Value;
            Quality = Convert.ToInt32(sliderQuality.Value);

            savePath = SaveDialog();
            List<Bitmap> bitmapImages = CreateBitmaps();
            List<Bitmap> bitmapResizedImages = new List<Bitmap>();

            foreach (Bitmap bitmap in bitmapImages)
            {
                bitmapResizedImages.Add(ResizeBitmap(bitmap, CustomHeight, CustomWidth, AspecRatio, true));
            }

            SaveMultiBitmap(bitmapResizedImages, Quality);
        }

        private void BtnOpenImages_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SelectImages();
        }

        #region open and create images
        private void SelectImages()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = @"c:\temp\";
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.JPG)|*.png;*.jpeg;*.jpg;*.JPG|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImagesPath = new List<string>();
                int i = 0;
                foreach (string fileName in openFileDialog.FileNames)
                {
                    SelectedImagesPath.Add(fileName);
                    listBoxImages.Items.Add(openFileDialog.SafeFileNames[i]);
                    i++;
                }
                //listBoxImages.ItemsSource = ImagesSelected;
                //Uri fileUri = new Uri(openFileDialog.FileName);
                //string selectedFileName = openFileDialog.FileName;
                //BitmapImage img = new BitmapImage(fileUri);
                //Image img2 = Image.FromFile(openFileDialog.FileName);
                //BitmapSource = new Bitmap(openFileDialog.FileName);
                //ImageSource = img2;
                //BitmapImageSource = img;
                //imageTest.Source = img;
            }
        }

        private List<Bitmap> CreateBitmaps()
        {
            List<Bitmap> bitmapList = new List<Bitmap>();
            foreach (string imagePath in SelectedImagesPath)
            {
                bitmapList.Add(new Bitmap(imagePath));
            }
            return bitmapList;
        }
        #endregion

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void GrayScale()
        {
            /*BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = Image;
            bitmap.EndInit();*/

            FormatConvertedBitmap grayBitmapSource = new FormatConvertedBitmap();
            grayBitmapSource.BeginInit();
            //grayBitmapSource.Source = BitmapImageSource;
            grayBitmapSource.DestinationFormat = PixelFormats.Gray32Float;
            grayBitmapSource.EndInit();

            /*Image grayImage = new Image();
            grayImage.Width = 300;
            grayImage.Source = grayBitmapSource;*/

            //imageTest.Source = grayBitmapSource;

        }

    }
}
//https://docs.microsoft.com/en-us/dotnet/framework/winforms/advanced/how-to-set-jpeg-compression-level?redirectedfrom=MSDN
//https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp