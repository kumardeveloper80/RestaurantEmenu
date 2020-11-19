using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.ViewComponents
{
    public class ConceptListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var SelectedConceptId = AppContext.Current.Session.GetInt32("SelectedConceptId") != null ? AppContext.Current.Session.GetInt32("SelectedConceptId").Value : 0;
            var conceptsList = AppContext.Current.Session.GetComplexData<List<Concepts_VM>>("conceptsList");
            var items = new List<SelectListItem>();
            foreach (var concept in conceptsList)
            {
                var selectedItem = false;
                if (concept.Id == SelectedConceptId)
                {
                    selectedItem = true;
                }

                items.Add(new SelectListItem { Selected = selectedItem, Text = concept.ConceptName, Value = concept.Id.ToString() });
            }
            ViewBag.ConceptList = items;
            return View();
        }
    }
}
