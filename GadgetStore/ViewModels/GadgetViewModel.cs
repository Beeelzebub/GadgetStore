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
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Name { get; set; }

        [Display(Name = "Тип")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public int GadgetTypeId { get; set; }

        [Display(Name = "Год выпуска")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public int Year { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public float Price { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public int Count { get; set; }

        [Display(Name = "Флэш память")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Memory { get; set; }

        [Display(Name = "Оперативная память")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string RAM { get; set; }

        [Display(Name = "Диагональ")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Diagonal { get; set; }

        [Display(Name = "Разрешение")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string ScreenResolution { get; set; }

        [Display(Name = "Цвет")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Color { get; set; }

        [Display(Name = "Процессор")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string CPU { get; set; }

        [Display(Name = "Производитель")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Manufacturer { get; set; }

        public IFormFile Image { get; set; }
    }
}
