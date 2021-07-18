using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ComputerCodeBlue.MyLaps;

namespace ComputerCodeBlue.MyLaps.Example.Features
{
    public class OrganizationRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }

        [FromQuery(Name = "count")]
        public int Count { get; set; }

        [FromQuery(Name = "offset")]
        public int Offset { get; set; }
    }

    public class OrganizationResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EventListItem> Events { get; set; }
        public DateTime Timestamp { get; set; }
    }


    public class Organization : BaseAsyncEndpoint
        .WithRequest<OrganizationRequest>
        .WithResponse<OrganizationResult>
    {

        [HttpGet("/organizations/{id}")]
        public override async Task<ActionResult<OrganizationResult>> HandleAsync([FromQuery] OrganizationRequest request, CancellationToken cancellationToken)
        {
            var org = await Results.GetOrganizationAsync(request.Id, request.Offset, request.Count);

            if (org != null)
            {
                var result = new OrganizationResult
                {
                    Id = org.Id,
                    Name = org.Name,
                    Events = org.Events,
                    Timestamp = DateTime.UtcNow
                };

                return Ok(result);
            }
            else
            {
                return NotFound();
            }


        }
    }


}
