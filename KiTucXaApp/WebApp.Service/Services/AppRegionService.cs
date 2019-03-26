using System.Linq;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;

namespace WebApp.Service.Services
{
    public interface IAppRegionService
    {
        IQueryable<AppRegion> GetRegions();

        AppRegion GetRegionById(int id);
        AppRegion AddRegion(AppRegion region);
        AppRegion UpdateRegion(AppRegion region);

        void DeleteRegion(int id);
        void SaveChanges();
    }

    public class AppRegionService : IAppRegionService
    {
        private IAppRegionRepository _appRegionRepository;
        private IUnitOfWork _unitOfWork;

        public AppRegionService(
            IAppRegionRepository appRegionRepository,
            IUnitOfWork unitOfWork)
        {
            this._appRegionRepository = appRegionRepository;
            this._unitOfWork = unitOfWork;
        }

        // *********************************
        // *********************************
        // *********************************

        public IQueryable<AppRegion> GetRegions()
        {
            return _appRegionRepository.GetAll().OrderBy(m => m.SortOrder);
        }


        public AppRegion GetRegionById(int id)
        {
            return _appRegionRepository.GetSingleById(id);
        }
        public AppRegion AddRegion(AppRegion region)
        {
            return _appRegionRepository.Add(region);
        } 
        public AppRegion UpdateRegion(AppRegion region)
        {
            return _appRegionRepository.Update(region);
        }

       
        public void DeleteRegion(int id)
        {
            _appRegionRepository.DeleteMulti(m => m.RegionId == id);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

    }
}
