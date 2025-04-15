using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplianceShop.Data;
using ApplianceShop.Entities.Parts;
using AutoMapper;
using ApplianceShop.Models.Parts;

namespace ApplianceShop.Controllers
{
    public class PartsController : Controller
    {
        #region Data and Constructor
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PartsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Actions
        // GET: Parts
        public async Task<IActionResult> Index()
        {
            var parts = await _context
                                    .Parts
                                    .ToListAsync();
;
            var partsVMs = _mapper.Map<List<PartViewModel>>(parts);

            return View(partsVMs);
        }

        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context
                                    .Parts
                                    .Include(part => part.Appliances)
                                    .Where(part => part.Id == id)
                                    .SingleOrDefaultAsync();
            if (part == null)
            {
                return NotFound();
            }

            var partsDetailsVM = _mapper.Map<PartDetailsViewModel>(part);

            return View(partsDetailsVM);
        }

        // GET: Parts/Create
        public IActionResult Create()
        {
            var createPartVM = new CreatePartViewModel();

            return View(createPartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePartViewModel createPartVM)
        {
            if (ModelState.IsValid)
            {
                var part = _mapper.Map<Part>(createPartVM);


                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createPartVM);
        }

        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var part = await _context
                                    .Parts
                                    .Where(part => part.Id == id)
                                    .SingleOrDefaultAsync();


            if (part == null)
            {
                return NotFound();
            }

            var updatePartVM = _mapper.Map<UpdatePartViewModel>(part);

            return View(updatePartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePartViewModel updatePartVM)
        {
            if (id != updatePartVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var part = await _context
                                         .Parts
                                         .Where(part => part.Id == id)
                                         .SingleOrDefaultAsync();

                if (part == null) return NotFound();

                _mapper.Map(updatePartVM, part);

                try
                {
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(updatePartVM.Id))
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
            return View(updatePartVM);
        }

        // GET: Parts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var part = await _context.Parts.FindAsync(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Private Methods
        private bool PartExists(int id)
        {
            return _context.Parts.Any(e => e.Id == id);
        } 
        #endregion
    }
}
