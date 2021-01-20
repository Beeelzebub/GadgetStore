using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadgetStore.Models;

namespace GadgetStore.ViewModels
{
    public class FilterViewModel
    {
        public string Memory { get; set; }
        public string RAM { get; set; }
        public int GadgetType { get; set; }
        public int Manufacturer { get; set; }
        public int Diagonal { get; set; }
        public int ScreenResolution { get; set; }
        public int Color { get; set; }
        public int CPU { get; set; }

    }
}
