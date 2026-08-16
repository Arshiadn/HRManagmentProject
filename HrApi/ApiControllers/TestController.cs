using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;


[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("error")]
    public IActionResult Error()
    {
        throw new Exception(
            "Test unhandled exception"
        );
    }
}
