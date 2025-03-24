using SportsPro.Models;
using System.Linq;

namespace SportsPro.Utilities
{
    public class IncidentFilters
    {
        //Constants for filter values
        public const string AllFilter = "all";
        public const string UnassignedFilter = "unassigned";
        public const string OpenFilter = "open";

        public string FilterString { get; }

        //Helper properties for check state
        public bool IsUnassigned => FilterString == UnassignedFilter;
        public bool IsOpen => FilterString == OpenFilter;
        public bool IsAll => FilterString == AllFilter;

        public IncidentFilters(string filterString)
        {
            FilterString = filterString ?? AllFilter;
        }

        //Apply filter to query
        public static IQueryable<Incident> ApplyFilter(IQueryable<Incident> query, IncidentFilters filters)
        {
            if (filters.IsUnassigned)
            {
                query = query.Where(i => i.TechnicianID == -1);
            }
            else if (filters.IsOpen)
            {
                query = query.Where(i => i.DateClosed == null);
            }

            return query;
        }
    }
}