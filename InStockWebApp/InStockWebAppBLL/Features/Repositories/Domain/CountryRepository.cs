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
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CountryRepository(ApplicationDbContext applicationDbContext) : base(
            applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public override Task Add(Country entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Country entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(Country entity)
        {
            throw new NotImplementedException();
        }
    }
}
