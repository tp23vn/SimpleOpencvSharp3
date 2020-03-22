using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace ImageViewer
{
    public partial class PhotoViewer : Form
    {
        private double _scaleRatio;
        private string _inputImagePath;
        private bool _bGrayMode;
        Image _originalImage;
        Bitmap _grayImage;

        public PhotoViewer()
        {
            _scaleRatio = 1.0;
            _bGrayMode = false;
            InitializeComponent();
        }

        private void PhotoViewer_Load(object sender, EventArgs e)
        {
            this.ptbViewer.MouseWheel += this.ptbViewer_MouseWheel;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openImageFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ptbViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_scaleRatio >= 0.125 && _scaleRatio <= 4)
            {
                if (e.Delta > 0)
                {
                    zoomIn();
                }
                else
                {
                    zoomOut();
                }
            }
        }

        private void doZoom() {
            ptbViewer.Width = (int)(ptbViewer.Image.Width * _scaleRatio);
            ptbViewer.Height = (int)(ptbViewer.Image.Height * _scaleRatio);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Developed by Tai Phung\nEmail: taiphungdinh@gmail.com\nGithub: www.github.com/tp23vn";
            MessageBox.Show(message, "About", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openImageFile();
        }

        private void openImageFile() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png, *.jpeg, *.jpg, *.bmp)|*.png;*.jpg;*.jpeg;*.bmp|PNG (*.png)|*.png|JPEG (*.jpeg)|*.jpeg|JPG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _originalImage = Image.FromFile(openFileDialog.FileName);
                ptbViewer.Image = _originalImage;
                ptbViewer.Width = ptbViewer.Image.Width;
                ptbViewer.Height = ptbViewer.Image.Height;
                _inputImagePath = openFileDialog.FileName;
                _grayImage = null;
                btnZoomIn.Enabled = true;
                btnZoomOut.Enabled = true;
                btnGrayConversion.Enabled = true;
            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            zoomIn();
        }

        private void zoomIn()
        {
            if (_scaleRatio < 4)
            {
                _scaleRatio *= 2;
                doZoom();
            }
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            zoomOut();
        }

        private void zoomOut()
        {
            if (_scaleRatio > 0.125)
            {
                _scaleRatio /= 2;
                doZoom();
            }
        }

        private void btnGrayConversion_Click(object sender, EventArgs e)
        {
            if (_bGrayMode)
            {
                ptbViewer.Image = _originalImage;
                _bGrayMode = false;
                btnGrayConversion.ToolTipText = "Convert To Gray";
            }
            else
            {
                if (_grayImage == null)
                {
                    Mat grayImageMat = new Mat(_inputImagePath, ImreadModes.Grayscale);
                    _grayImage = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(grayImageMat);
                }
                ptbViewer.Image = _grayImage;
                _bGrayMode = true;
                btnGrayConversion.ToolTipText = "Show Original Image";
            }
        }
    }
}
