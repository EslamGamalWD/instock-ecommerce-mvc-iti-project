using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStockWebAppBLL.Features.Repositories.Domain
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CityRepository(ApplicationDbContext applicationDbContext) : base(
            applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public override Task Add(City entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(City entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(City entity)
        {
            throw new NotImplementedException();
        }
    }
}
