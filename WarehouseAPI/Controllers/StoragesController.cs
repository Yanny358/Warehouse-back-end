using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.DTOs;
using WarehouseAPI.Entities;

namespace WarehouseAPI.Controllers;

[Route("api/storages")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class StoragesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public StoragesController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<StorageDTO>>>Get()
    {
        var storages =  await _context.Storages.ToListAsync();
        return _mapper.Map<List<StorageDTO>>(storages);

    }
    
    [HttpGet("{Id:int}")]
    public async Task<ActionResult<StorageDTO>> Get(int Id)
    {
        var storage = await _context.Storages.FirstOrDefaultAsync(x => x.Id == Id);

        if (storage == null)
        {
            return NotFound();
        }

        return _mapper.Map<StorageDTO>(storage);
    }

    [HttpPost]
    public async Task<ActionResult> Post(StorageCreationDTO storageCreationDto)
    {
        var storage = _mapper.Map<Storage>(storageCreationDto);
        _context.Add(storage);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpPut("{Id:int}")]
    public async Task<ActionResult> Put(int Id, StorageCreationDTO storageCreationDto)
    {
        var storage = await _context.Storages.FirstOrDefaultAsync(x => x.Id == Id);

        if (storage == null)
        {
            return NotFound();
        }

        storage = _mapper.Map(storageCreationDto, storage);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{Id:int}")]
    public async Task<ActionResult>  Delete(int Id)
    {
        var storage = await _context.Storages.FirstOrDefaultAsync(x => x.Id == Id);
        if (storage == null)
        {
            return NotFound();
        }

        _context.Remove(storage);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}