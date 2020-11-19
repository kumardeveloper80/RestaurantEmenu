using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Utility
{
    public class DataTableAjaxPostModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
        public int storeId { get; set; }
        public Guid storeGuid { get; set; }
        public string status { get; set; }
        public int menuId { get; set; }
        public int categoryId { get; set; }
        public int conceptId { get; set; }
        public int currencyId { get; set; }
        public int menuItemId { get; set; }
        public List<int> menuIds { get; set; }
        public List<int> nameIds { get; set; }
        public List<int> labelENIds { get; set; }
        public List<int> labelARIds { get; set; }

        public DataTableAjaxPostModel()
        {
            menuIds = new List<int>();
            nameIds = new List<int>();
            labelENIds = new List<int>();
            labelARIds = new List<int>();
        }
    }

    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}
