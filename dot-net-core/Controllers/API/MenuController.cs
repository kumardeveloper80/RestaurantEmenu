using System;
using System.Linq;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EMenuApplication.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class MenuController : ControllerBase
    {
        IMenuScheduleRepository _menuScheduleRepository;
        private readonly ILogger _logger;
        public MenuController(IMenuScheduleRepository menuScheduleRepository, ILogger<LoginController> logger)
        {
            _menuScheduleRepository = menuScheduleRepository;
            _logger = logger;
        }

        /// <summary>
        /// Function for get the menu schedule by code
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{lang}/{code}")]
        public IActionResult Get(string lang, string code)
        {
            try
            {
                _logger.LogInformation("Get API::Start" + lang + '=' + code);
                code = Helper.Convert_HexvalueToStringvalue(code, System.Text.Encoding.Unicode);
                CommonResponse<MenuSchedule_VM> response = new CommonResponse<MenuSchedule_VM>();
                var result = _menuScheduleRepository.GetMenuByUniqueCode(lang, code);
                if (result != null)
                {
                    _logger.LogInformation("Reuslt::Found");
                    var currentDate = DateTime.Now;
                    _logger.LogInformation("currentDate::"+ currentDate);
                    _logger.LogInformation("StartDate::" + result.dStartDate);
                    _logger.LogInformation("EndDate::" + result.dEndDate);
                    if (currentDate >= result.dStartDate && currentDate <= result.dEndDate)
                    {
                        _logger.LogInformation("condition check1::");
                        response.status = Helper.success_code;
                        response.dataenum = result;
                    }
                    else if (currentDate < result.dStartDate)
                    {
                        _logger.LogInformation("condition check2::");
                        response.message = Message.menuScheduleCommingSoon;
                    }
                    else
                    {
                        response.status = Helper.commingsoon_code;
                        response.message = Message.menuScheduleDateAway;
                    }
                }
                else
                {
                    response.message = Message.menuScheduleNotFound;
                }
                _logger.LogInformation("Get API::End" + lang + '=' + code);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error::"+ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Function for get the menu item
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{lang}/{code}/{id}")]
        public IActionResult GetMenuItemDetails(string lang, string code, int id)
        {
            code = Helper.Convert_HexvalueToStringvalue(code, System.Text.Encoding.Unicode);
            CommonResponse<MenuItem_VM> response = new CommonResponse<MenuItem_VM>();
            var result = _menuScheduleRepository.GetMenuByUniqueCode(lang, code);
            if (result != null && result.Menu.MenuItems.Count > 0)
            {
                var menuItem = result.Menu.MenuItems.Where(x => x.Id == id).FirstOrDefault();
                if (menuItem != null)
                {
                    response.dataenum = menuItem;
                    response.status = Helper.success_code;
                }
                else
                {
                    response.message = Message.menuItemNotFound;
                }
            }
            else
            {
                response.message = Message.menuItemNotFound;
            }
            return Ok(response);
        }
       
    }
}
