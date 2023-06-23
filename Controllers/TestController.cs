using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Store_ASP.NET_Core_MVC.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Online_Store_ASP.NET_Core_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET: api/<TestController>
        [HttpGet]
        [Route("Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value MT 1 ", "value MT 2" };
        }

        // GET: api/<TestController>
        [HttpGet]
        [Route("GetUser")]
        [Authorize(Roles = UsersRoles.USER)]
        public IEnumerable<string> GetUser()
        {
            return new string[] { "value USER 1 ", "value USER 2" };
        }

        // GET: api/<TestController>
        [HttpGet]
        [Route("GetAdmin")]
        [Authorize(Roles = UsersRoles.ADMIN)]
        public IEnumerable<string> GetAdmin()
        {
            return new string[] { "value ADMIN 3 ", "value ADMIN 4" };
        }

        // GET: api/<TestController>
        [HttpGet]
        [Route("GetOwner")]
        [Authorize(Roles = UsersRoles.OWNER)]
        public IEnumerable<string> GetOwner()
        {
            return new string[] { "value OWNER 5 ", "value OWNER 6" };
        }

        // GET api/<TestController>/5
        /*[HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }*/

        // POST api/<TestController>
       /* [HttpPost]
        public void Post([FromBody] string value)
        {
        }*/

        // PUT api/<TestController>/5
       /* [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }*/

        // DELETE api/<TestController>/5
       /* [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
