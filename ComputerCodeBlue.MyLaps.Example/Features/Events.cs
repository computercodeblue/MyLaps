using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ComputerCodeBlue.MyLaps;

namespace ComputerCodeBlue.MyLaps.Example.Features
{
    public class EventRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }

    public class EventResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<GroupResult> Groups { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Event : BaseAsyncEndpoint
        .WithRequest<EventRequest>
        .WithResponse<EventResult>
    {

        [HttpGet("/events/{id}")]
        public override async Task<ActionResult<EventResult>> HandleAsync([FromRoute] EventRequest request, CancellationToken cancellationToken)
        {
            var _event = await Results.GetEventAsync(request.Id);

            if (_event != null)
            {
                var result = new EventResult
                {
                    Id = _event.Id,
                    Name = _event.Name,
                    Date = _event.Date,
                    Groups = _event.Groups,
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