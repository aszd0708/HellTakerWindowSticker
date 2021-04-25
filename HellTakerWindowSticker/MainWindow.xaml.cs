using HellTakerWindowSticker.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HellTakerWindowSticker
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap origianl;
        Bitmap[,] frames = new Bitmap[9,12];
        ImageSource[,] imgFrame = new ImageSource[9,12];
        string[] bitmapName = new string[9];
        string bitmapPath = "Resource/";
        string[] devilName = new string[9];
        int frame = -1;

        /* for release bitmap */
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        int value = 1;

        public MainWindow()
        {
            InitializeComponent();

            SetBitmapName();
            SetFrames();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.0167 * 3);
            timer.Tick += NextFrame;
            timer.Start();

            MouseDown += MainWindow_MouseDown;

            /* for notify icon */
            SetMenu();

            Topmost = true;
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void NextFrame(object sender, EventArgs e)
        {
            frame = (frame + 1) % 12;
            iCerberus.Source = imgFrame[value, frame];
        }

        private void SetBitmapName()
        {
            bitmapName[0] = "Azazel";
            bitmapName[1] = "Cerberus";
            bitmapName[2] = "Judgement";
            bitmapName[3] = "Justice";
            bitmapName[4] = "Lucifer";
            bitmapName[5] = "Malina";
            bitmapName[6] = "Modeus";
            bitmapName[7] = "Pandemonica";
            bitmapName[8] = "Zdrada";

            devilName[0] = "아자젤";
            devilName[1] = "케르베로스";
            devilName[2] = "저지먼트";
            devilName[3] = "저스티스";
            devilName[4] = "루시퍼";
            devilName[5] = "말리나";
            devilName[6] = "모데우스";
            devilName[7] = "판데모니카";
            devilName[8] = "즈드라다";
        }

        private void SetFrames()
        {
            for (int i = 0; i < 9; i++)
            {
                //origianl = System.Drawing.Image.FromFile(bitmapPath + bitmapName[i] + ".png") as Bitmap;
                ResourceManager rm = new ResourceManager("HellTakerWindowSticker.Properties.Resources", typeof(Resources).Assembly);
                //origianl = (Bitmap)(rm.GetObject(bitmapName[i] + ".png"));
                origianl = (Bitmap)rm.GetObject(bitmapName[i]);
                
                
                for (int j = 0; j < 12; j++)
                {
                    frames[i, j] = new Bitmap(100, 100);
                    using (Graphics g = Graphics.FromImage(frames[i, j]))
                    {
                        g.DrawImage(origianl, new System.Drawing.Rectangle(0, 0, 100, 100),
                            new System.Drawing.Rectangle(j * 100, 0, 100, 100),
                            GraphicsUnit.Pixel);
                    }
                    var handle = frames[i, j].GetHbitmap();
                    try
                    {
                        imgFrame[i, j] = Imaging.CreateBitmapSourceFromHBitmap(handle,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                    }
                    finally
                    {
                        DeleteObject(handle);
                    }
                }
                
            }
        }

        private void SetMenu()
        {
            var menu = new System.Windows.Forms.ContextMenu();
            var noti = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.FromHandle(frames[0, 0].GetHicon()),
                Visible = true,
                Text = "히히 귀엽다능",
                ContextMenu = menu,
            };

            for (int i = 0; i < 9; i++)
            {
                var imgChange = new System.Windows.Forms.MenuItem
                {
                    Index = i,
                    Text = devilName[i] + "짜응",
                };

                imgChange.Click += (object o, EventArgs e) =>
                {
                    value = imgChange.Index;
                };
                menu.MenuItems.Add(imgChange);
            }

            var item = new System.Windows.Forms.MenuItem
            {
                Index = 10,
                Text = "잘가 악마쨔응",
            };

            item.Click += (object o, EventArgs e) =>
            {
                Application.Current.Shutdown();
            };

            menu.MenuItems.Add(item);
            noti.ContextMenu = menu;
        }
    }
}
