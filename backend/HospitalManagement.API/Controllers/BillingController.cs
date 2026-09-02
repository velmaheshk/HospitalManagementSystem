using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillRepository _billService;

        public BillingController(IBillRepository billService)
        {
            _billService = billService;
        }

        // GET: api/Billing
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bills = await _billService.GetAll();

            return Ok(bills);
        }

        // GET: api/Billing/1
        [HttpGet("{billId}")]
        public async Task<IActionResult> GetById(int billId)
        {
            var bill = await _billService.GetById(billId);

            if (bill == null)
            {
                return NotFound(new
                {
                    message = "Bill not found"
                });
            }

            return Ok(bill);
        }

        // POST: api/Billing
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _billService.Add(bill);

            return CreatedAtAction(
                nameof(GetById),
                new { billId = result.BillId },
                result
            );
        }

        // PUT: api/Billing/1
        [HttpPut("{billId}")]
        public async Task<IActionResult> Update(
            int billId,
            [FromBody] Bill bill)
        {
            if (billId != bill.BillId)
            {
                return BadRequest(new
                {
                    message = "Bill ID does not match"
                });
            }

            var existingBill = await _billService.GetById(billId);

            if (existingBill == null)
            {
                return NotFound(new
                {
                    message = "Bill not found"
                });
            }

            var result = await _billService.Update(bill);

            return Ok(result);
        }

        // DELETE: api/Billing/1
        [HttpDelete("{billId}")]
        public async Task<IActionResult> Delete(int billId)
        {
            var result = await _billService.Delete(billId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Bill not found"
                });
            }

            return Ok(new
            {
                message = "Bill deleted successfully"
            });
        }
    }
}