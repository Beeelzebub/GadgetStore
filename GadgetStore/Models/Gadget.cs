using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GadgetStore.Models
{
    public class Gadget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int PictureId { get; set; }
        public Picture Picture { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public string Memory { get; set; }
        public string RAM { get; set; }
        public int GadgetTypeId { get; set; }
        public GadgetType GadgetType { get; set; }
        public int DiagonalId { get; set; }
        public Diagonal Diagonal { get; set; }
        public int ScreenResolutionId { get; set; }
        public ScreenResolution ScreenResolution { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int CPUId { get; set; }
        public CPU CPU { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

    }
}
