using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WashLux.Converters;
using WashLux.Models;

namespace WashLux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RowsController : Controller
    {
        private readonly RowContext _context;

        public RowsController(RowContext context)
        {
            _context = context;
        }

        // GET: api/Rows
        [HttpGet]
        [AllowAnonymous]
        public List<RowDto> GetRows()
        {
            List<RowDto> result = RowConverter.RowConvert(_context.Rows.ToList());
            foreach (var row in result)
                if (row.Type == "MultiService")
                    foreach (var name in _context.Services.Where(u => u.RowId == row.Id).ToList())
                        row.Names.Add(name.Name);
            return result;
        }
        [HttpGet("{table}")]
        [AllowAnonymous]
        public List<RowDto> GetTable([FromRoute] string table)
        {
            List<RowDto> result = RowConverter.RowConvert(
                _context.Rows.Where(u => u.Table == table).ToList());
            foreach (var row in result)
                if (row.Type == "MultiService")
                    foreach (var name in _context.Services.Where(u => u.RowId == row.Id).ToList())
                        row.Names.Add(name.Name);
            result.Sort(new Services.PosComparer());
            return result;
        }
        

        // GET: api/Rows/id/5
        [HttpGet("id/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRow([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rawRow = await _context.Rows.FindAsync(id);
            
            if (rawRow == null)
            {
                return NotFound();
            }

            var row = RowConverter.RowConvert(rawRow);

            if (row.Type == "MultiService")
                foreach (var name in _context.Services.Where(u => u.RowId == row.Id).ToList())
                    row.Names.Add(name.Name);

            return Ok(row);
        }

        [HttpGet("up/{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> PutUp([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Row row = await _context.Rows.FindAsync(id);
            Row row2 = null;
            int min = 999999;

            foreach(var r in _context.Rows.ToList())
                if(r.Pos > row.Pos && r.Pos < min && r.Table == row.Table)
                {
                    row2 = r;
                    min = r.Pos;
                }

            if(min != _context.Rows.Count())
            {
                var temp = row.Pos;
                row.Pos = row2.Pos;
                row2.Pos = temp;
                await _context.SaveChangesAsync();
                return Ok(true);
            }

            return NotFound();
        }
        
        [HttpGet("down/{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> PutDown([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Row row = await _context.Rows.FindAsync(id);
            Row row2 = null;
            int max = -1;

            foreach (var r in _context.Rows.ToList())
                if (r.Pos < row.Pos && r.Pos > max && r.Table == row.Table)
                {
                    row2 = r;
                    max = r.Pos;
                }

            if(max != -1)
            {
                var temp = row.Pos;
                row.Pos = row2.Pos;
                row2.Pos = temp;
                await _context.SaveChangesAsync();
                return Ok(true);
            }

            return NotFound();
        }

        // PUT: api/Rows/5

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<bool>> PutRow([FromBody] RowDto row)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            while (_context.Services.Any(u => u.RowId == row.Id))
            {
                _context.Services.Remove(await _context.Services.FirstOrDefaultAsync(u => u.RowId == row.Id));
                await _context.SaveChangesAsync();
            }
            _context.Update(RowConverter.RowConvert(row));

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RowExists(row.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(true);
        }

        // DELETE: api/Rows/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRow([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var row = await _context.Rows.FindAsync(id);
            if (row == null)
            {
                return NotFound();
            }

            _context.Rows.Remove(row);
            await _context.SaveChangesAsync();

            return Ok(row);
        }

        // POST: api/Rows
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Row>> PostRow([FromBody] RowDto row)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var r in _context.Rows.ToList())
                if (r.Table == row.Table && r.Pos >= row.Pos)
                    r.Pos++;
            _context.Rows.Add(RowConverter.RowConvert(row));
            await _context.SaveChangesAsync();

            return Ok(row);
        }

        

        private bool RowExists(int id)
        {
            return _context.Rows.Any(e => e.Id == id);
        }

        [HttpPost("json")]
        [Authorize]
        public async Task<ActionResult<List<RowDto>>> FromJson([FromBody] List<RowDto> rows)
        {
            int pos = 1;
            foreach (var row in rows)
            {
                row.Pos = pos;
                pos++;
                _context.Rows.Add(RowConverter.RowConvert(row));
            }
            
            await _context.SaveChangesAsync();

            return Ok(rows);
        }
    }
}