using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ComputerCodeBlue.MyLaps;

namespace ComputerCodeBlue.MyLaps.Example.Features
{
    public class RunRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }

    public class RunResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<ClassResult> Classes { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Run : BaseAsyncEndpoint
        .WithRequest<RunRequest>
        .WithResponse<RunResult>
    {
        [HttpGet("/sessions/{id}")]
        public override async Task<ActionResult<RunResult>> HandleAsync([FromRoute] RunRequest request, CancellationToken cancellationToken)
        {
            var result = await Results.GetRunAsync(request.Id);

            return new RunResult
            {
                Id = result.Id,
                Name = result.Name,
                Type = result.Type,
                Classes = result.Classes,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
