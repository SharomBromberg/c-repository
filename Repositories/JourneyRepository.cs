// using FlightAPP.Contexts;
// using FlightAPP.Core.Entities;
// using FlightAPP.Interfaces;
// using Microsoft.EntityFrameworkCore;
// using Newtonsoft.Json;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;

// namespace FlightAPP.Repositories
// {
//     public class JourneyRepository : IJourneyRepository
//     {
//         private readonly FlightContext _context;

//         public JourneyRepository(FlightContext context)
//         {
//             _context = context;

//             if (!_context.Flights.Any())
//             {
//                 var flights = LoadFlightsFromJson();
//                 _context.Flights.AddRange(flights);
//                 _context.SaveChanges();
//             }
//         }

//         private List<FlightEntity> LoadFlightsFromJson()
//         {
//             var json = File.ReadAllText("markets.json");
//             var flights = JsonConvert.DeserializeObject<List<FlightEntity>>(json);
//             return flights ?? new List<FlightEntity>();
//         }

//         public IQueryable<FlightEntity> GetOneWayFlights(string origin, string destination, bool direct)
//         {
//             var query = _context.Flights.AsQueryable();

//             if (direct)
//             {
//                 query = query.Where(f => f.Origin == origin && f.Destination == destination);
//             }
//             else
//             {
//                 query = query.Where(f => f.Origin == origin || f.Destination == destination);
//             }

//             return query;
//         }

//         public IQueryable<FlightEntity> GetRoundTripFlights(string origin, string destination, bool direct)
//         {
//             var query = _context.Flights.AsQueryable();

//             if (direct)
//             {
//                 query = query.Where(f => (f.Origin == origin && f.Destination == destination) || (f.Origin == destination && f.Destination == origin));
//             }
//             else
//             {
//                 query = query.Where(f => f.Origin == origin || f.Destination == destination || f.Origin == destination || f.Destination == origin);
//             }

//             return query;
//         }
//     }
// }