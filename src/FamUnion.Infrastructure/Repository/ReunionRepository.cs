using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamUnion.Infrastructure.Repository
{
    public class ReunionRepository : DbAccess<Reunion>, IReunionRepository
    {
        public ReunionRepository(string connection) 
            : base(connection)
        {

        }

        public Reunion GetReunion(Guid id)
        {
            ParameterDictionary parameters = new ParameterDictionary(new string[]
            {
                "id", id.ToString()
            });
            return ExecuteStoredProc<Reunion>("[dbo].[spGetReunionById]", parameters).SingleOrDefault();
        }

        public IEnumerable<Reunion> GetReunions()
        {
            return ExecuteStoredProc<Reunion>("[dbo].[spGetReunions]", ParameterDictionary.Empty);
        }

        public Reunion SaveReunion(Reunion reunion)
        {
            ParameterDictionary parameters = new ParameterDictionary(new string[] {
                "id", (reunion.Id ?? Guid.Empty) == Guid.Empty ? Guid.NewGuid().ToString() : reunion.Id.ToString(),
                "name", reunion.Name,
                "description", reunion.Description,
                "startDate", reunion.StartDate.ToString(),
                "endDate", reunion.EndDate.ToString()
            });

            return ExecuteStoredProc<Reunion>("[dbo].[spSaveReunion]", parameters).SingleOrDefault();
        }
    }
}
