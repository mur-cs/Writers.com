using FinalProject.Models;

namespace FinalProject.Interfaces
{
    public interface IComposition
    {
            IEnumerable<Composition> GetAllCompositions();
            Composition GetComposition(string compositionName);
        void AddComposition(Composition composition);
    }
}
