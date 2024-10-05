using Microsoft.EntityFrameworkCore;
using Note_Taking_App.Data;
using Note_Taking_App.Models;
using Note_Taking_App.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Note_Taking_App.Services
{
    public class NoteService : INoteService
    {
        private readonly AppDbContext _context;

        public NoteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetNotesAsync(string search, string sort)
        {
            var query = _context.Notes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(n => n.Title.Contains(search) || n.Content.Contains(search));
            }

            if (sort == "date")
            {
                query = query.OrderBy(n => n.CreatedAt);
            }

            return await query.ToListAsync();
        }

        public async Task<Note> GetNoteByIdAsync(int id)
        {
            return await _context.Notes.FindAsync(id);
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task UpdateNoteAsync(int id, Note note)
        {
            if (id != note.Id)
            {
                throw new ArgumentException("Note ID mismatch");
            }

            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNoteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }
    }
}
