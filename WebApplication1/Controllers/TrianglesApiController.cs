using System.Net;
using System.Net.Http;
using System.Web.Http;
using System;
using System.Collections.Generic;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class TrianglesApiController : ApiController
    {
        private TrianglesService TrianglesService => new TrianglesService();

        /// <summary>
        /// Given a row and column, like "A1", "B5", "D12", etc, return 6 coordinates that represent the 3 vertices of the triangle in that position.
        /// </summary>
        [Route("api/TrianglesApi/{row}/{column}")]
        [HttpPost]
        public HttpResponseMessage Get(string row, int column)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, TrianglesService.GetVertices(row, column));
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Given 6 coordinates (3 vertices), find the name of the cell that the triangle occupies.  Right now I'm just returning a 400 if the vertices don't correspond
        /// to a triangle.
        /// 
        /// Not the cleanest of all routes.  Actually, it's straight up sloppy.  Plus the method's name isn't very fancy, is it?
        /// </summary>
        [Route("api/TrianglesApi/{v1x}/{v1y}/{v2x}/{v2y}/{v3x}/{v3y}")]
        [HttpPost]
        public HttpResponseMessage Get(int v1x, int v1y, int v2x, int v2y, int v3x, int v3y)
        {
            try
            {
                // That Tuple List is a tragic readability fail, but it's packaged together that way to make the coords sortable later.
                return Request.CreateResponse(HttpStatusCode.OK, TrianglesService.GetTriangleFromVertices(new List<Tuple<int, int>> { new Tuple<int, int>(v1x, v1y), new Tuple<int, int>(v2x, v2y), new Tuple<int, int>(v3x, v3y) }));
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}