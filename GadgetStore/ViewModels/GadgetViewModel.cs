using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GadgetStore.Models;
using Microsoft.AspNetCore.Http;

namespace GadgetStore.ViewModels
{
    public class GadgetViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int GadgetTypeId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public string Memory { get; set; }

        [Required]
        public string RAM { get; set; }

        [Required]
        public string Diagonal { get; set; }

        [Required]
        public string ScreenResolution { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public string CPU { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        public IFormFile Image { get; set; }
    }
}
