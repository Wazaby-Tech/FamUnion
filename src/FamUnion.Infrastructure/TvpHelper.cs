using Dapper;
using FamUnion.Core.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FamUnion.Infrastructure
{
    public static class TvpHelper
    {
        public static SqlMapper.ICustomQueryParameter MapInvites(IEnumerable<InviteRequest> requests)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ReunionId", typeof(Guid));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Name", typeof(string));

            foreach(var request in requests)
            {
                DataRow row = dt.NewRow();
                row["ReunionId"] = request.ReunionId;
                row["Email"] = request.Email;
                row["Name"] = request.Name;
                dt.Rows.Add(row);
            }

            return dt.AsTableValuedParameter("[dbo].[udfInviteType]");
        }
    }
}
