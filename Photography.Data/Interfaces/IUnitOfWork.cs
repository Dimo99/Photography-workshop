using Photography.Models;

namespace Photography.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Accessory> Accessories { get; }
        IRepository<Camera> Cameras { get; }
        IRepository<Lens> Lenses { get; }
        IRepository<Photographer> Photographers { get; }
        IRepository<Workshop> Worksops { get; }
        void Save();
    }
}