using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
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
            const string sql = "SELECT * FROM sp_get_reunion_by_id(@id)";
            return (await ExecuteStoredProc(sql, ParameterDictionary.Single("id", id))
                .ConfigureAwait(false)).SingleOrDefault();
        }

        public async Task<IEnumerable<Reunion>> GetReunionsAsync()
        {
            return await ExecuteStoredProc("SELECT * FROM sp_get_reunions()")
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Reunion>> GetManageReunionsAsync(string userId)
        {
            const string sql = "SELECT * FROM sp_get_manage_reunions(@userId)";
            return await ExecuteStoredProc(sql, ParameterDictionary.Single("userId", userId))
                .ConfigureAwait(false);
        }

        public async Task<Reunion> SaveReunionAsync(Reunion reunion)
        {
            if (!reunion.IsValid())
            {
                throw new Exception($"Reunion is not valid|{JsonConvert.SerializeObject(reunion)}");
            }

            const string sql = "SELECT * FROM sp_save_reunion(@id, @userId, @name, @description, @startDate, @endDate)";
            ParameterDictionary parameters = new ParameterDictionary(
                "id",          reunion.Id ?? Guid.NewGuid(),
                "userId",      reunion.ActionUserId,
                "name",        reunion.Name,
                "description", reunion.Description,
                "startDate",   reunion.StartDate,
                "endDate",     reunion.EndDate
            );

            return (await ExecuteStoredProc(sql, parameters).ConfigureAwait(false)).SingleOrDefault();
        }

        public async Task CancelReunionAsync(CancelRequest request)
        {
            const string sql = "SELECT sp_delete_reunion_by_id(@reunionId)";
            await ExecuteNonQueryProc(sql, ParameterDictionary.Single("reunionId", request.EntityId))
                .ConfigureAwait(false);
        }

        public async Task AddReunionOrganizer(Guid reunionId, string email)
        {
            const string sql = "SELECT sp_add_reunion_organizer(@reunionId, @email)";
            ParameterDictionary parameters = new ParameterDictionary(
                "reunionId", reunionId,
                "email",     email
            );
            await ExecuteNonQueryProc(sql, parameters).ConfigureAwait(false);
        }

        public async Task RemoveReunionOrganizer(Guid reunionId, string email)
        {
            const string sql = "SELECT sp_remove_reunion_organizer(@reunionId, @email)";
            ParameterDictionary parameters = new ParameterDictionary(
                "reunionId", reunionId,
                "email",     email
            );
            await ExecuteNonQueryProc(sql, parameters).ConfigureAwait(false);
        }
    }
}
