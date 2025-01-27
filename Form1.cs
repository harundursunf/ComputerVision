using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KMeansRGB
{
    public partial class Form1 : Form
    {
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "K-Means RGB")
            {
                if (pictureBox1.Image != null)
                {
                    Bitmap inputImage = new Bitmap(pictureBox1.Image);
                    ProcessKMeansRGB(inputImage);
                }
                else
                {
                    MessageBox.Show("Lütfen önce bir resim yükleyin.");
                }
            }
        }

        private void ProcessKMeansRGB(Bitmap image)
        {
            // ... existing code ...

            // Piksel verilerini diziye al - GetPixel yerine LockBits kullanarak hızlandırma
            BitmapData bmpData = image.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int stride = bmpData.Stride;
            int bytes = Math.Abs(stride) * height;
            byte[] rgbValues = new byte[bytes];
            
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
            
            List<Point3D> pixels = new List<Point3D>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idx = y * stride + x * 3;
                    pixels.Add(new Point3D(
                        rgbValues[idx + 2], // R
                        rgbValues[idx + 1], // G
                        rgbValues[idx],     // B
                        x, y));
                }
            }
            
            image.UnlockBits(bmpData);

            // ... existing code ...

            // Sonuç görüntüyü oluştur - SetPixel yerine LockBits kullanarak hızlandırma
            Bitmap result = new Bitmap(width, height);
            BitmapData resultData = result.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            byte[] resultValues = new byte[bytes];
            
            foreach (var pixel in pixels)
            {
                int idx = pixel.Y * stride + pixel.X * 3;
                resultValues[idx + 2] = (byte)centroids[pixel.Cluster].R; // R
                resultValues[idx + 1] = (byte)centroids[pixel.Cluster].G; // G
                resultValues[idx] = (byte)centroids[pixel.Cluster].B;     // B
            }
            
            Marshal.Copy(resultValues, 0, resultData.Scan0, bytes);
            result.UnlockBits(resultData);

            pictureBox2.Image = result;
        }
    }
} 