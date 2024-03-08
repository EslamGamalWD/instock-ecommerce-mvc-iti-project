﻿using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;

namespace InStockWebAppBLL.Features.Repositories.Domain;

public class SubCategoryRepository : GenericRepository<SubCategory>, ISubCategoryRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SubCategoryRepository(ApplicationDbContext applicationDbContext) : base(
        applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public override Task Add(SubCategory entity)
    {
        throw new NotImplementedException();
    }

    public override void Delete(SubCategory entity)
    {
        throw new NotImplementedException();
    }

    public override void Update(SubCategory entity)
    {
        throw new NotImplementedException();
    }
}