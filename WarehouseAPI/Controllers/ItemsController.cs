using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.DTOs;
using WarehouseAPI.Entities;
using WarehouseAPI.Helpers;

namespace WarehouseAPI.Controllers;

[Route("api/items")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ItemsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;
    private string container = "items";

    public ItemsController(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
    {
        _context = context;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDTO>> Get(int id)
    {
        var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        var dto = _mapper.Map<ItemDTO>(item);
        return dto;
    }


    [HttpGet("filter")]
    public async Task<ActionResult<List<ItemDTO>>> Filter([FromQuery] FilterItemsDTO filterItemsDto)
    {
        var itemsQueryable = _context.Items.AsQueryable();

        if (!string.IsNullOrEmpty(filterItemsDto.Title))
        {
            itemsQueryable = itemsQueryable.Where(x => x.Title.Contains(filterItemsDto.Title));
        }

        if (filterItemsDto.SerialNumber != 0)
        {
            itemsQueryable = itemsQueryable.Where(x => x.SerialNumber == filterItemsDto.SerialNumber.ToString());
        }

        var items = await itemsQueryable.ToListAsync();
        return _mapper.Map<List<ItemDTO>>(items);
    }

    [HttpGet("PostGet")]
    public async Task<ActionResult<ItemPostGetDTO>> PostGet()
    {
        var storages = await _context.Storages.ToListAsync();
        var storagesDTO = _mapper.Map<List<StorageDTO>>(storages);
        return new ItemPostGetDTO() { Storage = storagesDTO };
    }

    [HttpPost]
    public async Task<ActionResult<int>> Post([FromForm] ItemCreationDTO itemCreationDto)
    {
        var item = _mapper.Map<Item>(itemCreationDto);
        if (itemCreationDto.Image != null)
        {
            item.Image = await _fileStorageService.SaveFile(container, itemCreationDto.Image);
        }

        _context.Add(item);
        await _context.SaveChangesAsync();
        return item.Id;
    }
}