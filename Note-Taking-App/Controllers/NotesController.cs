//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Note_Taking_App.Data;
//using Note_Taking_App.Models;
//namespace Note_Taking_App.Controllers
//{


//    namespace NoteTakingAPI.Controllers
//    {
//        [Route("api/[controller]")]
//        [ApiController]
//        public class NotesController : ControllerBase
//        {
//            private readonly AppDbContext _context;

//            public NotesController(AppDbContext context)
//            {
//                _context = context;
//            }

//            // GET: api/notes
//            [HttpGet]
//            public async Task<ActionResult<IEnumerable<Note>>> GetNotes(string? search, string? sort)
//            {
//                var query = _context.Notes.AsQueryable();

//                if (!string.IsNullOrEmpty(search))
//                {
//                    query = query.Where(n => n.Title.Contains(search) || n.Content.Contains(search));
//                }

//                if (sort == "date")
//                {
//                    query = query.OrderBy(n => n.CreatedAt);
//                }

//                return await query.ToListAsync();
//            }


//            // GET: api/notes/5
//            [HttpGet("{id}")]
//            public async Task<ActionResult<Note>> GetNote(int id)
//            {
//                var note = await _context.Notes.FindAsync(id);

//                if (note == null)
//                {
//                    return NotFound();
//                }

//                return note;
//            }

//            // POST: api/notes
//            [HttpPost]
//            public async Task<ActionResult<Note>> PostNote(Note note)
//            {
//                _context.Notes.Add(note);
//                await _context.SaveChangesAsync();

//                return CreatedAtAction("GetNote", new { id = note.Id }, note);
//            }

//            // PUT: api/notes/5
//            [HttpPut("{id}")]
//            public async Task<IActionResult> PutNote(int id, Note note)
//            {
//                if (id != note.Id)
//                {
//                    return BadRequest();
//                }

//                _context.Entry(note).State = EntityState.Modified;

//                try
//                {
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!NoteExists(id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }

//                return NoContent();
//            }

//            // DELETE: api/notes/5
//            [HttpDelete("{id}")]
//            public async Task<IActionResult> DeleteNote(int id)
//            {
//                var note = await _context.Notes.FindAsync(id);
//                if (note == null)
//                {
//                    return NotFound();
//                }

//                _context.Notes.Remove(note);
//                await _context.SaveChangesAsync();

//                return NoContent();
//            }

//            private bool NoteExists(int id)
//            {
//                return _context.Notes.Any(e => e.Id == id);
//            }

//        }
//    }

//}
// Controllers/NotesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Note_Taking_App.Models;
using Note_Taking_App.Interfaces;
using Note_Taking_App.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Note_Taking_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // GET: api/notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes(string? search, string? sort)
        {
            var notes = await _noteService.GetNotesAsync(search, sort);
            return Ok(notes);
        }

        // GET: api/notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // POST: api/notes
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(Note note)
        {
            var createdNote = await _noteService.CreateNoteAsync(note);
            return CreatedAtAction(nameof(GetNote), new { id = createdNote.Id }, createdNote);
        }

        // PUT: api/notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            try
            {
                await _noteService.UpdateNoteAsync(id, note);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }
    }
}
