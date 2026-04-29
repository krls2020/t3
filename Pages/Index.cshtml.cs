using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Reminders.Data;
using Reminders.Models;

namespace Reminders.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db) => _db = db;

    public List<Reminder> Active { get; private set; } = new();
    public List<Reminder> Done { get; private set; } = new();

    [BindProperty] public string Title { get; set; } = "";
    [BindProperty] public string? Notes { get; set; }
    [BindProperty] public DateTime? DueAt { get; set; }

    public async Task OnGetAsync()
    {
        Active = await _db.Reminders.Where(r => !r.Completed).OrderBy(r => r.DueAt ?? DateTime.MaxValue).ThenByDescending(r => r.CreatedAt).ToListAsync();
        Done = await _db.Reminders.Where(r => r.Completed).OrderByDescending(r => r.CreatedAt).Take(20).ToListAsync();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        if (string.IsNullOrWhiteSpace(Title)) return RedirectToPage();
        _db.Reminders.Add(new Reminder
        {
            Title = Title.Trim(),
            Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
            DueAt = DueAt,
        });
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleAsync(int id)
    {
        var r = await _db.Reminders.FindAsync(id);
        if (r != null) { r.Completed = !r.Completed; await _db.SaveChangesAsync(); }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = await _db.Reminders.FindAsync(id);
        if (r != null) { _db.Reminders.Remove(r); await _db.SaveChangesAsync(); }
        return RedirectToPage();
    }
}
