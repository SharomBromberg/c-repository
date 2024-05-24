using FlightAPP.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightAPP.Contexts
{
    public class FlightContext : DbContext
    {
        public FlightContext(DbContextOptions<FlightContext> options) : base(options)
        {
        }
        public DbSet<JourneyEntity> Journeys { get; set; }
        public DbSet<FlightEntity> Flights { get; set; }
        public DbSet<TransportEntity> Transports { get; set; }
    }
}