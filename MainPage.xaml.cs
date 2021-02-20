﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using m1_image_projet.Source;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace m1_image_projet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Inpainting inpainting;

        public MainPage()
        {
            inpainting = new Inpainting();
            this.InitializeComponent();
        }

        private void Image_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        private async void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems)) {
                IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0) {
                    StorageFile storageFile = items[0] as StorageFile;
                    IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.Read);
                    inpainting.WriteableBitmap.SetSource(fileStream);

                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);

                    // Scale image to appropriate size
                    BitmapTransform transform = new BitmapTransform()
                    {
                        ScaledWidth = Convert.ToUInt32(inpainting.WriteableBitmap.PixelWidth),
                        ScaledHeight = Convert.ToUInt32(inpainting.WriteableBitmap.PixelHeight)
                    };

                    PixelDataProvider pixelData = await decoder.GetPixelDataAsync(
                        BitmapPixelFormat.Bgra8,    // WriteableBitmap uses BGRA format
                        BitmapAlphaMode.Straight,
                        transform,
                        ExifOrientationMode.IgnoreExifOrientation, // This sample ignores Exif orientation
                        ColorManagementMode.DoNotColorManage);

                    // An array containing the decoded image data, which could be modified before being displayed
                    inpainting.pixels = pixelData.DetachPixelData();

                    Image.Source = inpainting.WriteableBitmap;
                }
            }
        }
    }
}
