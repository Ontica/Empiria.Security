/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization services                *
*  Assembly : Empiria.Security.dll                         Pattern   : Service provider                      *
*  Type     : AuthorizationServiceProvider                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides subject's authorization services.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Security.Providers;

using Empiria.Security.SecurityItems.Adapters;

namespace Empiria.Security.Services {

  /// <summary>Provides subject's authorization services.</summary>
  internal class AuthorizationServiceProvider : IAuthorizationProvider {

    public FixedList<string> GetFeaturesPermissions(EmpiriaIdentity subject,
                                                      IClientApplication clientApp) {
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(clientApp, nameof(clientApp));

      var context = DetermineSecurityContext(clientApp);

      var permissionsBuilder = new PermissionsBuilder(subject, context);

      return permissionsBuilder.BuildFeatures()
                               .Select(x => x.Key)
                               .ToFixedList();
    }


    public FixedList<IObjectAccessRule> GetObjectAccessRules(EmpiriaIdentity subject,
                                                               IClientApplication clientApp) {
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(clientApp, nameof(clientApp));

      var context = DetermineSecurityContext(clientApp);

      var permissionsBuilder = new PermissionsBuilder(subject, context);

      return permissionsBuilder.BuildObjectAccessRules()
                               .Select(x => MapToObjectAccessRulesDto(x))
                               .ToFixedList();
    }


    public FixedList<string> GetRoles(EmpiriaIdentity subject,
                                        IClientApplication clientApp) {
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(clientApp, nameof(clientApp));

      var context = DetermineSecurityContext(clientApp);

      var permissionsBuilder = new PermissionsBuilder(subject, context);

      return permissionsBuilder.BuildRoles()
                               .Select(x => x.Key)
                               .ToFixedList();
    }


    public bool IsSubjectInRole(IIdentifiable subject, IClientApplication clientApp, string role) {
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(clientApp, nameof(clientApp));
      Assertion.Require(role, nameof(role));

      var context = DetermineSecurityContext(clientApp);

      return Role.IsSubjectInRole(subject, context, role);
    }

    #region Helpers

    private SecurityContext DetermineSecurityContext(IClientApplication clientApp) {
      return SecurityContext.ParseWith(clientApp);
    }

    private IObjectAccessRule MapToObjectAccessRulesDto(ObjectAccessRule rule) {
      return new ObjectAccessRuleDto {
        TypeName = rule.TypeName,
        ObjectsUIDs = rule.ObjectsUIDs.ToArray()
      };
    }


    #endregion Helpers

  }  // class AuthorizationServiceProvider

}  // namespace Empiria.Security.Services
