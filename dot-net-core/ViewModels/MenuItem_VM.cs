using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class MenuItem_VM
    {
        public int Id { get; set; }
        public string PLU { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
        public decimal? Price { get; set; }
        public bool Status { get; set; } = true;
        public string DetailsEN { get; set; }
        public string DetailsAR { get; set; }
        public string ThumbnailImageName { get; set; }
        public string LargeImageName { get; set; }
        public string LargeDetailsEN { get; set; }
        public string LargeDetailsAR { get; set; }
        public string OverLayImageName { get; set; }
        public string OverlayDetailsEN { get; set; }
        public string OverlayDetailsAR { get; set; }
        public IFormFile ThumbnailImage { get; set; }
        public IFormFile LargeImage { get; set; }
        public IFormFile OverLayImage { get; set; }
        public int CreatedBy { get; set; }
        public List<int> ItemTagsId { get; set; }
        public string CategoryName { get; set; }
        public List<ItemTag_VM> ItemTags { get; set; }
        public string CommentsEN { get; set; }
        public string CommentsAR { get; set; }
        public string LabelEN { get; set; }
        public string LabelAR { get; set; }
        public string sPrice { get; set; }
        public List<int> ConceptIds { get; set; }
        public string ConceptName { get; set; }
        public int MenuItemSequence { get; set; }
        public int ConceptId { get; set; }
        public List<MenuItemConcepts_VM> MenuItemConcepts { get; set; }
        public int StoreId { get; set; }
        public MenuItem_VM()
        {
            ItemTagsId = new List<int>();
            ItemTags = new List<ItemTag_VM>();
            ConceptIds = new List<int>();
            MenuItemConcepts = new List<MenuItemConcepts_VM>();
        }
    }
    public class MenuItemConcepts_VM
    {
        public int ConceptId { get; set; }
        public string ConceptName { get; set; }
    }
}
