using Application.Services;
using Domain.Constants;
using Domain.Dtos;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace BarkAndBuy.WebAPI.Controllers;

///<summary>
///Controller for managing order information
///</summary>
///
[ApiController]
[Route("categories")]
public class CategoriesController : ControllerBase
{
    private readonly MyDbContext _ctx;

    ///<summary>
    ///Create controller with order service injection
    ///</summary>
    public CategoriesController(MyDbContext ctx)
    {
        _ctx = ctx;
    }
    
// quick quick for test

    [HttpGet("GetCategories")]
    public IEnumerable<Category> GetCategories() //best practice: dto
    {
        return _ctx.Categories;
    }
    
    [HttpGet("GetCategoriesWithProducts")]
    public IEnumerable<Category> GetCategoriesWithProducts() //best practice: dto
    {
        return _ctx.Categories.Include(x => x.Products); //islus, nes possible circle reference. apejimas - su jsondeserializer. Bet neelegantiska. Geriau yra pasidaryti dto response'ui. ir tada is jo isfigureoutinti.
    }
}
