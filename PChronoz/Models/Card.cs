using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PChronoz.Models
{
    class Card
    {
        public string Id { get; set; }
        public string Arquetype { get; set; }
        public string Name { get; set; }
        public string Atribute { get; set; }
        public string Level { get; set; }
        public string Type1 { get; set; }
        public string Type2 { get; set; }
        public string Type3 { get; set; }
        public string Type4 { get; set; }
        public string Atk { get; set; }
        public string Def { get; set; }
        public string IconImage
        {
            get
            {
                string aux = "";
                if (Type3 == "-")
                {
                    if (Type2 == "Normal") aux = "normal";
                    else if (Type2 == "Efecto") aux = "effect";
                }
                else
                {
                    if (Type3 == "Fusion") aux = "fusion";
                    else if (Type3 == "Ritual") aux = "ritual";
                    else if (Type3 == "Sincronía") aux = "synchro";
                    else if (Type3 == "Xyz") aux = "xyz";
                    else if (Type3 == "Péndulo") aux = "pendulum";
                    else if (Type3 == "Link") aux = "link";
                    else if (Type3 == "Magia") aux = "spell";
                    else if (Type3 == "Trampa") aux = "trap";
                }
                return $"C:\\Users\\yamiy\\OneDrive\\Escritorio\\Proyectos\\4 -- C#\\PChronoz\\PChronoz\\Images\\Icons\\{aux}.png";
            }
        }
    }
}
