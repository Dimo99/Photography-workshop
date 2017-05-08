using Photography.Data.Interfaces;
using Photography.Models;

namespace Photography.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PhotographyContext context;
        private IRepository<Accessory> accessories;
        private IRepository<Camera> cameras;
        private IRepository<Lens> lenses;
        private IRepository<Photographer> photographers;
        private IRepository<Workshop> workshops;
        public UnitOfWork()
        {
            this.context = new PhotographyContext();
        }

        public IRepository<Accessory> Accessories => this.accessories ?? (this.accessories = new Repository<Accessory>(this.context.Accessories));
        public IRepository<Camera> Cameras => this.cameras ?? (this.cameras = new Repository<Camera>(this.context.Cameras));

        public IRepository<Lens> Lenses => this.lenses ?? (this.lenses = new Repository<Lens>(this.context.Lenses));

        public IRepository<Photographer> Photographers => this.photographers ?? (this.photographers = new Repository<Photographer>(this.context.Photographers));

        public IRepository<Workshop> Worksops => this.workshops ?? (this.workshops = new Repository<Workshop>(this.context.Workshops));

        public void Save()
        {
            context.SaveChanges();
        }
    }
}