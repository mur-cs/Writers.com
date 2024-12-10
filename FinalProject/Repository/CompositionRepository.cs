using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repository
{
    public class CompositionRepository : IComposition
    {
        private readonly ApplicationContext _context;
        public CompositionRepository(ApplicationContext context)
        {
            _context=context;
        }

        public IEnumerable<Composition> GetAllCompositions()
        {
            return _context.Compositions.Include(x=>x.User).ToList();
        }

        public Composition GetComposition(string compositionId)
        {
            var composition = _context.Compositions.Include(x => x.User).FirstOrDefault(x => x.Id==compositionId); 
            return composition;
        }
        public void AddComposition(Composition composition)
        {
            _context.Compositions.Add(composition);
            _context.SaveChanges();
        }
    }
}
