using System;
using System.ComponentModel.DataAnnotations;

namespace SaucierWeb.Models
{
    public class AddItem
    {

        [Display(Name = "Item")]
        public string Item { get; set; }

        [Display(Name = "ItemId")]
        public Guid ItemId { get; set; }

        [Display(Name = "Quantidade")]
        public decimal Quantidade { get; set; }

        [Display(Name = "Comanda")]
        public Guid ComandaId { get; set; }

        public AddItem() { }
    }
}