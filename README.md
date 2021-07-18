# ComputerCodeBlue.MyLaps

ComputerCodeBlue.MyLaps is a .NET 5.0 library used to get results from the Speedhive MyLaps website. The intent is for this library to be used to automate points for racing series. Since MyLaps doesn't have a public API, this library works using the HtmlAgilityPack library to navigate through the various pages posted on speedhive.mylaps.com.

## Features

All methods for retrieving data from MyLaps are asynchronous. The library also features the ability to cache results using EntityFrameworkCore. MySql, Sqlite, and SQL Server are supported.

## Installation

Clone the repository and build the solution, or you can install this as a NuGet package in your own projects.

## Usage

Use the static ComputerCodeBlue.MyLaps.Results class to get results from MyLaps. All results are returned as .NET objects. 

### Basic Functions

- GetOrganizationAsync(int organizationId, int offset, int count)
- GetEventAsync(int eventId)
- GetRunAsync(int runId)

Each of these functions returns a basic object that represents a single page on the MyLaps site. GetOrganizationAsync returns a list of events along with the organization information. The "offset" and "count" parameters are for pagination for events. The default is to return 25 events, but you can load as many as 100 with no ill effects.

### Advanced Functions

- GetEventWithRunsAsync(int eventId, MyLapsContext context, bool refresh)

This function will load an event, then load each run in the event and return a comprehensive object that includes the entirety of that event's results. This can take some time, so this is where the caching functionality using EFCore comes in. If an event with this event's ID exists in the database, this function will return the cached event and results by default, unless you set the optional "refresh" parameter to true. The database context parameter is optional, if a context isn't provided, the function will always load the results from the MyLaps website

### Limitations

Because of the way that MyLaps stores and structures data, the individual results are all returned as strings, even the field that reports positions, because positions can be numeric data or things like "DQ" for disqualified, "DNS" for did not start, and "DNF" for did not finish. The TopSpeed field is returned with units, i.e. "44.511 miles/h," and if no laps are completed Laps will be "-" instead of a zero. TotalTime might be an empty string if the type of run doesn't support this field. Diff can be expressed as a timespan or as a number of laps, such as "6.369" to represent 6.369 seconds or "2 laps" to mean that competitor finished two laps down. If you need this data parsed to a numeric or other type, it's up to you to do this in your application.

That said, because each entry is processed in the order it is displayed on the MyLaps website, you can be confident in assigning any points based on the order the results appear, unless, of course, you modify that order somehow in your code.

Navigation properties are provided in the EventWithRunsResult and child objects. If you are using some kind of JSON library like System.Text.Json to return your objects via an API, be aware that those navigation properties can create circular references if you try to return these objects directly. I suggest returning a view model without those properties instead.

## Example

An example is provided. It uses a MySql database to cache results and uses the Ardalis.ApiEndpoints library to serve JSON data.
