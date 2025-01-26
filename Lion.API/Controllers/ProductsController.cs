using Lion.API.Authorization;
using Lion.Application.Common.Interfaces;
using Lion.Application.DTOs;
using Lion.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lion.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    [Authorize(Policy = Permissions.ViewProduct)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name!,
            Description = p.Description,
            Price = p.Price
        });

        return Ok(productDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name!,
            Description = product.Description,
            Price = product.Price
        };

        return Ok(productDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = Permissions.CreateProduct)]
    public async Task<ActionResult<ProductDto>> Create(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price
        };

        await _productRepository.AddAsync(product);

        productDto.Id = product.Id;
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, productDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = Permissions.EditProduct)]
    public async Task<IActionResult> Update(int id, ProductDto productDto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.Price = productDto.Price;

        await _productRepository.UpdateAsync(product);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.DeleteProduct)]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        await _productRepository.DeleteAsync(id);

        return NoContent();
    }
}
