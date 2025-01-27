using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BilisayarliGormeOdev
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ComboBox1'e işlem seçeneklerini ekle
            comboBox1.Items.AddRange(new string[] {
                "Gri Yap",
                "Y yap",
                "Histogram",
                "KM intensity",
                "KM öklit RGB",
                "KM Mahalonobis",
                "KM Mahalonobis ND",
                "kenar bulma"
            });
            
            // Varsayılan olarak ilk öğeyi seç
            comboBox1.SelectedIndex = 0;
            
            // ComboBox2'ye K değerlerini ekle
            int[] kValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 50 };
            comboBox2.Items.AddRange(kValues.Select(x => x.ToString()).ToArray());
            
            // Varsayılan olarak ilk öğeyi seç
            comboBox2.SelectedIndex = 0;
        }

        private void btnResimYukle_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.Title = "Resim Seç";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Resim yüklenirken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
                return;
            }

            try 
            {
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            Bitmap resultImage = new Bitmap(originalImage.Width, originalImage.Height);
            string selectedMethod = comboBox1.SelectedItem.ToString();

                switch (selectedMethod)
                {
                    case "Gri Yap":
                        try
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            int totalPixels = originalImage.Width * originalImage.Height;
                            int iteration = 1; // Gri yapma işlemi tek iterasyon

                        // Her piksel için RGB değerlerinin ortalamasını al
                        for (int x = 0; x < originalImage.Width; x++)
                        {
                            for (int y = 0; y < originalImage.Height; y++)
                            {
                                Color pixel = originalImage.GetPixel(x, y);
                                int grayValue = (pixel.R + pixel.G + pixel.B) / 3;
                                resultImage.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                            }
                        }

                            watch.Stop();
                            label6.Text = $"İşlem Süresi: {watch.ElapsedMilliseconds} ms";
                            label7.Text = $"İterasyon: {iteration}";
                            label8.Text = $"Toplam Piksel: {totalPixels}";

                        // Gri seviye değerlerini listView1'de göster


                        // Histogram hesapla
                        int[] histogram = new int[256];
                        for (int x = 0; x < resultImage.Width; x++)
                        {
                            for (int y = 0; y < resultImage.Height; y++)
                            {
                                Color pixel = resultImage.GetPixel(x, y);
                                histogram[pixel.R]++; // Gri resimde R=G=B olduğu için herhangi birini kullanabiliriz
                            }
                        }


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Gri dönüşüm işlemi sırasında hata: {ex.Message}");
                        }
                        break;

                    case "Y yap":
                        try
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            int totalPixels = originalImage.Width * originalImage.Height;
                            int iteration = 1; // Y yapma işlemi tek iterasyon

                        // Y dönüşümü için her pikseli işle
                        for (int x = 0; x < originalImage.Width; x++)
                        {
                            for (int y = 0; y < originalImage.Height; y++)
                            {
                                Color pixel = originalImage.GetPixel(x, y);
                                int yValue = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                                resultImage.SetPixel(x, y, Color.FromArgb(yValue, yValue, yValue));
                            }
                        }

                            watch.Stop();
                            label6.Text = $"İşlem Süresi: {watch.ElapsedMilliseconds} ms";
                            label7.Text = $"İterasyon: {iteration}";
                            label8.Text = $"Toplam Piksel: {totalPixels}";

                        // Y değerlerinin histogramını hesapla ve göster

                        // Histogram hesapla
                        int[] yHistogram = new int[256];
                        for (int x = 0; x < resultImage.Width; x++)
                        {
                            for (int y = 0; y < resultImage.Height; y++)
                            {
                                Color pixel = resultImage.GetPixel(x, y);
                                yHistogram[pixel.R]++; // Y dönüşümünde R=G=B olduğu için herhangi birini kullanabiliriz
                            }
                        }


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Y dönüşümü işlemi sırasında hata: {ex.Message}");
                        }
                        break;

                    case "Histogram":
                        try
                        {
                            // Görüntüyü gri seviyeye dönüştür
                            int[,] histGrayValues = new int[originalImage.Width, originalImage.Height];
                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    Color pixel = originalImage.GetPixel(x, y);
                                    histGrayValues[x, y] = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                                }
                            }

                            // Histogram hesapla
                            int[] histFrequency = new int[256];
                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    histFrequency[histGrayValues[x, y]]++;
                                }
                            }

                            // Kümülatif histogram hesapla
                            int[] histCumulative = new int[256];
                            histCumulative[0] = histFrequency[0];
                            for (int i = 1; i < 256; i++)
                            {
                                histCumulative[i] = histCumulative[i - 1] + histFrequency[i];
                            }

                            // Histogram eşitleme için dönüşüm fonksiyonu
                            int totalPixels = originalImage.Width * originalImage.Height;
                            int[] histEqualizationMap = new int[256];
                        for (int i = 0; i < 256; i++)
                        {
                                histEqualizationMap[i] = (int)((histCumulative[i] * 255.0) / totalPixels);
                            }

                            // Eşitlenmiş görüntüyü oluştur
                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    int newValue = histEqualizationMap[histGrayValues[x, y]];
                                    resultImage.SetPixel(x, y, Color.FromArgb(newValue, newValue, newValue));
                                }
                            }



                            // Chart için histogram verilerini ekle
                            chart1.Series.Clear();
                            Series histOriginalSeries = new Series("Histogram");
                            histOriginalSeries.ChartType = SeriesChartType.Column;
                            histOriginalSeries.Color = Color.Blue;

                            for (int i = 0; i < 256; i++)
                            {
                                histOriginalSeries.Points.AddXY(i, histFrequency[i]);
                            }

                            chart1.Series.Add(histOriginalSeries);
                            chart1.ChartAreas[0].AxisX.Title = "Gri Seviye";
                            chart1.ChartAreas[0].AxisY.Title = "Piksel Sayısı";
                            chart1.ChartAreas[0].AxisX.Minimum = 0;
                            chart1.ChartAreas[0].AxisX.Maximum = 255;
                            chart1.ChartAreas[0].AxisX.Interval = 32;
                            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Histogram işlemi sırasında hata: {ex.Message}");
                        }

                        break;

                    case "KM intensity":
                        try
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            int totalPixels = originalImage.Width * originalImage.Height;

                            // K değerini al
                            int kmIntensityK = int.Parse(comboBox2.SelectedItem.ToString());

                            // Görüntüyü gri seviyeye çevir ve intensity değerlerini al
                            int[,] intensityValues = new int[originalImage.Width, originalImage.Height];
                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    Color pixel = originalImage.GetPixel(x, y);
                                    intensityValues[x, y] = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                                }
                            }

                            // Başlangıç merkezlerini rastgele seç (0-255 arası)
                            Random rand = new Random();
                            int[] kmCenters = new int[kmIntensityK];
                            for (int i = 0; i < kmIntensityK; i++)
                            {
                                kmCenters[i] = rand.Next(0, 256);
                            }

                            // ListView2'yi hazırla (başlangıç değerleri için)
                            listView2.Clear();
                            listView2.View = View.Details;
                            listView2.Columns.Add("Küme No", 70);
                            listView2.Columns.Add("Gri Değer", 70);


                            // Başlangıç değerlerini ListView2'ye ekle
                            for (int i = 0; i < kmIntensityK; i++)
                            {
                                int pixelCount = 0;
                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    for (int y = 0; y < originalImage.Height; y++)
                                    {
                                        if (Math.Abs(intensityValues[x, y] - kmCenters[i]) <= 5) // 5 birim tolerans
                                        {
                                            pixelCount++;
                                        }
                                    }
                                }

                                ListViewItem item = new ListViewItem((i + 1).ToString());
                                item.SubItems.Add(kmCenters[i].ToString());
                                item.SubItems.Add(pixelCount.ToString());
                                listView2.Items.Add(item);
                            }

                                // Küme etiketlerini tutacak dizi
                                int[,] kmLabels = new int[originalImage.Width, originalImage.Height];// Bu dizi, her pikselin hangi kümeye ait olduğunu tutar.
                                bool changed;
                                             // Eğer bir iterasyonda en az bir pikselin küme etiketi değişirse, bu değişken true yapılır.
                                        
                            int maxIterations = 100;// Algoritmanın maksimum kaç iterasyon çalışacağını belirleyen değişken.
                                                    // Bu, algoritmanın sonsuz döngüye girmesini engeller.
                                                    // Örneğin, maxIterations = 100 ise, algoritma en fazla 100 iterasyon çalışır.
                            int iteration = 0;// Algoritmanın kaçıncı iterasyonda olduğunu takip etmek için bir sayaç.
                                              // Her iterasyonda bu değişken bir artırılır (iteration++).
                                              // iteration değişkeni maxIterations değerine ulaştığında, algoritma durur

                            do
                            {
                                changed = false;
                                // Her pikseli en yakın merkeze ata
                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    for (int y = 0; y < originalImage.Height; y++)
                                    {
                                        int intensity = intensityValues[x, y];//gri foto
                                        int minDist = int.MaxValue;//een küçük mesafeyi tutar
                                        int closestCenter = 0;//küme merkezinin numrarsını

                                        // Tüm küme merkezlerini dolaşmak için bir döngü başlatılır.
                                        for (int k = 0; k < kmIntensityK; k++)
                                        {
                                            // Pikselin gri tonlama değeri ile küme merkezi arasındaki mesafe hesaplanır. mutlak değer içinde
                                            int dist = Math.Abs(intensity - kmCenters[k]);

                                            // Eğer hesaplanan mesafe, şu ana kadar bulunan en küçük mesafeden daha küçükse:
                                            if (dist < minDist)
                                            {
                                                // En küçük mesafe güncellenir.
                                                minDist = dist;

                                                // En yakın küme merkezinin numarası güncellenir.
                                                closestCenter = k;
                                            }
                                        }

                                        // Etiket değişirse changed=true yap
                                        if (kmLabels[x, y] != closestCenter)
                                        {
                                            changed = true;
                                            kmLabels[x, y] = closestCenter;
                                        }
                                    }
                                }

                                // Merkezleri güncelle
                                int[] sums = new int[kmIntensityK];
                                int[] counts = new int[kmIntensityK];

                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    for (int y = 0; y < originalImage.Height; y++)
                                    {
                                        int label = kmLabels[x, y];
                                        sums[label] += intensityValues[x, y];
                                        counts[label]++;
                                    }
                                }

                                // Yeni merkez değerlerini hesapla
                                for (int k = 0; k < kmIntensityK; k++)
                                {
                                    if (counts[k] > 0)
                                    {
                                        kmCenters[k] = sums[k] / counts[k];
                                    }
                                }

                                iteration++;
                            } while (changed && iteration < maxIterations);

                            // Son küme merkezlerini sırala
                            Array.Sort(kmCenters);

                            // ListView1'i hazırla (son değerler için)
                            listView1.Clear();
                            listView1.View = View.Details;
                            listView1.Columns.Add("Küme No", 70);
                            listView1.Columns.Add("Gri Değer", 70);
                            listView1.Columns.Add("Piksel Sayısı", 100);

                            // Son küme merkezlerini ve piksel sayılarını hesapla
                            int[] finalCounts = new int[kmIntensityK];
                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    finalCounts[kmLabels[x, y]]++;
                                }
                            }

                            // Son değerleri ListView1'e ekle
                            for (int i = 0; i < kmIntensityK; i++)
                            {
                                ListViewItem item = new ListViewItem((i + 1).ToString());
                                item.SubItems.Add(kmCenters[i].ToString());
                                item.SubItems.Add(finalCounts[i].ToString());
                                listView1.Items.Add(item);
                            }

                            // Sonuç görüntüsünü oluştur
                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    int label = kmLabels[x, y];
                                    int intensity = kmCenters[label];
                                    resultImage.SetPixel(x, y, Color.FromArgb(intensity, intensity, intensity));
                                }
                            }

                            // Chart'ı hazırla
                            chart1.Series.Clear();

                            // Histogram serisi
                            Series histogramSeries = new Series("Histogram");
                            histogramSeries.ChartType = SeriesChartType.Area;
                            histogramSeries.Color = Color.Blue;
                            histogramSeries.BorderWidth = 1;

                            // Histogram verilerini hesapla
                            int[] histogram = new int[256];
                            for (int y = 0; y < originalImage.Height; y++)
                            {
                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    Color pixel = originalImage.GetPixel(x, y);
                                    int grayValue = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                                    histogram[grayValue]++;
                                }
                            }

                            // Histogram verilerini ekle
                            for (int i = 0; i < 256; i++)
                            {
                                histogramSeries.Points.AddXY(i, histogram[i]);
                            }

                            // Seriyi chart'a ekle
                            chart1.Series.Add(histogramSeries);

                            // Küme merkezleri için yeni seri oluştur
                            Series clusterCenterSeries = new Series("Küme Merkezleri");
                            clusterCenterSeries.ChartType = SeriesChartType.Point;
                            clusterCenterSeries.Color = Color.Red;
                            clusterCenterSeries.MarkerStyle = MarkerStyle.Circle;
                            clusterCenterSeries.MarkerSize = 10;

                            // Küme merkezlerini (T değerlerini) histograma ekle
                            for (int i = 0; i < kmIntensityK; i++)
                            {
                                int centerValue = kmCenters[i];
                                clusterCenterSeries.Points.AddXY(centerValue, histogram[centerValue]);
                            }

                            // Küme merkezleri serisini chart'a ekle
                            chart1.Series.Add(clusterCenterSeries);

                            // Chart ayarları
                            chart1.ChartAreas[0].AxisX.Title = "Gri Seviye";
                            chart1.ChartAreas[0].AxisY.Title = "Piksel Sayısı";
                            chart1.ChartAreas[0].AxisX.Minimum = 0;
                            chart1.ChartAreas[0].AxisX.Maximum = 255;
                            chart1.ChartAreas[0].AxisX.Interval = 32;
                            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N0";

                            if (pictureBox2.Image != null)
                                pictureBox2.Image.Dispose();
                            pictureBox2.Image = new Bitmap(resultImage);

                            watch.Stop();
                            label6.Text = $"İşlem Süresi: {watch.ElapsedMilliseconds} ms";
                            label7.Text = $"İterasyon: {iteration}";
                            label8.Text = $"Toplam Piksel: {totalPixels}";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"K-Means Intensity işlemi sırasında hata: {ex.Message}");
                        }
                        break;

                    case "KM öklit RGB":
                        try
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            int totalPixels = originalImage.Width * originalImage.Height;

                            // K değerini al
                            int rgbKCount = int.Parse(comboBox2.SelectedItem.ToString());

                            // Küme merkezlerini tutacak yapı (R,G,B değerleri)
                            List<Color> rgbCenters = new List<Color>();
                            Random rgbRand = new Random();

                            // ListView2'yi hazırla (başlangıç değerleri için)
                            listView2.Clear();
                            listView2.View = View.Details;
                            listView2.Columns.Add("Küme No", 70);
                            listView2.Columns.Add("R", 70);
                            listView2.Columns.Add("G", 70);
                            listView2.Columns.Add("B", 70);

                            // Başlangıç merkezlerini rastgele seç ve ListView2'ye ekle
                            for (int i = 0; i < rgbKCount; i++)
                            {
                                Color center = Color.FromArgb(
                                    rgbRand.Next(256),
                                    rgbRand.Next(256),
                                    rgbRand.Next(256)
                                );
                                rgbCenters.Add(center);

                                ListViewItem item = new ListViewItem((i + 1).ToString());
                                item.SubItems.Add(center.R.ToString());
                                item.SubItems.Add(center.G.ToString());
                                item.SubItems.Add(center.B.ToString());
                                item.BackColor = center;
                                listView2.Items.Add(item);
                            }

                            // Küme etiketlerini tutacak dizi
                            int[,] rgbLabels = new int[originalImage.Width, originalImage.Height];
                            bool rgbChanged;
                            int rgbMaxIterations = 100;
                            int rgbIteration = 0;

                            do
                            {
                                rgbChanged = false;

                                // Her pikseli en yakın merkeze ata (Öklid mesafesine göre)
                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    for (int y = 0; y < originalImage.Height; y++)
                                    {
                                        Color pixel = originalImage.GetPixel(x, y);
                                        double minDist = double.MaxValue;
                                        int closestCenter = 0;

                                        // En yakın merkezi bul (Öklid mesafesi)
                                        for (int k = 0; k < rgbKCount; k++)
                                        {
                                            Color center = rgbCenters[k];
                                            // Öklid mesafesi = sqrt((R1-R2)² + (G1-G2)² + (B1-B2)²)
                                            double dist = Math.Sqrt(
                                                Math.Pow(pixel.R - center.R, 2) +
                                                Math.Pow(pixel.G - center.G, 2) +
                                                Math.Pow(pixel.B - center.B, 2)
                                            );

                                            if (dist < minDist)
                                            {
                                                minDist = dist;
                                                closestCenter = k;
                                            }
                                        }

                                        // Etiket değişirse changed=true yap
                                        if (rgbLabels[x, y] != closestCenter)
                                        {
                                            rgbChanged = true;
                                            rgbLabels[x, y] = closestCenter;
                                        }
                                    }
                                }

                                // Yeni merkez renklerini hesapla (her kümenin ortalama R,G,B değerleri)
                                List<Color> newCenters = new List<Color>();
                                for (int k = 0; k < rgbKCount; k++)
                                {
                                    long sumR = 0, sumG = 0, sumB = 0;
                                    int count = 0;

                                    for (int x = 0; x < originalImage.Width; x++)
                                    {
                                        for (int y = 0; y < originalImage.Height; y++)
                                        {
                                            if (rgbLabels[x, y] == k)
                                            {
                                                Color pixel = originalImage.GetPixel(x, y);
                                                sumR += pixel.R;
                                                sumG += pixel.G;
                                                sumB += pixel.B;
                                                count++;
                                            }
                                        }
                                    }

                                    if (count > 0)
                                    {
                                        newCenters.Add(Color.FromArgb(
                                            (int)(sumR / count),
                                            (int)(sumG / count),
                                            (int)(sumB / count)
                                        ));
                                    }
                                    else
                                    {
                                        newCenters.Add(rgbCenters[k]); // Eğer küme boşsa eski merkezi koru
                                    }
                                }

                                rgbCenters = newCenters;
                                rgbIteration++;
                            } while (rgbChanged && rgbIteration < rgbMaxIterations);

                            // Sonuç görüntüsünü oluştur
                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    Color centerColor = rgbCenters[rgbLabels[x, y]];
                                    resultImage.SetPixel(x, y, centerColor);
                                }
                            }

                            // ListView1'i hazırla (son değerler için)
                            listView1.Clear();
                            listView1.View = View.Details;
                            listView1.Columns.Add("Küme No", 70);
                            listView1.Columns.Add("R", 70);
                            listView1.Columns.Add("G", 70);
                            listView1.Columns.Add("B", 70);
                            listView1.Columns.Add("Piksel Sayısı", 100);

                            // Her küme için istatistikleri hesapla ve ListView1'e ekle
                            for (int k = 0; k < rgbKCount; k++)
                            {
                                int pixelCount = 0;
                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    for (int y = 0; y < originalImage.Height; y++)
                                    {
                                        if (rgbLabels[x, y] == k)
                                        {
                                            pixelCount++;
                                        }
                                    }
                                }

                                // Küme bilgilerini ListView1'e ekle
                                ListViewItem item = new ListViewItem((k + 1).ToString());
                                item.SubItems.Add(rgbCenters[k].R.ToString());
                                item.SubItems.Add(rgbCenters[k].G.ToString());
                                item.SubItems.Add(rgbCenters[k].B.ToString());
                                item.SubItems.Add(pixelCount.ToString());
                                item.BackColor = rgbCenters[k];
                                listView1.Items.Add(item);
                            }

                            // Chart'ı hazırla
                            chart1.Series.Clear();

                            // Histogram serisi
                            Series histogramSeries = new Series("Histogram");
                            histogramSeries.ChartType = SeriesChartType.Area;
                            histogramSeries.Color = Color.Blue;
                            histogramSeries.BorderWidth = 1;

                            // Histogram verilerini hesapla
                            int[] histogram = new int[256];
                            for (int y = 0; y < originalImage.Height; y++)
                            {
                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    Color pixel = originalImage.GetPixel(x, y);
                                    int grayValue = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                                    histogram[grayValue]++;
                                }
                            }

                            // Histogram verilerini ekle
                            for (int i = 0; i < 256; i++)
                            {
                                histogramSeries.Points.AddXY(i, histogram[i]);
                            }

                            // Seriyi chart'a ekle
                            chart1.Series.Add(histogramSeries);

                            // Küme merkezleri için yeni seri oluştur
                            Series clusterCenterSeries = new Series("Küme Merkezleri");
                            clusterCenterSeries.ChartType = SeriesChartType.Point;
                            clusterCenterSeries.Color = Color.Red;
                            clusterCenterSeries.MarkerStyle = MarkerStyle.Circle;
                            clusterCenterSeries.MarkerSize = 10;

                            // Küme merkezlerini histograma ekle
                            for (int i = 0; i < rgbKCount; i++)
                            {
                                int centerValue = (rgbCenters[i].R + rgbCenters[i].G + rgbCenters[i].B) / 3;
                                clusterCenterSeries.Points.AddXY(centerValue, histogram[centerValue]);
                            }

                            // Küme merkezleri serisini chart'a ekle
                            chart1.Series.Add(clusterCenterSeries);

                            // Chart ayarları
                            chart1.ChartAreas[0].AxisX.Title = "Gri Seviye";
                            chart1.ChartAreas[0].AxisY.Title = "Piksel Sayısı";
                            chart1.ChartAreas[0].AxisX.Minimum = 0;
                            chart1.ChartAreas[0].AxisX.Maximum = 255;
                            chart1.ChartAreas[0].AxisX.Interval = 32;
                            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N0";

                            if (pictureBox2.Image != null)
                                pictureBox2.Image.Dispose();
                            pictureBox2.Image = new Bitmap(resultImage);

                            watch.Stop();
                            label6.Text = $"İşlem Süresi: {watch.ElapsedMilliseconds} ms";
                            label7.Text = $"İterasyon: {rgbIteration}";
                            label8.Text = $"Toplam Piksel: {totalPixels}";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"K-Means Öklid RGB işlemi sırasında hata: {ex.Message}");
                        }
                        break;

                    case "KM Mahalonobis":
                        try
                        {
                            // İşlem süresini ölçmek için bir kronometre başlatılır.
                            var watch = System.Diagnostics.Stopwatch.StartNew();

                            // Görüntünün toplam piksel sayısı hesaplanır.
                            int totalPixels = originalImage.Width * originalImage.Height;

                            // Kullanıcı tarafından seçilen küme sayısı (K değeri) alınır.
                            int mhKCount = int.Parse(comboBox2.SelectedItem.ToString());

                            // Küme merkezleri ve kovaryans matrisleri için diziler oluşturulur.
                            Color[] centroids = new Color[mhKCount]; // Küme merkezleri (renk değerleri)
                            Matrix3x3[] covarianceMatrices = new Matrix3x3[mhKCount]; // Her küme için kovaryans matrisleri
                            int[,] clusterAssignments = new int[originalImage.Width, originalImage.Height]; // Her pikselin ait olduğu küme
                            bool centroidsChanged; // Küme merkezlerinin değişip değişmediğini kontrol etmek için bayrak
                            Random mhRand = new Random(); // Rastgele sayı üretmek için Random nesnesi

                            // ListView2'yi hazırla (başlangıç değerleri için)
                            listView2.Clear();
                            listView2.View = View.Details;
                            listView2.Columns.Add("Küme", 70);
                            listView2.Columns.Add("Gri Değer", 70);

                            // 1. Rastgele küme merkezleri belirleme
                            for (int i = 0; i < mhKCount; i++)
                            {
                                // Görüntüden rastgele bir piksel seçilir.
                                Color randomPixel = originalImage.GetPixel(mhRand.Next(originalImage.Width), mhRand.Next(originalImage.Height));

                                // Seçilen pikselin gri değeri hesaplanır.
                                int grayValue = (int)(0.299 * randomPixel.R + 0.587 * randomPixel.G + 0.114 * randomPixel.B);

                                // Küme merkezi olarak gri renk atanır.
                                centroids[i] = Color.FromArgb(grayValue, grayValue, grayValue);

                                // Kovaryans matrisi başlangıçta birim matris olarak ayarlanır.
                                covarianceMatrices[i] = Matrix3x3.Identity();

                                // Başlangıç değerlerini ListView2'ye ekle
                                ListViewItem initialItem = new ListViewItem((i + 1).ToString());
                                initialItem.SubItems.Add(grayValue.ToString());
                                initialItem.BackColor = centroids[i];
                                listView2.Items.Add(initialItem);
                            }

                            int iteration = 0; // İterasyon sayacı
                            do
                            {
                                centroidsChanged = false; // Küme merkezlerinin değişip değişmediğini kontrol etmek için bayrak sıfırlanır.

                                // Her küme için toplam renk değerleri ve piksel sayıları sıfırlanır.
                                double[] sumR = new double[mhKCount];
                                double[] sumG = new double[mhKCount];
                                double[] sumB = new double[mhKCount];
                                int[] clusterSizes = new int[mhKCount];

                                // Yeni kovaryans matrislerini sıfırla
                                Matrix3x3[] newCovarianceMatrices = new Matrix3x3[mhKCount];
                                for (int i = 0; i < mhKCount; i++)
                                    newCovarianceMatrices[i] = new Matrix3x3();

                                // 2. Pikselleri kümelere ata
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    for (int x = 0; x < originalImage.Width; x++)
                                    {
                                        Color pixel = originalImage.GetPixel(x, y);
                                        int bestCluster = 0;
                                        double minDistance = MahalanobisDistance(pixel, centroids[0], covarianceMatrices[0]);

                                        // En yakın küme merkezini bul
                                        for (int i = 1; i < mhKCount; i++)
                                        {
                                            double distance = MahalanobisDistance(pixel, centroids[i], covarianceMatrices[i]);
                                            if (distance < minDistance)
                                            {
                                                bestCluster = i;
                                                minDistance = distance;
                                            }
                                        }

                                        // Pikseli en yakın kümeye ata
                                        clusterAssignments[x, y] = bestCluster;

                                        // Küme renk toplamlarına piksel renk değerlerini ekle
                                        sumR[bestCluster] += pixel.R;
                                        sumG[bestCluster] += pixel.G;
                                        sumB[bestCluster] += pixel.B;
                                        clusterSizes[bestCluster]++;

                                        // Kovaryans matrisine katkı ekle
                                        newCovarianceMatrices[bestCluster].AddContribution(pixel, centroids[bestCluster]);
                                    }
                                }

                                // 3. Küme merkezlerini güncelle
                                for (int i = 0; i < mhKCount; i++)
                                {
                                    if (clusterSizes[i] > 0)
                                    {
                                        // Yeni küme merkezini hesapla (renk ortalaması)
                                        Color newCentroid = Color.FromArgb(
                                            (int)(sumR[i] / clusterSizes[i]),
                                            (int)(sumG[i] / clusterSizes[i]),
                                            (int)(sumB[i] / clusterSizes[i])
                                        );

                                        // Küme merkezi değiştiyse bayrağı güncelle
                                        if (!centroids[i].Equals(newCentroid))
                                        {
                                            centroidsChanged = true;
                                            centroids[i] = newCentroid;
                                        }

                                        // Yeni kovaryans matrisini hesapla
                                        newCovarianceMatrices[i].DivideBy(clusterSizes[i]);
                                        covarianceMatrices[i] = newCovarianceMatrices[i];
                                    }
                                }

                                iteration++; // İterasyon sayacını artır
                            } while (centroidsChanged && iteration < 100); // Küme merkezleri değişmeyene veya maksimum iterasyona ulaşana kadar devam et


                            // ListView1'i hazırla (son değerler için)
                            listView1.Clear();
                            listView1.View = View.Details;
                            listView1.Columns.Add("Küme", 70);
                            listView1.Columns.Add("R", 70);
                            listView1.Columns.Add("G", 70);
                            listView1.Columns.Add("B", 70);
                            listView1.Columns.Add("Piksel Sayısı", 100);

                            // Son değerleri ListView1'e ekle
                            int[] clusterPixelCounts = new int[mhKCount];
                            for (int y = 0; y < originalImage.Height; y++)
                            {
                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    clusterPixelCounts[clusterAssignments[x, y]]++;
                                }
                            }

                            for (int i = 0; i < mhKCount; i++)
                            {
                                int grayValue = (centroids[i].R + centroids[i].G + centroids[i].B) / 3;
                                Color grayColor = Color.FromArgb(grayValue, grayValue, grayValue);
                                centroids[i] = grayColor;

                                ListViewItem finalItem = new ListViewItem((i + 1).ToString());
                                finalItem.SubItems.Add(grayValue.ToString());
                                finalItem.SubItems.Add(grayValue.ToString());
                                finalItem.SubItems.Add(grayValue.ToString());
                                finalItem.SubItems.Add(clusterPixelCounts[i].ToString());
                                finalItem.BackColor = grayColor;
                                listView1.Items.Add(finalItem);
                            }

                            // Histogram için veri yapısı
                            int[] histogram = new int[256];

                            // 4. Grileştirme işlemi ve histogram hesaplama
                            for (int y = 0; y < originalImage.Height; y++)
                            {
                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    int cluster = clusterAssignments[x, y];
                                    int grayValue = (centroids[cluster].R + centroids[cluster].G + centroids[cluster].B) / 3;
                                    resultImage.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                                    histogram[grayValue]++;
                                }
                            }

                            // Chart'ı hazırla
                            chart1.Series.Clear();

                            // Histogram serisi
                            Series histogramSeries = new Series("Histogram");
                            histogramSeries.ChartType = SeriesChartType.Area;
                            histogramSeries.Color = Color.Blue;
                            histogramSeries.BorderWidth = 1;

                            // Histogram verilerini ekle
                            for (int i = 0; i < 256; i++)
                            {
                                histogramSeries.Points.AddXY(i, histogram[i]);
                            }

                            // Seriyi chart'a ekle
                            chart1.Series.Add(histogramSeries);

                            // Küme merkezleri için yeni seri oluştur
                            Series clusterCenterSeries = new Series("Küme Merkezleri");
                            clusterCenterSeries.ChartType = SeriesChartType.Point;
                            clusterCenterSeries.Color = Color.Red;
                            clusterCenterSeries.MarkerStyle = MarkerStyle.Circle;
                            clusterCenterSeries.MarkerSize = 10;

                            // Küme merkezlerini histograma ekle
                            for (int i = 0; i < mhKCount; i++)
                            {
                                int centerValue = (centroids[i].R + centroids[i].G + centroids[i].B) / 3;
                                clusterCenterSeries.Points.AddXY(centerValue, histogram[centerValue]);
                            }

                            // Küme merkezleri serisini chart'a ekle
                            chart1.Series.Add(clusterCenterSeries);

                            // Chart ayarları
                            chart1.ChartAreas[0].AxisX.Title = "Gri Seviye";
                            chart1.ChartAreas[0].AxisY.Title = "Piksel Sayısı";
                            chart1.ChartAreas[0].AxisX.Minimum = 0;
                            chart1.ChartAreas[0].AxisX.Maximum = 255;
                            chart1.ChartAreas[0].AxisX.Interval = 32;
                            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N0";

                            if (pictureBox2.Image != null)
                                pictureBox2.Image.Dispose();
                            pictureBox2.Image = new Bitmap(resultImage);

                            watch.Stop();
                            label6.Text = $"İşlem Süresi: {watch.ElapsedMilliseconds} ms";
                            label7.Text = $"İterasyon: {iteration}";
                            label8.Text = $"Toplam Piksel: {totalPixels}";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"K-Means Mahalanobis işlemi sırasında hata: {ex.Message}");
                        }
                        break;

                    case "KM Mahalonobis ND":
                        try
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            int totalPixels = originalImage.Width * originalImage.Height;

                            // K değerini al
                            int ndKCount = int.Parse(comboBox2.SelectedItem.ToString());

                            // Veri noktalarını tut (R,G,B değerleri)
                            List<(double R, double G, double B)> ndPoints = new List<(double R, double G, double B)>();

                            // Önce tüm renk değerlerini topla (normalizasyon için)
                            double sumR = 0, sumG = 0, sumB = 0;
                            double sumR2 = 0, sumG2 = 0, sumB2 = 0;

                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    Color pixel = originalImage.GetPixel(x, y);
                                    sumR += pixel.R;
                                    sumG += pixel.G;
                                    sumB += pixel.B;
                                    sumR2 += pixel.R * pixel.R;
                                    sumG2 += pixel.G * pixel.G;
                                    sumB2 += pixel.B * pixel.B;
                                }
                            }

                            // Ortalama ve standart sapmaları hesapla
                            double meanR = sumR / totalPixels;
                            double meanG = sumG / totalPixels;
                            double meanB = sumB / totalPixels;

                            double stdR = Math.Sqrt((sumR2 / totalPixels) - (meanR * meanR));
                            double stdG = Math.Sqrt((sumG2 / totalPixels) - (meanG * meanG));
                            double stdB = Math.Sqrt((sumB2 / totalPixels) - (meanB * meanB));

                            // Normalize edilmiş renk değerlerini hesapla
                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    Color pixel = originalImage.GetPixel(x, y);
                                    ndPoints.Add((
                                        (pixel.R - meanR) / stdR,
                                        (pixel.G - meanG) / stdG,
                                        (pixel.B - meanB) / stdB
                                    ));
                                }
                            }

                            // Başlangıç merkezlerini seç
                            List<(double R, double G, double B)> ndCenters = new List<(double R, double G, double B)>();
                            Random ndRand = new Random();

                            // İlk merkezi rastgele seç
                            for (int i = 0; i < ndKCount; i++)
                            {
                                int randIdx = ndRand.Next(ndPoints.Count);
                                var point = ndPoints[randIdx];
                                ndCenters.Add(point);
                            }

                            // Küme etiketlerini tutacak dizi
                            int[,] ndLabels = new int[originalImage.Width, originalImage.Height];
                            bool ndChanged;
                            int ndMaxIterations = 100;
                            int ndIteration = 0;

                            do
                            {
                                ndChanged = false;
                                int pixelIndex = 0;

                                // Her pikseli en yakın merkeze ata
                                for (int x = 0; x < originalImage.Width; x++)
                                {
                                    for (int y = 0; y < originalImage.Height; y++)
                                    {
                                        var point = ndPoints[pixelIndex++];
                                        double minDist = double.MaxValue;
                                        int closestCenter = 0;

                                        // En yakın merkezi bul (Normalize edilmiş Mahalanobis mesafesi)
                                        for (int k = 0; k < ndKCount; k++)
                                        {
                                            var center = ndCenters[k];

                                            // Normalize edilmiş uzaklık hesapla
                                            double dist = Math.Sqrt(
                                                Math.Pow(point.R - center.R, 2) +
                                                Math.Pow(point.G - center.G, 2) +
                                                Math.Pow(point.B - center.B, 2)
                                            );

                                            if (dist < minDist)
                                            {
                                                minDist = dist;
                                                closestCenter = k;
                                            }
                                        }

                                if (ndLabels[x, y] != closestCenter)
                                {
                                    ndChanged = true;
                                    ndLabels[x, y] = closestCenter;
                                }
                            }
                        }

                        // Yeni merkez renklerini hesapla
                        List<(double R, double G, double B)> newCenters = new List<(double R, double G, double B)>();
                        for (int k = 0; k < ndKCount; k++)
                        {
                            double sumNdR = 0, sumNdG = 0, sumNdB = 0;
                            int count = 0;
                            pixelIndex = 0;

                            for (int x = 0; x < originalImage.Width; x++)
                            {
                                for (int y = 0; y < originalImage.Height; y++)
                                {
                                    if (ndLabels[x, y] == k)
                                    {
                                        var point = ndPoints[pixelIndex];
                                        sumNdR += point.R;
                                        sumNdG += point.G;
                                        sumNdB += point.B;
                                        count++;
                                    }
                                    pixelIndex++;
                                }
                            }

                            if (count > 0)
                            {
                                newCenters.Add((
                                    sumNdR / count,
                                    sumNdG / count,
                                    sumNdB / count
                                ));
                            }
                            else
                            {
                                // Boş küme durumunda rastgele bir nokta seç
                                int randIdx = ndRand.Next(ndPoints.Count);
                                newCenters.Add(ndPoints[randIdx]);
                            }
                        }

                        ndCenters = newCenters;
                        ndIteration++;
                    } while (ndChanged && ndIteration < ndMaxIterations);

                    // Sonuç görüntüsünü oluştur
                    for (int x = 0; x < originalImage.Width; x++)
                    {
                        for (int y = 0; y < originalImage.Height; y++)
                        {
                            var center = ndCenters[ndLabels[x, y]];
                            // Normalize edilmiş değerleri geri dönüştür
                            int r = (int)Math.Max(0, Math.Min(255, (center.R * stdR) + meanR));
                            int g = (int)Math.Max(0, Math.Min(255, (center.G * stdG) + meanG));
                            int b = (int)Math.Max(0, Math.Min(255, (center.B * stdB) + meanB));
                            resultImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }

                    // ListView'i hazırla
                    listView1.Clear();
                    listView1.View = View.Details;
                    listView1.Columns.Add("Küme No", 60);
                    listView1.Columns.Add("Norm. R", 70);
                    listView1.Columns.Add("Norm. G", 70);
                    listView1.Columns.Add("Norm. B", 70);
                    listView1.Columns.Add("Gerçek R", 70);
                    listView1.Columns.Add("Gerçek G", 70);
                    listView1.Columns.Add("Gerçek B", 70);
                    listView1.Columns.Add("Piksel Sayısı", 80);

                    // ListView2'yi hazırla - İstatistikler için
                    listView2.Clear();
                    listView2.View = View.Details;
                    listView2.Columns.Add("Küme No", 70);
                    listView2.Columns.Add("R", 70);
                    listView2.Columns.Add("G", 70);
                    listView2.Columns.Add("B", 70);

                    // Başlangıç değerlerini ListView2'ye ekle
                    for (int i = 0; i < ndKCount; i++)
                    {
                        int randIdx = ndRand.Next(ndPoints.Count);
                        var point = ndPoints[randIdx];
                        ndCenters.Add(point);

                        // Normalize edilmiş değerleri geri dönüştür
                        int realR = (int)Math.Max(0, Math.Min(255, (point.R * stdR) + meanR));
                        int realG = (int)Math.Max(0, Math.Min(255, (point.G * stdG) + meanG));
                        int realB = (int)Math.Max(0, Math.Min(255, (point.B * stdB) + meanB));

                        ListViewItem item = new ListViewItem((i + 1).ToString());
                        item.SubItems.Add(realR.ToString());
                        item.SubItems.Add(realG.ToString());
                        item.SubItems.Add(realB.ToString());
                        item.BackColor = Color.FromArgb(realR, realG, realB);
                        listView2.Items.Add(item);
                    }

                    // ListView1'i hazırla
                    listView1.Clear();
                    listView1.View = View.Details;
                    listView1.Columns.Add("Küme No", 70);
                    listView1.Columns.Add("R", 70);
                    listView1.Columns.Add("G", 70);
                    listView1.Columns.Add("B", 70);
                    listView1.Columns.Add("Piksel Sayısı", 80);

                    // Son değerleri ListView1'e ekle
                    for (int k = 0; k < ndKCount; k++)
                    {
                        int pixelCount = 0;
                        var center = ndCenters[k];

                        // Normalize edilmiş değerleri geri dönüştür
                        int realR = (int)Math.Max(0, Math.Min(255, (center.R * stdR) + meanR));
                        int realG = (int)Math.Max(0, Math.Min(255, (center.G * stdG) + meanG));
                        int realB = (int)Math.Max(0, Math.Min(255, (center.B * stdB) + meanB));

                        for (int x = 0; x < originalImage.Width; x++)
                        {
                            for (int y = 0; y < originalImage.Height; y++)
                            {
                                if (ndLabels[x, y] == k)
                                {
                                    pixelCount++;
                                }
                            }
                        }

                        ListViewItem item = new ListViewItem((k + 1).ToString());
                        item.SubItems.Add(realR.ToString());
                        item.SubItems.Add(realG.ToString());
                        item.SubItems.Add(realB.ToString());
                        item.SubItems.Add(pixelCount.ToString());
                        item.BackColor = Color.FromArgb(realR, realG, realB);
                        listView1.Items.Add(item);
                    }

                    // Chart'ı hazırla
                    chart1.Series.Clear();

                    // Histogram serisi
                    Series histogramSeriesND = new Series("Histogram");
                    histogramSeriesND.ChartType = SeriesChartType.Area;
                    histogramSeriesND.Color = Color.FromArgb(128, 0, 0, 255); // Yarı saydam mavi
                    histogramSeriesND.BorderWidth = 1;

                    // Histogram verilerini hesapla
                    int[] histogramND = new int[256];
                    for (int y = 0; y < resultImage.Height; y++)
                    {
                        for (int x = 0; x < resultImage.Width; x++)
                        {
                            Color pixel = resultImage.GetPixel(x, y);
                            int grayValue = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                            histogramND[grayValue]++;
                        }
                    }

                    // Histogram verilerini ekle
                    for (int i = 0; i < 256; i++)
                    {
                        histogramSeriesND.Points.AddXY(i, histogramND[i]);
                    }

                    // Seriyi chart'a ekle
                    chart1.Series.Add(histogramSeriesND);

                    // Küme merkezleri için yeni seri oluştur
                    Series clusterCenterSeriesND = new Series("Küme Merkezleri");
                    clusterCenterSeriesND.ChartType = SeriesChartType.Point;
                    clusterCenterSeriesND.Color = Color.Red;
                    clusterCenterSeriesND.MarkerStyle = MarkerStyle.Circle;
                    clusterCenterSeriesND.MarkerSize = 10;

                    // ListView2'yi temizle ve hazırla
                    listView2.Clear();
                    listView2.View = View.Details;
                    listView2.Columns.Add("Küme No", 70);
                    listView2.Columns.Add("Gri Değer", 70);
                    listView2.Columns.Add("Piksel Sayısı", 100);

                    // Küme merkezlerini (T değerlerini) histograma ekle ve ListView2'ye ekle
                    for (int i = 0; i < ndKCount; i++)
                    {
                        // Normalize edilmiş değerleri geri dönüştür
                        int r = (int)Math.Max(0, Math.Min(255, (ndCenters[i].R * stdR) + meanR));
                        int g = (int)Math.Max(0, Math.Min(255, (ndCenters[i].G * stdG) + meanG));
                        int b = (int)Math.Max(0, Math.Min(255, (ndCenters[i].B * stdB) + meanB));
                        int grayValue = (int)(0.299 * r + 0.587 * g + 0.114 * b);

                        clusterCenterSeriesND.Points.AddXY(grayValue, histogramND[grayValue]);

                        // ListView2'ye ekle
                        ListViewItem item = new ListViewItem((i + 1).ToString());
                        item.SubItems.Add(grayValue.ToString());
                        item.SubItems.Add(histogramND[grayValue].ToString());
                        listView2.Items.Add(item);
                    }

                    // Küme merkezleri serisini chart'a ekle
                    chart1.Series.Add(clusterCenterSeriesND);

                    // Chart ayarları
                    chart1.ChartAreas[0].AxisX.Title = "Gri Seviye";
                    chart1.ChartAreas[0].AxisY.Title = "Piksel Sayısı";
                    chart1.ChartAreas[0].AxisX.Minimum = 0;
                    chart1.ChartAreas[0].AxisX.Maximum = 255;
                    chart1.ChartAreas[0].AxisX.Interval = 32;
                    chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N0";

                    if (pictureBox2.Image != null)
                        pictureBox2.Image.Dispose();
                    pictureBox2.Image = new Bitmap(resultImage);

                    watch.Stop();
                    label6.Text = $"İşlem Süresi: {watch.ElapsedMilliseconds} ms";
                    label7.Text = $"İterasyon: {ndIteration}";
                    label8.Text = $"Toplam Piksel: {totalPixels}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"K-Means Mahalonobis ND işlemi sırasında hata: {ex.Message}");
                }
                        break;

                case "kenar bulma":
                    int width = originalImage.Width;
                    int height = originalImage.Height;
                    resultImage = new Bitmap(width, height);

                    // Grayscale dönüşümü
                    int[,] grayMatrix = new int[height, width];
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            Color pixel = originalImage.GetPixel(x, y);
                            grayMatrix[y, x] = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B) /3 ;
                        }
                    }

                    // Sobel filtreleri
                    int[,] horizontalFilter = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
                    int[,] verticalFilter = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };

                    // Yatay ve dikey filtreleme sonuçları için matrisler
                    int[,] horizontalResult = new int[height, width];
                    int[,] verticalResult = new int[height, width];

                    // Filtreleme işlemi
                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {
                            int sumH = 0;
                            int sumV = 0;

                            // 3x3 pencere için filtreleme
                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    sumH += grayMatrix[y + i, x + j] * horizontalFilter[i + 1, j + 1];
                                    sumV += grayMatrix[y + i, x + j] * verticalFilter[i + 1, j + 1];
                                }
                            }

                            horizontalResult[y, x] = sumH;
                            verticalResult[y, x] = sumV;
                        }
                    }

                    // Maksimum değerleri alma
                    int[,] maxHorizontal = new int[height, width];
                    int[,] maxVertical = new int[height, width];

                    // Yatay filtreleme sonrası dikey maksimumları al
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            int maxVal = horizontalResult[y, x];
                            for (int i = Math.Max(0, y - 1); i <= Math.Min(height - 1, y + 1); i++)
                            {
                                maxVal = Math.Max(maxVal, horizontalResult[i, x]);
                            }
                            maxHorizontal[y, x] = maxVal;
                        }
                    }

                    // Dikey filtreleme sonrası yatay maksimumları al
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int maxVal = verticalResult[y, x];
                            for (int i = Math.Max(0, x - 1); i <= Math.Min(width - 1, x + 1); i++)
                            {
                                maxVal = Math.Max(maxVal, verticalResult[y, i]);
                            }
                            maxVertical[y, x] = maxVal;
                        }
                    }

                    // Matrisleri birleştir ve threshold uygula
                    Random random = new Random();
                    int threshold = random.Next(2, 100); // Trash değerini 45-90 arası rastgele seç

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int sum = Math.Abs(maxHorizontal[y, x]) + Math.Abs(maxVertical[y, x]);

                            // Threshold kontrolü
                            if (sum < threshold)
                                sum = 0;
                            else if (sum > 255)
                                sum = 255;
                            // threshold ile 255 arası değerler aynen kalıyor

                            resultImage.SetPixel(x, y, Color.FromArgb(sum, sum, sum));
                        }
                    }


                    if (pictureBox2.Image != null)
                        pictureBox2.Image.Dispose();
                    pictureBox2.Image = new Bitmap(resultImage);
                    break;
                default:
                    MessageBox.Show("Geçersiz işlem seçildi.");
                    break;
            }

            // Sonuç görüntüsünü göster - switch bloğunun dışında
            if (resultImage != null)
            {
                pictureBox2.Image?.Dispose(); // Eski görüntüyü temizle
                pictureBox2.Image = new Bitmap(resultImage);
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                MessageBox.Show("İşlem sonucunda görüntü oluşturulamadı.");
            }

            // Kullanılmayan kaynakları temizle
            if (originalImage != null)
            {
                originalImage.Dispose();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İşlem sırasında bir hata oluştu: {ex.Message}");
            }
        }

    /// <summary>
    /// 3x3 matrisin tersini hesaplayan fonksiyon
    /// Kovaryans matrisinin tersini almak için kullanılır
    /// </summary>
    private double[,] InvertMatrix(double[,] matrix)
    {
        // 3x3 matris için tersini hesapla
        int n = 3;
        double[,] result = new double[n, n];

        // Determinantı hesapla
        double det = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1])
                    - matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0])
                    + matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);

        // Determinantın tersini al
        double invDet = 1.0 / det;

        // Kofaktör matrisini hesapla ve determinantın tersi ile çarp
        result[0, 0] = (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) * invDet;
        result[0, 1] = (matrix[0, 2] * matrix[2, 1] - matrix[0, 1] * matrix[2, 2]) * invDet;
        result[0, 2] = (matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1]) * invDet;
        result[1, 0] = (matrix[1, 2] * matrix[2, 0] - matrix[1, 0] * matrix[2, 2]) * invDet;
        result[1, 1] = (matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0]) * invDet;
        result[1, 2] = (matrix[0, 2] * matrix[1, 0] - matrix[0, 0] * matrix[1, 2]) * invDet;
        result[2, 0] = (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]) * invDet;
        result[2, 1] = (matrix[0, 1] * matrix[2, 0] - matrix[0, 0] * matrix[2, 1]) * invDet;
        result[2, 2] = (matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]) * invDet;

        return result;
    }

    /// <summary>
    /// İki nokta arasındaki Mahalanobis mesafesini hesaplayan fonksiyon
    /// Kovaryans matrisini kullanarak RGB uzayında mesafe hesaplar
    /// </summary>
    private double CalculateMahalanobisDistance((double R, double G, double B) point,
        (double R, double G, double B) center, double[,] invCovariance)
    {
        // RGB değerleri arasındaki farkları hesapla
        double diffR = point.R - center.R;
        double diffG = point.G - center.G;
        double diffB = point.B - center.B;

        // Fark vektörünü oluştur
        double[] diff = new double[] { diffR, diffG, diffB };
        double distance = 0;

        // Mahalanobis mesafesi formülü: sqrt((x-μ)^T * Σ^-1 * (x-μ))
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                distance += diff[i] * invCovariance[i, j] * diff[j];
            }
        }

        return Math.Sqrt(distance);
    }

    /// <summary>
    /// 3x3 matris işlemleri için yardımcı sınıf
    /// Kovaryans matrisi hesaplamaları için kullanılır
    /// </summary>
    private class Matrix3x3
    {
        // Matris değerlerini tutan 3x3 dizi
        private double[,] values;

        /// <summary>
        /// Boş bir 3x3 matris oluşturur
        /// </summary>
        public Matrix3x3()
        {
            values = new double[3, 3];
        }

        /// <summary>
        /// Birim matris oluşturur (köşegen değerleri 1, diğerleri 0)
        /// </summary>
        public static Matrix3x3 Identity()
        {
            Matrix3x3 matrix = new Matrix3x3();
            for (int i = 0; i < 3; i++) matrix.values[i, i] = 1.0;
            return matrix;
        }

        /// <summary>
        /// Kovaryans matrisine piksel katkısını ekler
        /// Her piksel için RGB farklarının çarpımlarını hesaplar
        /// </summary>
        public void AddContribution(Color pixel, Color centroid)
        {
            double[] diff = { pixel.R - centroid.R, pixel.G - centroid.G, pixel.B - centroid.B };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    values[i, j] += diff[i] * diff[j];
                }
            }
        }

        /// <summary>
        /// Matris değerlerini verilen sayıya böler
        /// Ortalama hesaplamak için kullanılır
        /// </summary>
        public void DivideBy(int scalar)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    values[i, j] /= scalar;
                }
            }
        }

        /// <summary>
        /// Vektör ile matris çarpımını hesaplar
        /// Mahalanobis mesafesi hesabında kullanılır
        /// </summary>
        public double Transform(double[] vector)
        {
            double result = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result += vector[i] * values[i, j] * vector[j];
                }
            }
            return result;
        }
    }

    /// <summary>
    /// Piksel ve merkez arasındaki Mahalanobis mesafesini hesaplar
    /// RGB renk uzayında kümeleme için kullanılır
    /// </summary>
    private double MahalanobisDistance(Color pixel, Color centroid, Matrix3x3 covarianceMatrix)
    {
        double[] diff = { pixel.R - centroid.R, pixel.G - centroid.G, pixel.B - centroid.B };
        double result = covarianceMatrix.Transform(diff);
        return Math.Sqrt(result);
    }
}


    }

/*

*/