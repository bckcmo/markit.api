using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Markit.Api.Models.Dtos
{
    public class MarkitApiResponse
    {
        public int StatusCode { get; set; } = StatusCodes.Status200OK;
        public List<string> Errors { get; set; }
        public object Data { get; set; }
    }
}