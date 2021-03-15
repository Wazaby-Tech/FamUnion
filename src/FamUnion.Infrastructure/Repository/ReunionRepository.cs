using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Reunion>> GetManageReunionsAsync()
        {
            return await ExecuteStoredProc("[dbo].[spGetManageReunions]", ParameterDictionary.Empty)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<Reunion> SaveReunionAsync(Reunion reunion)
        {
            if(!reunion.IsValid())
            {
                throw new Exception($"Reunion is not valid|{JsonConvert.SerializeObject(reunion)}");
            }

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

        public async Task DeleteReunionAsync(Guid id)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("reunionId", id);

            _ = await ExecuteStoredProc("[dbo].[spDeleteReunionById]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
