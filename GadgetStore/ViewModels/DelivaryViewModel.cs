using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GadgetStore.ViewModels
{
    public class DelivaryViewModel
    {
        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Город")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string City { get; set; }

        [Display(Name = "Улица")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Street { get; set; }

        [Display(Name = "Дом")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public int Hous { get; set; }

        [Display(Name = "Подъезд (необязательно)")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public int? Porch { get; set; }

        [Display(Name = "Квартира (необязательно)")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public int? Apartment { get; set; }

        [Required(ErrorMessage = "ВЫберете способ оплаты")]
        public int PayType { get; set; }
    }
}
