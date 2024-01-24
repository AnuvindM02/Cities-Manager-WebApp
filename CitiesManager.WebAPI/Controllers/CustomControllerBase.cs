using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")] //Use when version specified in Url
    //[Route("api/[controller]")] //Use when version specified as query string "api-version" or inside request header
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
    }
}