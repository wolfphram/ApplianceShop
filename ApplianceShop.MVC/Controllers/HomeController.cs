using System.Diagnostics;
using ApplianceShop.Data;
using ApplianceShop.Models;
using ApplianceShop.Models.Appliances;
using ApplianceShop.Models.Customers;
using ApplianceShop.Models.Homes;
using ApplianceShop.Models.Parts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplianceShop.Controllers
{
    public class HomeController : Controller
    {
        #region Data & Constructor
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Actions

        public async Task<IActionResult> Index()
        {
            var homePageVM = new HomePageViewModel();

            homePageVM.GetAppliances = await GetAppliances2();
            homePageVM.GetParts = await GetParts2();
            homePageVM.OurCustomers = await GetOurCustomers();


            //var appliances = _context.Appliances.ToList();
            //var parts = _context.Parts.ToList();

            //var viewModel = new HomePageViewModel
            //{
            //    Appliances = appliances,
            //    Parts = parts
            //};

            return View(homePageVM);

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
        #endregion

        #region Private Methods

        private async Task<List<ApplianceViewModel>> GetAppliances2()
        {
            var getAppliances = await _context
                                        .Appliances
                                        .OrderByDescending(appliances => appliances.Price)
                                        .ToListAsync();

            var getAppliancesVMs = _mapper.Map<List<ApplianceViewModel>>(getAppliances); 

            return getAppliancesVMs;
        }

        private async Task<List<PartViewModel>> GetParts2()
        {
            var getParts = await _context
                                     .Parts
                                     .OrderByDescending (parts => parts.Price)
                                     .ToListAsync();

            var getPartsVMs = _mapper.Map<List<PartViewModel>>(getParts);

            return getPartsVMs;
        }

        private async Task<List<CustomerViewModel>> GetOurCustomers()
        {
            var customers = await _context
                                    .Customers
                                    .Select(customer => new CustomerViewModel
                                    {
                                        Id = customer.Id,
                                        FirstName = customer.FirstName,
                                        LastName = customer.LastName,
                                        Address = customer.Address,
                                        Email = customer.Email,
                                        Phone = customer.Phone,
                                        Gender = customer.Gender,
                                        DateOfBirth = customer.DateOfBirth,
                                        Orders = customer.Orders
                                    })
                                    .ToListAsync();

            customers = customers.OrderBy(c => c.FullName != "VIP Customer").ToList();

            return customers;
        }


        #endregion
    }
}
