using FlightAPP.Core.Entities;

namespace FlightAPP.Interfaces
{
    public interface IJourneyRepository
    {
        IQueryable<FlightEntity> GetOneWayFlights(string origin, string destination, bool direct);
        IQueryable<FlightEntity> GetRoundTripFlights(string origin, string destination, bool direct);
    }
}