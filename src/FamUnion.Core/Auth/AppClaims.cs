using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace FamUnion.Core.Auth
{
    public class AppClaimPolicy
    {
        // Policy Names
        public const string Access = "Access";
        public const string Manage = "Manage";
        public const string Admin = "Admin";

        public static IEnumerable<(string PolicyName, AuthorizationPolicy Policy)> AppPolicies => new List<(string, AuthorizationPolicy)>()
        {
            (Access, new AuthorizationPolicyBuilder()
                .RequireClaim(AppClaims.PermissionsType, AppClaims.GetActionClaims(AppClaims.View).Select(c => c.ToString()).ToArray())
                .Build()),
            (Manage, new AuthorizationPolicyBuilder()
                .RequireClaim(AppClaims.PermissionsType, AppClaims.GetActionClaims(AppClaims.Manage).Select(c => c.ToString()).ToArray())
                .Build()),
            (Admin, new AuthorizationPolicyBuilder()
                .RequireClaim(AppClaims.PermissionsType, AppClaims.GetActionClaims(AppClaims.Admin).Select(c => c.ToString()).ToArray())
                .Build())
        };
    }

    public class AppClaims
    {
        public static string PermissionsType = "permissions";

        // Entities
        public static string Access = "access";
        public static string Reunions = "reunions";
        public static string Admin = "admin";

        // Actions
        public static string View = "view";
        public static string Manage = "manage";
        public static string System = "system";


        private static readonly IEnumerable<(string action, string entity)> _internalClaims = new List<(string, string)>()
        {
            (View, Access),
            (Manage, Reunions),
            (System, Admin)
        };

        public static readonly IEnumerable<AppClaim> AllClaims = _internalClaims.Select(ic => new AppClaim(ic.action, ic.entity)).ToList();
        public static IEnumerable<AppClaim> GetEntityClaims(string entity)
        {
            return AllClaims.Where(ac => ac.Entity == entity).ToList();
        }

        public static IEnumerable<AppClaim> GetActionClaims(string action)
        {
            return AllClaims.Where(ac => ac.Action == action).ToList();
        }
    }

    public class AppClaim
    {
        public AppClaim(string action, string entity)
        {
            Action = action;
            Entity = entity;
        }
        public string Action { get; }
        public string Entity { get; }
        public string ClaimType { get; } = AppClaims.PermissionsType;

        public override string ToString()
        {
            return $"{Action}:{Entity}";
        }
    }

}
