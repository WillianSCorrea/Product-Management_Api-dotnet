using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Dto;
using Product_Management.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/categories")]
[Authorize(Roles = "Admin")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync();

        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        var category = new Category { Name = dto.Name };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
