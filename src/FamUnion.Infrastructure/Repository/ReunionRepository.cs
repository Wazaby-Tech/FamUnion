using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Repository
{
    public class ReunionRepository : DbAccess<Reunion>, IReunionRepository
    {
        public ReunionRepository(string connection) 
            : base(connection)
        {

        }

        public async Task<Reunion> GetReunionAsync(Guid id)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("id", id.ToString());
            return (await ExecuteStoredProc("[dbo].[spGetReunionById]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }

        public async Task<IEnumerable<Reunion>> GetReunionsAsync()
        {
            return await ExecuteStoredProc("[dbo].[spGetReunions]", ParameterDictionary.Empty)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<Reunion> SaveReunionAsync(Reunion reunion)
        {
            ParameterDictionary parameters = new ParameterDictionary(new string[] {
                "id", reunion.Id.GetDbGuidString(),
                "name", reunion.Name,
                "description", reunion.Description,
                "startDate", reunion.StartDate.ToString(),
                "endDate", reunion.EndDate.ToString()
            });

            return (await ExecuteStoredProc("[dbo].[spSaveReunion]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }
    }
}
