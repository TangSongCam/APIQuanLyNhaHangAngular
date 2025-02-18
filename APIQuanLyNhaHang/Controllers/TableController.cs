using APIQuanLyNhaHang.Data;
using APIQuanLyNhaHang.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class TableController : ControllerBase
{
    private readonly QLNhaHangDbContext _context;

    public TableController(QLNhaHangDbContext context)
    {
        _context = context;
    }

    // Lấy danh sách bàn (bao gồm cả tên và trạng thái)
    [HttpGet]
    public IActionResult GetTables()
    {
        var tables = _context.Tables.Select(t => new
        {
            t.Id,
            t.Name,
            t.Status
        }).ToList();

        return Ok(tables);
    }

    // Thêm bàn mới
    [HttpPost]
    public IActionResult AddTable([FromBody] Table newTable)
    {
        if (newTable == null ||
            string.IsNullOrWhiteSpace(newTable.Status) ||
            string.IsNullOrWhiteSpace(newTable.Name))
        {
            return BadRequest(new { message = "Tên hoặc trạng thái bàn không hợp lệ!" });
        }

        _context.Tables.Add(newTable);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTables), new { id = newTable.Id }, newTable);
    }

    // Cập nhật thông tin bàn (cập nhật tên và trạng thái)
    [HttpPut("{id}")]
    public IActionResult UpdateTable(int id, [FromBody] Table updatedTable)
    {
        var existingTable = _context.Tables.Find(id);
        if (existingTable == null)
        {
            return NotFound(new { message = "Bàn không tồn tại!" });
        }

        existingTable.Name = updatedTable.Name;
        existingTable.Status = updatedTable.Status;
        _context.SaveChanges();

        return Ok(existingTable);
    }

    // Xóa bàn
    [HttpDelete("{id}")]
    public IActionResult DeleteTable(int id)
    {
        var table = _context.Tables.Find(id);
        if (table == null)
        {
            return NotFound(new { message = "Bàn không tồn tại!" });
        }

        _context.Tables.Remove(table);
        _context.SaveChanges();

        return Ok(new { message = "Xóa bàn thành công!" });
    }
}
