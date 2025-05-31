using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplianceShop.Data;
using ApplianceShop.Entities.Appliances;
using AutoMapper;
using ApplianceShop.Models.Appliances;
using Microsoft.AspNetCore.Mvc.Rendering;
using ApplianceShop.Models.Orders;

namespace ApplianceShop.Controllers
{
    public class AppliancesController : Controller
    {
        #region Data and Constructor
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public AppliancesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Actions

        // GET: Appliances
        public async Task<IActionResult> Index()
        {
            var appliances = await _context
                                    .Appliances
                                    .ToListAsync();

            var appliancesVMs = _mapper.Map<List<Appliance>, List<ApplianceViewModel>>(appliances);

            return View(appliancesVMs);
        }

        // GET: Appliances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appliance = await _context
                                        .Appliances
                                        .Where(appliance => appliance.Id == id)
                                        .SingleOrDefaultAsync();

            if (appliance == null)
            {
                return NotFound();
            }

            var applianceVM = _mapper.Map<ApplianceDetailsViewModel>(appliance);

            return View(applianceVM);
        }

        // GET: Appliances/Create
        public IActionResult Create()
        {
            var createApplianceVM = new CreateApplianceViewModel();

            return View(createApplianceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateApplianceViewModel createApplianceVM)
        {
            if (ModelState.IsValid)
            {
                var appliance = _mapper.Map<Appliance>(createApplianceVM);


                _context.Add(appliance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createApplianceVM);
        }

        // GET: Appliances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appliance = await _context
                                        .Appliances
                                        .Where(appliance => appliance.Id == id)
                                        .SingleOrDefaultAsync();

            if (appliance == null)
            {
                return NotFound();
            }

            var updateApplianceVM = _mapper.Map<UpdateApplianceViewModel>(appliance);

            return View(updateApplianceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateApplianceViewModel updateApplianceVM)
        {
            if (id != updateApplianceVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var appliance = await _context
                                               .Appliances
                                               .Where(appliance => appliance.Id == id)
                                               .SingleOrDefaultAsync();


                if (appliance == null) return NotFound();

                try
                {
                    _mapper.Map(updateApplianceVM, appliance);


                    _context.Update(appliance);
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplianceExists(updateApplianceVM.Id))
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
            return View(updateApplianceVM);
        }

        // GET: Appliances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appliance = await _context.Appliances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appliance == null)
            {
                return NotFound();
            }

            return View(appliance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appliance = await _context.Appliances.FindAsync(id);
            if (appliance != null)
            {
                _context.Appliances.Remove(appliance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Private Methods
        private bool ApplianceExists(int id)
        {
            return _context.Appliances.Any(e => e.Id == id);
        }


        #endregion
    }
}
