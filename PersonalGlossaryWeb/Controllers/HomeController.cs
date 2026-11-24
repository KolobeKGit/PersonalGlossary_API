using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalGlossary_API.Models.DTO;
using PersonalGlossaryWeb.Models;
using PersonalGlossaryWeb.Services.IServices;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PersonalGlossaryWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITermService _termService;
        private readonly IMapper _mapper;

        public HomeController(ITermService termService, IMapper mapper)
        {
            _termService = termService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<TermDTO> termList = new();
            try
            {
                var response = await _termService.GetAllAsync<ApiResponse<List<TermDTO>>>("");
                if (response != null && response.Success && response.Data != null)
                {
                    termList = response.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }
            return View(termList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
