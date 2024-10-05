using Note_Taking_App.Models;
using System.Collections.Generic;
using System.Linq;

namespace Note_Taking_App.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetNotesAsync(string search, string sort);
        Task<Note> GetNoteByIdAsync(int id);
        Task<Note> CreateNoteAsync(Note note);
        Task UpdateNoteAsync(int id, Note note);
        Task DeleteNoteAsync(int id);
    }
}