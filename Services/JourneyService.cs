using FlightAPP.Interfaces;
using FlightAPP.Models;
using Newtonsoft.Json;
using RestSharp;
public class JourneyService : IJourneyService
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "markets.json");
    private List<Flight> _flights;
    private readonly ICurrencyService _currencyService;

    public JourneyService(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
        _flights = new List<Flight>();
        InitializeFlightsAsync().Wait();
    }

    private async Task InitializeFlightsAsync()
    {
        var json = await File.ReadAllTextAsync(_filePath);
        _flights = JsonConvert.DeserializeObject<List<Flight>>(json) ?? new List<Flight>();
    }

    public async Task<IEnumerable<Flight>> GetAllFlights()
    {
        return await Task.FromResult(_flights.AsEnumerable());
    }

    private void FindRoutes(string current, string destination, HashSet<string> visited, List<List<Flight>> routes, List<Flight> route)
    {
        var flights = _flights.Where(f => f.Origin == current && !visited.Contains(f.Destination));
        foreach (var flight in flights)
        {
            route.Add(flight);
            if (flight.Destination == destination)
            {
                routes.Add(new List<Flight>(route));
            }
            else
            {
                visited.Add(flight.Destination);
                FindRoutes(flight.Destination, destination, visited, routes, route);
                visited.Remove(flight.Destination);
            }
            route.Remove(flight);
        }
    }

    public async Task<IEnumerable<List<Flight>>> GetOneWayFlights(string origin, string destination, string currency, bool allowStops)
    {
        var routes = new List<List<Flight>>();
        var visited = new HashSet<string>();
        var route = new List<Flight>();
        visited.Add(origin);
        var oneWayFlights = _flights.Where(f => f.Origin == origin && f.Destination == destination).ToList();
        FindRoutes(origin, destination, visited, routes, route);

        foreach (var routeList in routes)
        {
            foreach (var flight in routeList)
            {
                flight.Price = await _currencyService.ConvertCurrency("USD", currency, flight.Price);
            }
        }

        return await Task.FromResult(routes.AsEnumerable());
    }
    public async Task<IEnumerable<List<Flight>>> GetRoundTripFlights(string origin, string destination, string currency, bool allowStops)
    {
        var routes = new List<List<Flight>>();
        var visited = new HashSet<string>();
        var route = new List<Flight>();

        visited.Add(origin);
        FindRoutes(origin, destination, visited, routes, route);
        var outboundFlights = routes;

        routes = new List<List<Flight>>();
        visited.Clear();
        route.Clear();

        visited.Add(destination);
        FindRoutes(destination, origin, visited, routes, route);
        var returnFlights = routes;

        var roundTrips = new List<List<Flight>>();
        foreach (var outbound in outboundFlights)
        {
            foreach (var returnFlight in returnFlights)
            {
                var roundTrip = new List<Flight>();
                roundTrip.AddRange(outbound.Select(flight => flight.Clone()));
                roundTrip.AddRange(returnFlight.Select(flight => flight.Clone()));
                roundTrips.Add(roundTrip);
            }
        }

        foreach (var routeList in roundTrips)
        {
            foreach (var flight in routeList)
            {
                flight.Price = await _currencyService.ConvertCurrency("USD", currency, flight.Price);
            }
        }

        return roundTrips;
    }

    public Task<IEnumerable<List<Flight>>> GetOneWayFlights(string origin, string destination, string currency, bool allowStops, bool direct)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<List<Flight>>> GetRoundTripFlights(string origin, string destination, string currency, bool allowStops, bool direct)
    {
        throw new NotImplementedException();
    }
}