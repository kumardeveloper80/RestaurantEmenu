using EMenuApplication.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;

namespace EMenuApplication.Controllers
{
    public class DashboardController : BaseController
    {
        IDashboardRepository _dashboardRepository;
        ICategoryRepository _categoryRepository;
        IItemTagRepository _itemTagRepository;
        IMenuRepository _menuRepository;
        IMenuItemRepository _menuItemRepository;


        public DashboardController(IDashboardRepository dashboardRepository, IMenuItemRepository menuItemRepository, IMenuRepository menuRepository, IItemTagRepository itemTagRepository, ICategoryRepository categoryRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _dashboardRepository = dashboardRepository;
            _categoryRepository = categoryRepository;
            _itemTagRepository = itemTagRepository;
            _menuRepository = menuRepository;
            _menuItemRepository = menuItemRepository;
        }
        public IActionResult Index()
        {
            var result = _dashboardRepository.GetCounts(this.loginUserId, this.isAdmin);

            result.TotalCategories = _categoryRepository.GetList(this.loginUserId, this.isAdmin).Count;
            result.TotalMenuItems = _menuItemRepository.GetList(this.loginUserId, this.isAdmin).Count;
            result.TotalMenu = _menuRepository.GetList(this.loginUserId, this.isAdmin).Count;

            return View(result);
        }
    }
}