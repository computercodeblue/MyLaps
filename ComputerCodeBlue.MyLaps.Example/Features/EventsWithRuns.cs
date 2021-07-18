using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ComputerCodeBlue.MyLaps.Example.Models;

namespace ComputerCodeBlue.MyLaps.Example.Features
{
    public class EventsWithRunsRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }

    public class EventWithRunsResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<MyLapsGroup> Groups { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class EventWithRuns : BaseAsyncEndpoint
        .WithRequest<EventsWithRunsRequest>
        .WithResponse<EventWithRunsResult>
    {
        private readonly MyLapsContext _myLapsContext;

        public EventWithRuns(MyLapsContext myLapsContext)
        {
            _myLapsContext = myLapsContext;
        }


        [HttpGet("/eventswithruns/{id}")]
        public override async Task<ActionResult<EventWithRunsResult>> HandleAsync([FromRoute] EventsWithRunsRequest request, CancellationToken cancellationToken)
        {
            var _event = await Results.GetEventWithRunsAsync(request.Id, _myLapsContext);

            if (_event != null)
            {
                var result = new EventWithRunsResult
                {
                    Id = _event.Id,
                    Name = _event.Name,
                    Date = _event.Date,
                    Groups = _event.Groups.Select(g => new MyLapsGroup
                    {
                        EventId = g.EventId,
                        Id = g.Id,
                        Name = g.Name,
                        Runs = g.Runs.Select(r => new MyLapsRun
                        {
                            Classes = r.Classes.Select(c => new MyLapsClass
                            {
                                Id = c.Id,
                                Results = c.Results.Select(re => new MyLapsDriver
                                {
                                    BestLap = re.BestLap,
                                    BestTime = re.BestTime,
                                    RaceNumber = re.RaceNumber,
                                    ClassId = re.ClassId,
                                    Competitor = re.Competitor,
                                    Diff = re.Diff,
                                    Id = re.Id,
                                    Laps = re.Laps,
                                    Position = re.Position,
                                    TopSpeed = re.TopSpeed,
                                    TotalTime = re.TotalTime
                                }).ToList(),
                                RunId = c.RunId,
                                Name = c.Name
                            }).ToList(),
                            GroupId = r.GroupId,
                            Id = r.Id,
                            Name = r.Name,
                            Type = r.Type
                        }).ToList()
                    }).ToList(),
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
