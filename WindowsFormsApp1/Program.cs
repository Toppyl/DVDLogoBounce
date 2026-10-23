using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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



        [STAThread]
        static void Main()
        {
            Random rng = new Random();
            float dx = 5f;
            float dy = 5f;
            float x = 0;
            float y = 0;
            bool randomizeSpeed = false;
            bool randomizePos = false;
            Form mainForm = new Form();
            mainForm.Icon = new Icon(".\\DVD_VIDEO_logo.ico");
            mainForm.SizeGripStyle = SizeGripStyle.Hide;
            mainForm.Size = new Size(400, 400);
            mainForm.MaximizeBox = false;
            mainForm.MinimizeBox = false;
            Button button = new Button();
            button.Text = "Spawn";
            button.Location = new Point(0, 50);
            
            //Spawning function is at line 190 :3


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
            
            NumericUpDown xPos = new NumericUpDown();
            xPos.Value = 0;
            xPos.Maximum = Screen.FromControl(mainForm).Bounds.Width;
            xPos.Width = 50;
            xPos.Location = new Point(50, 225);

            NumericUpDown yPos = new NumericUpDown();
            yPos.Value = 0;
            yPos.Maximum = Screen.FromControl(mainForm).Bounds.Height;
            yPos.Width = 50;
            yPos.Location = new Point(150, 225);

            Label xPosLabel = new Label();
            xPosLabel.Text = "x: ";
            xPosLabel.Width = 50;
            xPosLabel.Location = new Point(0, 225);

            Label yPosLabel = new Label();
            yPosLabel.Text = "y: ";
            yPosLabel.Width = 50;
            yPosLabel.Location = new Point(100, 225);

            CheckBox randomPos = new CheckBox();
            randomPos.Checked = false;
            randomPos.Location = new Point(300, 225);

            randomPos.CheckedChanged += (o, e) =>
            {
                randomizePos = randomPos.Checked;
                xPos.Enabled = !randomPos.Checked;
                yPos.Enabled = !randomPos.Checked;
            };

            Label randomPosLabel = new Label();
            randomPosLabel.Text = "random pos";
            randomPosLabel.Width = 50;
            randomPosLabel.Location = new Point(250, 225);

            NumericUpDown xSpeed = new NumericUpDown();
            xSpeed.Value = 5;
            xSpeed.Width = 50;
            xSpeed.Location = new Point(50, 250);
            xSpeed.Minimum = -xSpeed.Maximum;

            Label dxLabel = new Label();
            dxLabel.Text = "dx: ";
            dxLabel.Location = new Point(0, 250);
            dxLabel.Size = new Size(50,50);

            Label dyLabel = new Label();
            dyLabel.Text = "dy: ";
            dyLabel.Location = new Point(100, 250);
            dyLabel.Size = new Size(50, 50);


            NumericUpDown ySpeed = new NumericUpDown();
            ySpeed.Value = 5;
            ySpeed.Width = 50;
            ySpeed.Location = new Point(150, 250);
            ySpeed.Minimum = -ySpeed.Maximum;

            Label randomizeSpeedLabel = new Label();
            randomizeSpeedLabel.Text = "random speed: ";
            randomizeSpeedLabel.Width = 100;
            randomizeSpeedLabel.Location = new Point(200,250);

            CheckBox randomSpeed = new CheckBox();
            randomSpeed.Checked = false;
            randomSpeed.CheckedChanged += (o, e) =>
            {
                ySpeed.Enabled = !randomSpeed.Checked;
                xSpeed.Enabled = !randomSpeed.Checked;
                randomizeSpeed = randomSpeed.Checked;
            };
            randomSpeed.Location = new Point(300,250);

            Label Title = new Label();
            Title.Text = "Welcome to the DVD screen manager";
            Title.Size = new Size(600,30);
            Title.Font = new Font(FontFamily.GenericSerif, 15);

            Label info = new Label();
            info.Text = "close this window to close the dvd logos";
            info.Location = new Point(0, 300);
            info.Width = 300;



            button.Click += (sender, e) =>
            {
                if (randomizeSpeed)
                {
                    dx = (float)rng.NextDouble() * 20 - 10;
                    dy = (float)rng.NextDouble() * 20 - 10;
                } else
                {

                    dx = (float)xSpeed.Value;
                    dy = (float)ySpeed.Value;
                }

                if (randomizePos)
                {
                    x = (float)rng.NextDouble() * Screen.FromControl(mainForm).Bounds.Width;
                    y = (float)rng.NextDouble() * Screen.FromControl(mainForm).Bounds.Height;
                } else
                {
                    x = (float)xPos.Value;
                    y = (float)yPos.Value;
                }

                    Spawn(mainForm, x, y, dx, dy);
            };


            mainForm.Controls.Add(Title);
            mainForm.Controls.Add(button);
            mainForm.Controls.Add(setPath);
            mainForm.Controls.Add(resetPath);
            mainForm.Controls.Add(fileName);
            mainForm.Controls.Add(fileSize);
            mainForm.Controls.Add(setColor);
            mainForm.Controls.Add(resetColor);
            mainForm.Controls.Add(xPosLabel);
            mainForm.Controls.Add(xPos);
            mainForm.Controls.Add(yPosLabel);
            mainForm.Controls.Add(yPos);
            mainForm.Controls.Add(randomPos);
            mainForm.Controls.Add(randomPosLabel);
            mainForm.Controls.Add(xSpeed);
            mainForm.Controls.Add(dxLabel);
            mainForm.Controls.Add(ySpeed);
            mainForm.Controls.Add(dyLabel);
            mainForm.Controls.Add(randomizeSpeedLabel);
            mainForm.Controls.Add(randomSpeed);
            mainForm.Controls.Add(info);
            mainForm.ShowDialog();
        }

        static void Spawn(Form root, float x = 0, float y = 0, float dx = 5f, float dy = 5f)
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
                MainWindow.ShowInTaskbar = false;
                if (x >= Screen.FromControl(root).Bounds.Width - MainWindow.Width)
                {
                    x = Screen.FromControl(root).Bounds.Width - MainWindow.Width - 1;
                }
                if (y >= Screen.FromControl(root).Bounds.Height - MainWindow.Height)
                {
                    y = Screen.FromControl(root).Bounds.Height - MainWindow.Height - 1;
                }


                tmr.Interval = 1;
                tmr.Tick += onmove;
                tmr.Start();
                void onmove(object sender, EventArgs e)
                {
                    if (MainWindow.Location.X + dx <= 0 || MainWindow.Location.X + dx >= Screen.FromControl(MainWindow).Bounds.Width - MainWindow.Width)
                    {
                        dx = -dx;
                    }
                    if (MainWindow.Location.Y + dy <= 0 || MainWindow.Location.Y + dy >= Screen.FromControl(MainWindow).Bounds.Height - MainWindow.Height)
                    {
                        dy = -dy;
                    }
                    x += dx;
                    y += dy;
                    MainWindow.Location = new Point((int)Math.Round(x), (int)Math.Round(y));
                }

                Button removeButton = new Button();
                removeButton.Text = "Remove";
                removeButton.Click += (o, e) =>
                {
                    MainWindow.Close();
                    removeButton.Dispose();
                };
                removeButton.Location = new Point(100, 50);
                root.Controls.Add(removeButton);
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
