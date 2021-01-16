using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadgetStore.Models;

namespace GadgetStore.ViewModels
{
    public class FilterViewModel
    {
        public int[] Year { get; set; }
        public string[] Memory { get; set; }
        public string[] RAM { get; set; }
        public List<GadgetType> GadgetTypes { get; set; }
        public List<Diagonal> Diagonals { get; set; }
        public List<ScreenResolution> ScreenResolutions { get; set; }
        public List<Color> Colors { get; set; }
        public List<CPU> CPUs { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
        
        public FilterViewModel()
        {
            GadgetTypes = new List<GadgetType>();
            Diagonals = new List<Diagonal>();
            ScreenResolutions = new List<ScreenResolution>();
            Colors = new List<Color>();
            CPUs = new List<CPU>();
            Manufacturers = new List<Manufacturer>();
        }
    }
}
