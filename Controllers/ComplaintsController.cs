using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComplaintBox.Models;

namespace ComplaintBox.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly ComplaintBoxDbContext _context;

        public ComplaintsController(ComplaintBoxDbContext context)
        {
            _context = context;
        }

        // GET: Complaints
        public async Task<IActionResult> Index()
        {
            var complaintBoxDbContext = _context.Complaints.Include(c => c.AdminAlloted).Include(c => c.PinCodeNavigation).Include(c => c.Victim);
            return View(await complaintBoxDbContext.ToListAsync());
        }

        // GET: Complaints/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.AdminAlloted)
                .Include(c => c.PinCodeNavigation)
                .Include(c => c.Victim)
                .FirstOrDefaultAsync(m => m.ComplaintId == id);
            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        // GET: Complaints/Create
        public IActionResult Create()
        {
            ViewData["AdminAllotedId"] = new SelectList(_context.Admins, "AdminId", "AdminId");
            ViewData["PinCode"] = new SelectList(_context.AreaInfos, "PinCode", "PinCode");
            ViewData["VictimId"] = new SelectList(_context.VictimInfos, "VictimId", "VictimId");
            return View();
        }

        // POST: Complaints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComplaintType,Description,StreetNo,BuildingNo,PinCode,VictimName,VictimAge,VictimGender,Images")] ComplaintInfo complaintInfo)
        {
            if (ModelState.IsValid)
            {
                // Create a new Victim_Info entry if victim details are provided
                if (!string.IsNullOrEmpty(complaintInfo.VictimName))
                {
                    VictimInfo victimInfo = new VictimInfo
                    {
                        VictimName = complaintInfo.VictimName,
                        VictimAge = complaintInfo.VictimAge,
                        VictimGender = complaintInfo.VictimGender
                    };

                    _context.VictimInfos.Add(victimInfo);
                    await _context.SaveChangesAsync(); // Save to trigger the ID generation
                }

                // Create a new Complaint entry
                Complaint complaint = new Complaint
                {
                    ComplaintType = complaintInfo.ComplaintType,
                    DateTimeLodged = DateTime.Now,
                    StreetNo = complaintInfo.StreetNo,
                    BuildingNo = complaintInfo.BuildingNo,
                    Description = complaintInfo.Description,
                    PinCode = complaintInfo.PinCode,
                    Images = complaintInfo.Images
                };

                _context.Complaints.Add(complaint);
                await _context.SaveChangesAsync(); // Save to trigger the ID generation and other logic in the trigger

                // Create UserComplaint relationship
                var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailId == HttpContext.Session.GetString("EmailId"));
                if (user != null)
                {
                    UserComplaint userComplaint = new UserComplaint
                    {
                        UserId = user.UserId,
                        ComplaintId = complaint.ComplaintId
                    };

                    _context.UserComplaints.Add(userComplaint);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Populate ViewData for dropdowns if ModelState is not valid
            ViewData["AdminAllotedId"] = new SelectList(_context.Admins, "AdminId", "AdminId", null);
            ViewData["PinCode"] = new SelectList(_context.AreaInfos, "PinCode", "PinCode", complaintInfo.PinCode);
            ViewData["VictimId"] = new SelectList(_context.VictimInfos, "VictimId", "VictimId", null);

            return View(complaintInfo);
        }

        // GET: Complaints/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }
            ViewData["AdminAllotedId"] = new SelectList(_context.Admins, "AdminId", "AdminId", complaint.AdminAllotedId);
            ViewData["PinCode"] = new SelectList(_context.AreaInfos, "PinCode", "PinCode", complaint.PinCode);
            ViewData["VictimId"] = new SelectList(_context.VictimInfos, "VictimId", "VictimId", complaint.VictimId);
            return View(complaint);
        }

        // POST: Complaints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ComplaintId,ComplaintType,Description,DateTimeLodged,ComplaintStatus,StreetNo,BuildingNo,PinCode,VictimId,AdminAllotedId,Images")] Complaint complaint)
        {
            if (id != complaint.ComplaintId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(complaint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComplaintExists(complaint.ComplaintId))
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
            ViewData["AdminAllotedId"] = new SelectList(_context.Admins, "AdminId", "AdminId", complaint.AdminAllotedId);
            ViewData["PinCode"] = new SelectList(_context.AreaInfos, "PinCode", "PinCode", complaint.PinCode);
            ViewData["VictimId"] = new SelectList(_context.VictimInfos, "VictimId", "VictimId", complaint.VictimId);
            return View(complaint);
        }

        // GET: Complaints/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.AdminAlloted)
                .Include(c => c.PinCodeNavigation)
                .Include(c => c.Victim)
                .FirstOrDefaultAsync(m => m.ComplaintId == id);
            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        // POST: Complaints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint != null)
            {
                _context.Complaints.Remove(complaint);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComplaintExists(string id)
        {
            return _context.Complaints.Any(e => e.ComplaintId == id);
        }
    }
}
