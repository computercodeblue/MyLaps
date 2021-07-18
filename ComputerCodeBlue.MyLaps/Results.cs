using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using RestSharp;

namespace ComputerCodeBlue.MyLaps
{
    public static class Results
    {
        public async static Task<OrganizationResult> GetOrganizationAsync(int organizationId, int offset, int count)
        {
            string infoUrl = $"https://speedhive.mylaps.com/Organizations/{organizationId}";
            string url = infoUrl + "/LoadMoreEvents";
            count = count < 25 ? 25 : count;
            offset = offset < 1 ? 0 : offset;

            HtmlWeb web = new HtmlWeb();
            HtmlDocument org = await web.LoadFromWebAsync(infoUrl);

            string orgName = org.GetElementbyId("organization-header")?.SelectNodes("//h1").FirstOrDefault()?.InnerHtml ?? string.Empty;

            if (string.IsNullOrWhiteSpace(orgName)) return null;

            var client = new RestClient(url);
            var req = new RestRequest(Method.POST);
            client.Timeout = -1;

            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            req.AddParameter("id", organizationId);
            req.AddParameter("count", count);
            req.AddParameter("offset", offset);

            var res = await client.ExecuteAsync(req);

            List<EventListItem> mylapsEvents = new List<EventListItem>();
            string htmlContent = string.Empty;

            if (res.IsSuccessful)
            {
                var events = new HtmlDocument();
                htmlContent = res.Content.Replace("\r\n", string.Empty);
                events.LoadHtml($"<html><body>{htmlContent}</body></html");

                mylapsEvents = events.DocumentNode.SelectNodes("//a").Select(n => new EventListItem
                {
                    Id = int.Parse(n.Attributes["href"].Value.Split('/').Last()),
                    Name = n.ChildNodes[5].InnerText.Trim(),
                    Date = DateTime.Parse(n.ChildNodes[7].InnerText.Trim())
                }).ToList();
            }

            return new OrganizationResult
            {
                Id = organizationId,
                Name = orgName,
                Events = mylapsEvents
            };
        }

        public async static Task<EventResult> GetEventAsync(int eventId)
        {
            string url = $"https://speedhive.mylaps.com/Events/{eventId}";

            HtmlWeb web = new HtmlWeb();
            HtmlDocument eventHtml = await web.LoadFromWebAsync(url);

            string eventName = eventHtml.GetElementbyId("event-details-headerstop")?.SelectNodes("//h1").FirstOrDefault()?.InnerText;
            string dateString = eventHtml.GetElementbyId("event-details-headerstop")?.SelectNodes("//div[@class='race-date']").FirstOrDefault()?.InnerText.Substring(7);
            DateTime raceDate = DateTime.Parse(dateString);

            if (string.IsNullOrWhiteSpace(eventName)) return null;

            var groups = eventHtml.DocumentNode.SelectNodes("//div[@class='row-event']").Select(n => new GroupResult
            {
                Name = n.SelectNodes("div/div/h3").FirstOrDefault().InnerText,
                Runs = n.SelectNodes("a").Select(a => new RunListItem
                {
                    Id = int.Parse(a.Attributes["href"].Value.Split('/').Last()),
                    Name = a.SelectNodes("div/div[@class='row-event-name']").FirstOrDefault().InnerText,
                    Time = string.IsNullOrWhiteSpace(a.SelectNodes("div/div/div[@class='row-event-date']").FirstOrDefault().InnerText) ? null :
                            DateTime.Parse(a.SelectNodes("div/div/div[@class='row-event-date']").FirstOrDefault().InnerText + " " + raceDate.Year.ToString() + " "
                                            + a.SelectNodes("div/div/div[@class='row-event-time']").FirstOrDefault().InnerText),
                    Type = a.SelectNodes("div/div/span").FirstOrDefault().HasClass("ico-practice") ? "practice" :
                            a.SelectNodes("div/div/span").FirstOrDefault().HasClass("ico-qualify") ? "qualify" :
                            a.SelectNodes("div/div/span").FirstOrDefault().HasClass("ico-points-merge") ? "merge" : "race"
                }).ToList()
            }).ToList();

            return new EventResult
            {
                Id = eventId,
                Name = eventName,
                Date = raceDate,
                Groups = groups
            };
        }

        public async static Task<RunResult> GetRunAsync(int runId)
        {
            string url = $"https://speedhive.mylaps.com/Sessions/{runId}#byClass";

            HtmlWeb web = new HtmlWeb();
            HtmlDocument eventHtml = await web.LoadFromWebAsync(url);

            string sessionName = string.Empty;

            try
            {
                sessionName = eventHtml.DocumentNode.SelectNodes("//h1").FirstOrDefault().InnerText.Replace(@"\r\n", string.Empty).Trim();
            }
            catch
            {
                return new RunResult
                {
                    Id = 0,
                    Classes = new List<ClassResult>(),
                    Name = "Error",
                    Type = "error"
                };
            }

            string type = eventHtml.DocumentNode.SelectNodes("//ul/li/a[@class='clearfix border-active']/span/span").FirstOrDefault().HasClass("ico-practice") ? "practice" :
                            eventHtml.DocumentNode.SelectNodes("//ul/li/a[@class='clearfix border-active']/span/span").FirstOrDefault().HasClass("ico-qualify") ? "qualify" :
                            eventHtml.DocumentNode.SelectNodes("//ul/li/a[@class='clearfix border-active']/span/span").FirstOrDefault().HasClass("ico-points-merge") ? "merge" : "race";

            var result = new RunResult();
            result.Id = runId;
            result.Name = sessionName;
            result.Type = type;
            result.Classes = new List<ClassResult>();

            if (type != "merge")
            {
                var nodes = eventHtml.GetElementbyId("session-results").ChildNodes;
                ClassResult classResult = null;

                foreach (var node in nodes)
                {
                    if (node.HasClass("row-event"))
                    {
                        if (node.HasClass("row-event-classname-wrapper"))
                        {
                            if (classResult != null) result.Classes.Add(new ClassResult
                            {
                                Name = classResult.Name,
                                Results = new List<DriverResult>(classResult.Results)
                            });
                            classResult = new ClassResult();
                            classResult.Results = new List<DriverResult>();
                            classResult.Name = node.SelectNodes("div[@class='row-event-classname']/h2").FirstOrDefault().InnerText;
                        }
                        else
                        {
                            classResult.Results.Add(new DriverResult
                            {
                                Position = node.SelectNodes("div[@class='row-event-position']/span").FirstOrDefault().InnerText,
                                RaceNumber = node.SelectNodes("div[@class='row-event-racenumber']").FirstOrDefault().InnerText,
                                Competitor = node.SelectNodes("div[@class='row-event-competitor']/a/span").FirstOrDefault().InnerText,
                                TotalTime = node.SelectNodes("div[@class='row-event-totaltime']")?.FirstOrDefault()?.InnerText ?? string.Empty,
                                Diff = node.SelectNodes("div[@class='row-event-diff']").FirstOrDefault().InnerText,
                                Laps = node.SelectNodes("div[@class='row-event-laps']").FirstOrDefault().InnerText,
                                BestTime = node.SelectNodes("div[@class='row-event-besttime']").FirstOrDefault().InnerText,
                                BestLap = node.SelectNodes("div[@class='row-event-bestlap']").FirstOrDefault().InnerText,
                                TopSpeed = node.SelectNodes("div[@class='row-event-topspeed']").FirstOrDefault().InnerText
                            });
                        }
                    }
                }

                if (classResult != null) result.Classes.Add(new ClassResult
                {
                    Name = classResult.Name,
                    Results = new List<DriverResult>(classResult.Results)
                });
            }

            return result;
        }

        public async static Task MigrateDatabaseAsync(MyLapsContext context)
        {
            await context.Database.MigrateAsync();
        }

        public async static Task<EventWithRunsResult> GetEventWithRunsAsync(int eventId, MyLapsContext context = null, bool refresh = false)
        {
           
            var _event = await GetEventAsync(eventId);
            EventWithRunsResult result = null;
            bool update = false;

            if (_event == null) return null;

            if (context != null)
            {
                if ((await context.Database.GetPendingMigrationsAsync()).Count() > 0)
                {
                    throw new InvalidOperationException("Cannot update the database with pending migrations. Run the MigrateDatabaseAsync() method in your application.");
                }

                result = await context.Events.Where(e => e.Id == eventId)
                    .Include(e => e.Groups)
                    .ThenInclude(g => g.Runs)
                    .ThenInclude(r => r.Classes)
                    .ThenInclude(c => c.Results)
                    .FirstOrDefaultAsync();
            }

            if (result == null)
            {
                result = new EventWithRunsResult
                {
                    Name = _event.Name,
                    Date = _event.Date,
                    Id = _event.Id,
                    Groups = new List<GroupWithRunsResult>()
                };
            }
            else
            {
                update = true;
            }

            refresh = refresh ? true : _event.Groups.Count != result.Groups.Count;

            if (refresh)
            {
                foreach (GroupResult group in _event.Groups)
                {
                    var newGroup = new GroupWithRunsResult
                    {
                        Name = group.Name,
                        Runs = new List<RunResult>()
                    };

                    foreach (RunListItem run in group.Runs)
                    {
                        newGroup.Runs.Add(await GetRunAsync(run.Id));
                    }
                    result.Groups.Add(newGroup);
                }
            }

            if (context != null && refresh)
            {
                if (update)
                {
                    context.Events.Update(result);
                }
                else
                {
                    await context.Events.AddAsync(result);
                }
                await context.SaveChangesAsync();
            }

            return result;
        }
    }
}
