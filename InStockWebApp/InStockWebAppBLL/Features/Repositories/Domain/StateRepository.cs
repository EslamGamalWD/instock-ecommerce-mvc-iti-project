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
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public StateRepository(ApplicationDbContext applicationDbContext) : base(
            applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public override Task Add(State entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(State entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(State entity)
        {
            throw new NotImplementedException();
        }
    }
}
