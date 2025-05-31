using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplianceShop.Data;
using ApplianceShop.Entities.Orders;
using AutoMapper;
using ApplianceShop.Models.Orders;
using ApplianceShop.Entities.Parts;
using System.IO;
using ApplianceShop.Entities.Appliances;

namespace ApplianceShop.Controllers
{
    public class OrdersController : Controller
    {
        #region Data and Constructor
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrdersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Actions
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context
                                    .Orders
                                    .Include(order => order.Customer)
                                    .ToListAsync();

            var orderVMs = _mapper.Map<List<OrderViewModel>>(orders);

            return View(orderVMs);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context
                                    .Orders
                                    .Include(order => order.Customer)
                                    .Include(order => order.Appliances)
                                    .Include(order => order.Parts)
                                    .Where(order => order.Id == id)
                                    .SingleOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }

            var orderDetailsVM = _mapper.Map<OrderDetailsViewModel>(order);
            return View(orderDetailsVM);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            //PopulateViewBags();

            var createOrderVM = new CreateOrderViewModel();

            createOrderVM.CustomerLookup = new SelectList(_context.Customers, "Id", "FullName");
            createOrderVM.ApplianceLookup = new MultiSelectList(_context.Appliances, "Id", "Brand", "Model");
            createOrderVM.PartLookup = new MultiSelectList(_context.Parts, "Id", "Name");


            return View(createOrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel createOrderVM)
        {

            if (ModelState.IsValid)
            {

                var order = _mapper.Map<Order>(createOrderVM);

                order.OrderTime = DateTime.Now;

                await UpdateOrderAppliances(order, createOrderVM.ApplianceIds);

                order.TotalPrice = GetOrderTotalPrice(order);

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            createOrderVM.CustomerLookup = new SelectList(_context.Customers, "Id", "FullName");
            createOrderVM.ApplianceLookup = new MultiSelectList(_context.Appliances, "Id", "Brand", "Model");
            createOrderVM.PartLookup = new MultiSelectList(_context.Parts, "Id", "Name");

            //PopulateViewBags();

            return View(createOrderVM);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context
                                    .Orders
                                    .Include(order => order.Appliances)
                                    .Include(order => order.Parts)
                                    .Include(order => order.Customer)
                                    .Where(order => order.Id == id)
                                    .SingleOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }

            //var updateOrderVM = _mapper.Map<UpdateOrderViewModel>(order);

            var updateOrderVM = new UpdateOrderViewModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Notes = order.Notes,
                OrderTime = order.OrderTime,
                IsPaid = order.IsPaid,
                ApplianceIds = order.Appliances.Select(a => a.Id).ToList(),
                PartIds = order.Parts.Select(p => p.Id).ToList(),
                ApplianceLookup = new MultiSelectList(_context.Appliances, "Id", "Brand", order.Appliances.Select(a => a.Id)),
                PartLookup = new MultiSelectList(_context.Parts, "Id", "Name", order.Parts.Select(p => p.Id)),
                CustomerLookup = new SelectList(_context.Customers, "Id", "FullName", order.CustomerId)
            };

            //updateOrderVM.CustomerLookup = new SelectList(_context.Customers, "Id", "FullName");
            //updateOrderVM.ApplianceLookup = new MultiSelectList(_context.Appliances, "Id", "Brand", "Model");
            //updateOrderVM.PartLookup = new MultiSelectList(_context.Parts, "Id", "Name");

            //PopulateViewBags(order);
            return View(updateOrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateOrderViewModel updateOrderVM)
        {
            if (id != updateOrderVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Get the order including Appliances and Parts from the DB
                var order = await _context
                                    .Orders
                                    .Include(order => order.Appliances)
                                    .Include(order => order.Parts)
                                    .Where(order => order.Id == id)
                                    .SingleOrDefaultAsync();

                if (order == null)
                {
                    return NotFound();
                }

                // Patch the order
                _mapper.Map(updateOrderVM, order);
                order.IsPaid = updateOrderVM.IsPaid;

                // Update order appliances
                await UpdateOrderAppliances(order, updateOrderVM.ApplianceIds);
                await UpdateOrderParts(order, updateOrderVM.PartIds);

                // Update order total price

                order.TotalPrice = GetOrderTotalPrice(order);
                

                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(updateOrderVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            updateOrderVM.CustomerLookup = new SelectList(_context.Customers, "Id", "FullName");
            updateOrderVM.ApplianceLookup = new MultiSelectList(_context.Appliances, "Id", "Brand", "Model");
            updateOrderVM.PartLookup = new MultiSelectList(_context.Parts, "Id", "Name");

            //PopulateViewBags();
            return View(updateOrderVM);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Private Methods
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private async Task UpdateOrderAppliances(Order order, List<int> ApplianceIds)
        {
            // Clear Order.Appliance
            order.Appliances.Clear();

            // Load Appliance from ApplianceIds
            var appliance = await _context
                                .Appliances
                                .Where(Appliance => ApplianceIds.Contains(Appliance.Id))
                                .ToListAsync();

            // Add Appliance to Order
            order.Appliances.AddRange(appliance);
        }

        private async Task UpdateOrderParts(Order order, List<int> PartIds)
        {
            // Clear Order.Part
            order.Parts.Clear();

            // Load Part from PartIds
            var part = await _context
                                .Parts
                                .Where(Part => PartIds.Contains(Part.Id))
                                .ToListAsync();

            // Add Appliance to Order
            order.Parts.AddRange(part);
        }


        private decimal GetOrderTotalPrice(Order order)
        {
            var appliancePrice = order.Appliances.Sum(a => a.Price);
            var partPrice = order.Parts.Sum(p => p.Price);
            return (appliancePrice + partPrice) * 1.16m;
        }

        //private decimal GetOrderTotalPrice(List<Appliance> appliances)
        //{
        //    var appliancePrice = appliances.Sum(Appliance => Appliance.Price);
        //    var applianceWithVAT = appliancePrice * 1.16m;

        //    return applianceWithVAT;
        //}

        //private decimal GetOrderTotalPriceParts(List<Part> parts)
        //{
        //    var partsPrice = parts.Sum(Appliance => Appliance.Price);
        //    var partsWithVAT = partsPrice * 1.16m;

        //    return partsWithVAT;
        //}

        private void PopulateViewBags(Order order = null)
        {
            ViewBag.CustomerId = new SelectList(_context.Customers, "Id", "FullName", order?.CustomerId);
            ViewBag.Appliances = new MultiSelectList(
                _context.Appliances,
                "Id",
                "Brand",
                order?.Appliances.Select(a => a.Id));
            ViewBag.PartIds = new MultiSelectList(
                _context.Parts,
                "Id",
                "Name",
                order?.Parts.Select(p => p.Id));
        }

        private async Task<decimal> CalculateTotalPriceAsync(List<int> applianceIds, List<int> partIds)
        {
            
            if (applianceIds == null) applianceIds = new List<int>();
            if (partIds == null) partIds = new List<int>();

            var appliances = await _context.Appliances
                                           .Where(a => applianceIds.Contains(a.Id))
                                           .ToListAsync();

            var parts = await _context.Parts
                                      .Where(p => partIds.Contains(p.Id))
                                      .ToListAsync();

            
            return appliances.Sum(a => a.Price) + parts.Sum(p => p.Price);
        }




        #endregion


    }
}
