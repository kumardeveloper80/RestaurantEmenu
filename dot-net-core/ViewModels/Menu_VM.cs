using System.Collections.Generic;

namespace EMenuApplication.ViewModels
{
    public class Menu_VM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<int> MenuItemIds { get; set; }
        public int CreatedBy { get; set; }
        public bool Status { get; set; } = true;
        public List<MenuItem_VM> MenuItems { get; set; }
        public int ConceptId { get; set; }
        public string ConceptName { get; set; }
        public int CategoryId { get; set; }
        public Menu_VM()
        {
            MenuItemIds = new List<int>();
            MenuItems = new List<MenuItem_VM>();
        }
    }

    public class CategorySequence_VM
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategorySequence { get; set; }
        public int MenuId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ItemSequence_VM
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public int ItemSequence { get; set; }
        public int MenuId { get; set; }
        public int CreatedBy { get; set; }
    }
}
