using System.Reflection;
using draft_ml.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace draft_ml.Controllers
{
    [ApiController]
    public class HealthController() : ControllerBase
    {
        [HttpGet("diagnostics")]
        public async Task<IActionResult> Diagnostics()
        {
            var response = new Dictionary<string, bool>();

            var dbContext = HttpContext.RequestServices.GetRequiredService<DietDbContext>();

            response["Database"] = await dbContext.Database.CanConnectAsync();

            return Ok(response);
        }

        [HttpGet("version")]
        public async Task<IActionResult> Version()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi =
                System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion!;

            return Ok(version);
        }

        [HttpGet("authtest")]
        [Authorize]
        public async Task<IActionResult> AuthTest()
        {
            return Ok(HttpContext.User.Claims.First(c => c.Type.Equals("id")).Value);
        }
    }
}
