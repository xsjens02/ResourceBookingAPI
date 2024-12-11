using Microsoft.AspNetCore.Mvc;
using ResourceBookingAPI.Interfaces.Controllers.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Controllers
{
    public interface IErrorReportController : 
        ICrudController<ErrorReport, string>
    {
        Task<IActionResult> GetResourceStatus([FromQuery] string resourceId);
    }
}
