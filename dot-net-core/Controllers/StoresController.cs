using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMenuApplication.Controllers
{
    [AuthLog]
    [AllowAnonymous]
    public class StoresController : BaseController
    {
        private readonly IClientRepository _clientRepository;
        private readonly IConceptsRepository _conceptsRepository;
        private readonly IStoresRespository _storesRespository;
        private readonly ICountryRepository _countryRepository;
        private readonly IRegionRepository _regionRepository;
        public List<Concepts_VM> conceptsLists { get; set; }
        public StoresController(IStoresRespository storesRespository, IConceptsRepository conceptsRepository, IClientRepository clientRepository,
            ICountryRepository countryRepository, IRegionRepository regionRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _storesRespository = storesRespository;
            _conceptsRepository = conceptsRepository;
            _countryRepository = countryRepository;
            _regionRepository = regionRepository;
            _clientRepository = clientRepository;
            conceptsLists = new List<Concepts_VM>();
        }

        public IActionResult Index(int Id = 0)
        {
            var store = new Store_VM();
            ViewBag.Clients = new SelectList(_clientRepository.GetList(loginUserId).Where(x => x.Status).ToList(), "Id", "CompanyName");
            ViewBag.ConceptsList = new SelectList(_conceptsRepository.GetList(loginUserId).Where(x => x.Active && x.ClientId == 0), "Id", "ConceptName");
            ViewBag.CountryList = new SelectList(_countryRepository.GetList().Where(x => x.Status), "CountryID", "Name");
            ViewBag.RegionList = new SelectList(_regionRepository.GetList().Where(x => x.Status), "RegionId", "Region");

            if (Id > 0)
            {
                store = _storesRespository.Get(Id, loginUserId);
                if (store == null)
                {
                    return RedirectToAction("List", "Stores");
                }
                else
                {
                    ViewBag.ConceptsList = new SelectList(_conceptsRepository.GetList(loginUserId).Where(x => x.Active && x.ClientId == store.ClientId), "Id", "ConceptName");
                    ViewData["Title"] = "Edit";
                    return View(store);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(store);
            }
        }

        public IActionResult List()
        {
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName", conceptsList.Where(x => x.Id == SelectedConceptId).Select(x => x.Id).FirstOrDefault());
            return View();
        }

        [HttpPost]
        public IActionResult GetList(DataTableAjaxPostModel param)
        {
            int recordsTotal = 0;
            var result = _storesRespository.GetList(this.loginUserId);

            if (result.Any())
            {
                recordsTotal = result.Count;

                if (param.status == Helper.Active)
                {
                    result = result.Where(x => x.Status).ToList();
                }
                else if (param.status == Helper.Inactive)
                {
                    result = result.Where(x => !x.Status).ToList();
                }

                result.ForEach(x => x.ConceptName = string.Join(", ", x.StoreConcepts.Select(x => x.ConceptName).ToList()));

                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.StoreName.ToLower().Contains(search.ToLower())
                        || x.StoreCode.ToLower().Contains(search.ToLower())
                        || x.ClientName.ToLower().Contains(search.ToLower())
                        || x.ConceptName.ToLower().Contains(search.ToLower())
                        || x.RegionName.ToLower().Contains(search.ToLower())
                        || x.CountryName.ToLower().Contains(search.ToLower())).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.StoreCode).ToList() : result.OrderBy(p => p.StoreCode).ToList();
                    break;
                case "1":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.StoreName).ToList() : result.OrderBy(p => p.StoreName).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ClientName).ToList() : result.OrderBy(p => p.ClientName).ToList();
                    break;
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ConceptName).ToList() : result.OrderBy(p => p.ConceptName).ToList();
                    break;
                case "4":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.RegionName).ToList() : result.OrderBy(p => p.RegionName).ToList();
                    break;
                case "5":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.CountryName).ToList() : result.OrderBy(p => p.CountryName).ToList();
                    break;
                default:
                    result = result.OrderByDescending(p => p.Id).ToList();
                    break;
            }

            var data = result;
            if (param.length > 0)
            {
                data = result.Skip(param.start).Take(param.length).ToList();
            }

            return Json(new
            {
                draw = param.draw,
                recordsFiltered = result.Count,
                recordsTotal = recordsTotal,
                data = data
            });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            var result = _storesRespository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.storeDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.storeDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Store_VM store_VM)
        {
            if (store_VM != null)
            {
                if (store_VM.Id > 0)
                {
                    if (_storesRespository.Update(store_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.storeUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.storeUpdateError;
                    }
                }
                else
                {
                    if (_storesRespository.Add(store_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.storeAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.storeAddedError;
                    }
                }
            }
            return RedirectToAction("List", "Stores");
        }

        [HttpGet]
        public IActionResult CheckUniqueStoreCode(int id, string storeCode, int clientId)
        {
            return Ok(_storesRespository.IsUniqueStoreCode(id, storeCode, clientId, loginUserId));
        }

        [HttpGet]
        public IActionResult CheckUniqueStoreName(int id, string storeName, int clientId)
        {
            return Ok(_storesRespository.IsUniqueStoreName(id, storeName, clientId, loginUserId));
        }

        [HttpGet]
        public IActionResult GetConceptList(int clientId)
        {
            if (clientId > 0)
            {
                conceptsLists = _conceptsRepository.GetList(loginUserId).Where(x => x.Active && x.ClientId == clientId).ToList();
            }
            else
            {
                conceptsLists = new List<Concepts_VM>();
            }
            return Ok(new SelectList(conceptsLists, "Id", "ConceptName"));
        }
    }
}
