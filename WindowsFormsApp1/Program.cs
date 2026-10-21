using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        private static string path = ".\\DVD_VIDEO_logo.png";
        private static string defaultPath = ".\\DVD_VIDEO_logo.png";
        private static Color key = Color.FromArgb(255, 1, 1, 1);
        private static Color defaultKey = Color.FromArgb(255, 1, 1, 1);
        private static Form[] children = new Form[0];
        [STAThread]
        static void Main()
        {
            

            Form mainForm = new Form();

            Button button = new Button();
            button.Text = "Spawn";
            button.Location = new Point(0, 50);
            button.Click += (sender, e) =>
            {
                Spawn(mainForm);
            };

            Label fileName = new Label();
            fileName.Text = path;
            fileName.Location = new Point(200, 100);
            fileName.Size = new Size(200, 25);

            Label fileSize = new Label();
            fileSize.Text = $"{File.ReadAllBytes(path).Length.ToString()} bytes";
            fileSize.Location = new Point(200, 125);
            fileSize.Size = new Size(200, 50);

            Button setPath = new Button();
            setPath.Text = "set dvd logo";
            setPath.Click += (sender, e) =>
            {
                path = getNewPath();
                fileName.Text = path.Split('\\').Last();
                fileSize.Text = $"{File.ReadAllBytes(path).Length.ToString()} bytes";
            };
            setPath.Location = new Point(0, 100);

            Button resetPath = new Button();
            resetPath.Text = "reset dvd logo";
            resetPath.Click += (sender, e) =>
            {
                path = defaultPath;
                fileName.Text = path.Split('\\').Last();
                fileSize.Text = $"{File.ReadAllBytes(path).Length.ToString()} bytes";
            };

            

            resetPath.Location = new Point(100, 100);

            Button setColor = new Button();
            setColor.Text = "Set Key Color";
            setColor.Click += (sender, e) =>
            {
                key = GetColor();
            };


            setColor.Location = new Point(0, 150);


            Button resetColor = new Button();
            resetColor.Text = "reset key color";
            resetColor.Click += (sender, e) =>
            {
                key = defaultKey;
            };
            resetColor.Location = new Point(100, 150);

            Label Title = new Label();
            Title.Text = "Welcome to the DVD screen manager";
            Title.Size = new Size(600,30);
            Title.Font = new Font(FontFamily.GenericSerif, 15);

            Label info = new Label();
            info.Text = "close this window to close the dvd logos";
            info.Location = new Point(0, 200);
            info.Width = 300;


            mainForm.Controls.Add(Title);
            mainForm.Controls.Add(button);
            mainForm.Controls.Add(setPath);
            mainForm.Controls.Add(resetPath);
            mainForm.Controls.Add(fileName);
            mainForm.Controls.Add(fileSize);
            mainForm.Controls.Add(setColor);
            mainForm.Controls.Add(resetColor);
            mainForm.Controls.Add(info);
            mainForm.ShowDialog();
        }
        static void Spawn(Form root)
        {
            Random rng = new Random();
            System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
            Form MainWindow = new Form();
            try
            {
                Image bg = Image.FromFile(path);
                MainWindow.FormBorderStyle = FormBorderStyle.None;
                MainWindow.BackgroundImage = bg;
                MainWindow.BackColor = key;
                MainWindow.TransparencyKey = MainWindow.BackColor;
                MainWindow.Height = bg.Height;
                MainWindow.Width = bg.Width;

                float dx = 5f;
                float dy = 5f;
                float x = 5;
                float y = 5;
                tmr.Interval = 1;
                tmr.Tick += onmove;
                tmr.Start();
                void onmove(object sender, EventArgs e)
                {
                    if (MainWindow.Location.X <= 0 || MainWindow.Location.X >= Screen.PrimaryScreen.WorkingArea.Width - MainWindow.Width)
                    {
                        dx = -dx;
                    }
                    if (MainWindow.Location.Y <= 0 || MainWindow.Location.Y >= Screen.PrimaryScreen.WorkingArea.Height - MainWindow.Height)
                    {
                        dy = -dy;
                    }
                    x += dx;
                    y += dy;
                    MainWindow.Location = new Point((int)Math.Round(x), (int)Math.Round(y));
                }
                MainWindow.Show(root);
            }
            catch (Exception e)
            {
                {
                    if (e.GetType() == typeof(FileNotFoundException))
                    {
                        DialogResult a = MessageBox.Show($"kunde inte hitta filen, vill du ha en yogurt istället\n\n{e.StackTrace}", e.GetType().Name, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (a == DialogResult.Yes)
                        {
                            Image bg = Image.FromFile(".\\yogurt.png");
                            MainWindow.FormBorderStyle = FormBorderStyle.None;
                            MainWindow.BackgroundImage = bg;
                            MainWindow.BackColor = Color.Black;
                            MainWindow.TransparencyKey = MainWindow.BackColor;
                            MainWindow.Height = bg.Height;
                            MainWindow.Width = bg.Width;

                            MainWindow.Show(root);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"{e.Message}\n\n{e.StackTrace}", e.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                        

                }
            }

            


        }
        static string getNewPath()
        {
            FileDialog fdg = new OpenFileDialog();

            fdg.Filter = "normal image files (.jpg, .jpeg, .png, .webp, .gif) |*.jpg;*.jpeg;*.png;*.webp;*.gif|weird ass image formats that no one uses (.avif, .tif, .tiff)|*.avif;*.tif;*.tiff|I dont think this image format will work honestly (.bmp, .svg, .ai, .eps, .raw, .cr2, .nef)|*.bmp;*.svg;*.ai;*.eps,*.raw;*.cr2;*.nef|all files for some reason IT'S NOT GONNA WORK PAL|*";

            if (fdg.ShowDialog() == DialogResult.OK) {
                return fdg.FileName;
            } else{
                return path;
            }
        }
        static Color GetColor()
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                return cd.Color;
            }
            else
            {
                return key;
            }
        }
    }
}
