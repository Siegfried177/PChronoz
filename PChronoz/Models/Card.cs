using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using PChronoz.ViewModels;
using System;

namespace PChronoz.Models
{
    class Card : ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Attribute { get; set; }
        public string Level { get; set; }
        public string Type1 { get; set; }
        public string Type2 { get; set; }
        public string Type3 { get; set; }
        public string Type4 { get; set; }
        public string Atk { get; set; }
        public string Def { get; set; }
        public string Quantity { get; set; }
        public string Rarity { get; set; }
        public string IconImage
        {
            get
            {
                string aux = "";
                if (Attribute != "Trap" && Attribute != "Spell")
                    if (Type3 == "-")
                    {
                        if (Type2 == "Normal") aux = "normal";
                        else if (Type2 == "Effect") aux = "effect";
                    }
                    else
                    {
                        if (Type3 == "Fusion") aux = "fusion";
                        else if (Type3 == "Ritual") aux = "ritual";
                        else if (Type3 == "Synchro") aux = "synchro";
                        else if (Type3 == "Xyz") aux = "xyz";
                        else if (Type3 == "Pendulum") aux = "pendulum";
                        else if (Type3 == "Link") aux = "link";
                        else if (Type3.Contains("Pendulum")) aux = "pendulum";
                    }
                else
                {
                    if (Attribute == "Spell") aux = "spell";
                    else aux = "trap";
                }
                string ImagePath = File.ReadAllLines(@"C:\ProjectChronoz\key.txt")[1];
                return $@"{ImagePath}\Icons\{aux}.png";
            }
        }

        public object Clone()
        {
            return new Card
            {
                Id = this.Id,
                Name = this.Name,
                Attribute = this.Attribute,
                Level = this.Level,
                Type1 = this.Type1,
                Type2 = this.Type2,
                Type3 = this.Type3,
                Type4 = this.Type4,
                Atk = this.Atk,
                Def = this.Def,
                Quantity = this.Quantity,
                Rarity = this.Rarity
            };
        }
    }
}
