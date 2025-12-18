using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Dto;
using Product_Management.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1,
        int pageSize = 10,
        int? categoryId = null)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        var total = await query.CountAsync();

        var products = await query.Skip((page - 1) * pageSize).Take(pageSize).Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.Category.Name))
                .ToListAsync();

        return Ok(new { total, page, pageSize, products });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        product.IsDeleted = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
