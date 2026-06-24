using FinanceTracker.Core.DTOs;
using FinanceTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;
        public WatchlistController(IWatchlistService watchlistService)
        {
            _watchlistService = watchlistService;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _watchlistService.GetAllAsync(GetUserId());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _watchlistService.GetByIdAsync(id, GetUserId());
            if (item == null)
                return NotFound(new { message = $"Id {id} bulunamadı." });

            return Ok(item);
        }
        // İSTEK ÜZERİNDEN DROPdOWN LA aLERT DİRECTİON U ÇEKTİREBİLİRSİN. bUNU DA DATABASE TARAFINDA STRİNG OLARAK TUTABİLİRSİN. SONRASINDA
        //  sTRİNG İFADEYİ PRİCEcALC BU STRİNG İFADEYİ ENUM PARSE SONRAIDA BUNLA KIYASLAYABİLİRSİN.

        /*
         kULLANICI wATCHLİST OLUŞTURDU.
        wATCHLİSTLERİNİ bELİRLEDİĞİ PRİCE DEĞERİNİ SETLEDİ.

        gİRİŞ YAPABİLİYOR SİSTEME 



         */

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWatchlistItemDto dto)
        {
            var created = await _watchlistService.AddAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}/refresh")]
        public async Task<IActionResult> RefreshPrice(int id)
        {
            var updated = await _watchlistService.RefreshPriceAsync(id, GetUserId());
            if (updated == null)
                return NotFound(new { message = $"Id {id} bulunamadı." });

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _watchlistService.DeleteAsync(id, GetUserId());
            if (!deleted)
                return NotFound(new { message = $"Id {id} bulunamadı." });

            return NoContent();
        }
    }
}
