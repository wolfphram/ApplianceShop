using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplianceShop.Data;
using ApplianceShop.Entities.Customers;
using AutoMapper;
using ApplianceShop.Models.Customers;
using ApplianceShop.Entities.Appliances;
using ApplianceShop.Models.Appliances;

namespace ApplianceShop.Controllers
{
    public class CustomersController : Controller
    {
        #region Data and Constructor
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CustomersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Actions
        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _context
                                        .Customers
                                        .ToListAsync();

            var customersVMs = _mapper.Map<List<Customer>, List<CustomerViewModel>>(customers);

            return View(customersVMs);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context
                                    .Customers
                                    .Include(customer => customer.Orders)
                                    .Where(customer => customer.Id == id)
                                    .SingleOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }

            var customerVM = _mapper.Map<CustomerDetailsViewModel>(customer);

            return View(customerVM);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel createCustomerVM)
        {
            if (ModelState.IsValid)
            {
                var customer = _mapper.Map<Customer>(createCustomerVM);

                _context.Add(createCustomerVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createCustomerVM);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context
                                        .Customers
                                        .Where(customer => customer.Id == id)
                                        .SingleOrDefaultAsync();



            if (customer == null)
            {
                return NotFound();
            }

            var updateCustomerVM = _mapper.Map<UpdateCustomerViewModel>(customer);

            return View(updateCustomerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCustomerViewModel updateCustomerVM)
        {
            if (id != updateCustomerVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var customer = await _context
                                               .Customers
                                               .Where(customer => customer.Id == id)
                                               .SingleOrDefaultAsync();

                if (customer == null) return NotFound();


                try
                {
                    _mapper.Map(updateCustomerVM, customer);

                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(updateCustomerVM.Id))
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
            return View(updateCustomerVM);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Private Methods
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        } 

        #endregion
    }
}
