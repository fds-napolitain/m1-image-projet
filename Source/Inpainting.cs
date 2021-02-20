﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using System.IO;

namespace m1_image_projet.Source
{
    public sealed class Inpainting
    {
        private const short BLUE = 0;
        private const short GREEN = 1;
        private const short RED = 2;
        private const short PIXEL_STRIDE = 4;
        private WriteableBitmap writeableBitmap;
        public byte[] pixels;

        public Inpainting()
        {
            writeableBitmap = new WriteableBitmap(100, 100);
        }

        public WriteableBitmap WriteableBitmap { get => writeableBitmap; }

        public byte this[int i, int j = 0, int color = 0] {
            get => pixels[i * PIXEL_STRIDE + color + (j * writeableBitmap.PixelWidth)];
            set => pixels[i * PIXEL_STRIDE + color + (j * writeableBitmap.PixelWidth)] = value;
        }


        public async void Reload()
        {
            // Open a stream to copy the image contents to the WriteableBitmap's pixel buffer
            using (Stream stream = writeableBitmap.PixelBuffer.AsStream()) {
                await stream.WriteAsync(pixels, 0, pixels.Length);
            }
            // Redraw the WriteableBitmap
            writeableBitmap.Invalidate();
        }

        public byte[] Neighbors(int i, int j, int color = 0)
        {
            return new byte[] {
                this[i-PIXEL_STRIDE+color, j-PIXEL_STRIDE],
                this[i+color, j-PIXEL_STRIDE],
                this[i+PIXEL_STRIDE+color, j-PIXEL_STRIDE],
                this[i-PIXEL_STRIDE+color, j],
                this[i+PIXEL_STRIDE+color, j],
                this[i-PIXEL_STRIDE+color, j+PIXEL_STRIDE],
                this[i+color, j+PIXEL_STRIDE],
                this[i+PIXEL_STRIDE+color, j+PIXEL_STRIDE],
            };
        }

        public void Erode()
        {
            byte[] copy = pixels;
            for (int j = 0; j < writeableBitmap.PixelHeight; j++) {
                for (int i = 0; i < writeableBitmap.PixelWidth; i++) {
                    byte[] blueNeighbors = Neighbors(i, j, BLUE);
                    byte[] greenNeighbors = Neighbors(i, j, GREEN);
                    byte[] redNeighbors = Neighbors(i, j, RED)
                    if (blueNeighbors.Max() < this[i, j, BLUE]) {
                        this[i, j, BLUE] = 
                    }
                }
            }
        }
    }

}
