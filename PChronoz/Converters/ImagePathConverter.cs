using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PChronoz.Models;

namespace PChronoz.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        string[] keys_path = File.ReadAllLines(@"C:\ProjectChronoz\key.txt");
        public string ImagesPath { get; set; }

        public ImagePathConverter()
        {
            ImagesPath = keys_path[1];
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine("Convert called with value: " + value);

            if (value is Card card)
            {
                string ruta = $@"{ImagesPath}\Cards\{card.Id}.jpg";
                Debug.WriteLine("Ruta final: " + ruta);

                if (File.Exists(ruta))
                {
                    return new BitmapImage(new Uri(ruta, UriKind.Absolute));
                }
                else
                {
                    Debug.WriteLine("File not found: " + ruta);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
