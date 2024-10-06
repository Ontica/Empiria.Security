/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : EmpiriaPrincipal                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents the security context of the user or access account on whose behalf the Empiria      *
*             framework code is running, including that user's identity (EmpiriaIdentity) and any domain     *
*             roles to which they belong. This class cannot be derived.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Linq;

using System.Security.Principal;

using Empiria.Contacts;
using Empiria.Collections;

using Empiria.Security.Providers;

using Empiria.Security.Data;

namespace Empiria.Security {

  /// <summary>Represents the security context of the user or access account on whose behalf the Empiria
  /// framework code is running, including that user's identity (EmpiriaIdentity) and any domain
  /// roles to which they belong. This class can't be derived.</summary>
  internal sealed class EmpiriaPrincipal : IEmpiriaPrincipal {

    #region Fields

    static private EmpiriaDictionary<string, EmpiriaPrincipal> principalsCache =
                                        new EmpiriaDictionary<string, EmpiriaPrincipal>(128);

    private readonly Lazy<SecurityObjects> _securityObjects;

    #endregion Fields

    #region Constructors and parsers

    internal EmpiriaPrincipal(EmpiriaIdentity identity,
                              IClientApplication clientApp,
                              IEmpiriaSession session) {

      Assertion.Require(identity, nameof(identity));
      Assertion.Require(clientApp, nameof(clientApp));
      Assertion.Require(session, nameof(session));

      if (!identity.IsAuthenticated) {
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }

      this.Initialize(identity, clientApp, session);

      _securityObjects = new Lazy<SecurityObjects>(() => new SecurityObjects(this));
    }

    /// <summary>Initializes a new instance of the EmpiriaPrincipal class from an authenticated
    /// EmpiriaIdentity. Fails if identity represents a non authenticated EmpiriaIdentity.</summary>
    /// <param name="identity">Represents an authenticated Empiria user.</param>
    internal EmpiriaPrincipal(EmpiriaIdentity identity, IClientApplication clientApp,
                              IUserCredentials credentials) {

      Assertion.Require(identity, nameof(identity));
      Assertion.Require(clientApp, nameof(clientApp));
      Assertion.Require(credentials, nameof(credentials));

      if (!identity.IsAuthenticated) {
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }

      this.Initialize(identity, clientApp, credentials);

      _securityObjects = new Lazy<SecurityObjects>(() => new SecurityObjects(this));
    }


    static public IEmpiriaPrincipal Current {
      get {
        return ExecutionServer.CurrentPrincipal;
      }
    }


    static internal EmpiriaPrincipal TryParseWithToken(string sessionToken) {
      EmpiriaPrincipal principal = null;

      if (principalsCache.ContainsKey(sessionToken)) {
        principal = principalsCache[sessionToken];
        principal.RefreshBeforeReturn();
      }

      return principal;
    }


    static internal void CloseAllSessions(Contact user) {
      principalsCache.Remove((x) => x.Identity.User.Contact.Id == user.Id);

      SessionsDataService.CloseAllSessions(user);
    }

    #endregion Constructors and parsers

    #region Properties

    /// <summary>Gets the ClientApplication of the current principal.</summary>
    public IClientApplication ClientApp {
      get;
      private set;
    }


    public AssortedDictionary ContextItems {
      get;
      private set;
    }


    /// <summary>Gets the IIdentity instance of the current principal.</summary>
    public EmpiriaIdentity Identity {
      get;
      private set;
    }


    IEmpiriaIdentity IEmpiriaPrincipal.Identity {
      get {
        return this.Identity;
      }
    }


    IIdentity IPrincipal.Identity {
      get {
        return this.Identity;
      }
    }


    private FixedList<IObjectAccessRule> ObjectAccessRules {
      get {
        return _securityObjects.Value.ObjectAccessRules;
      }
    }


    public FixedList<string> Permissions {
      get {
        return _securityObjects.Value.Permissions;
      }
    }


    private FixedList<string> Roles {
      get {
        return _securityObjects.Value.Roles;
      }
    }


    public IEmpiriaSession Session {
      get;
      private set;
    }

    #endregion Properties

    #region Methods

    public void Logout() {
      try {

        this.Session.Close();

        principalsCache.Remove(this.Session.Token);

      } finally {
        // no-op
      }
    }


    public bool HasDataAccessTo<T>(T entity) where T : IIdentifiable {

      var rules = ObjectAccessRules.FindAll(x => x.TypeName == entity.GetType().Name &&
                                                 x.ObjectsUIDs.Contains(entity.UID));
      if (rules.Count != 0) {
        return true;
      }

      rules = ObjectAccessRules.FindAll(x => x.TypeName == entity.GetType().BaseType.Name &&
                                             x.ObjectsUIDs.Contains(entity.UID));
      if (rules.Count != 0) {
        return true;
      }

      rules = ObjectAccessRules.FindAll(x => x.TypeName == entity.GetType().BaseType.BaseType.Name &&
                                             x.ObjectsUIDs.Contains(entity.UID));
      if (rules.Count != 0) {
        return true;
      }

      rules = ObjectAccessRules.FindAll(x => x.TypeName == entity.GetType().Name &&
                                            !x.ObjectsUIDs.Contains(entity.UID));

      if (rules.Count != 0) {
        return false;
      }

      rules = ObjectAccessRules.FindAll(x => x.TypeName == entity.GetType().BaseType.Name &&
                                            !x.ObjectsUIDs.Contains(entity.UID));

      if (rules.Count != 0) {
        return false;
      }

      rules = ObjectAccessRules.FindAll(x => x.TypeName == entity.GetType().BaseType.BaseType.Name &&
                                            !x.ObjectsUIDs.Contains(entity.UID));

      if (rules.Count != 0) {
        return false;
      }

      // ToDo: Remove this hard-coded rule
      if (entity.UID == "NivelacionCuentasCompraventa") {

        return EmpiriaMath.IsMemberOf(ExecutionServer.CurrentUserId, new[] { 135, 1002, 1003, 2006, 3512, 3548 });
      }

      return true;
    }


    public bool HasPermission(string permissionID) {
      return this.Permissions.Contains(permissionID);
    }


    /// <summary>Determines whether the current principal belongs to the specified role.</summary>
    /// <param name="role">The name of the role for which to check membership.</param>
    /// <returns>true if the current principal is a member of the specified role in the current domain;
    /// otherwise, false.</returns>
    public bool IsInRole(string role) {
      return this.Roles.Contains(role);
    }


    #endregion Methods

    #region Helpers

    private void Initialize(EmpiriaIdentity identity, IClientApplication clientApp,
                            IUserCredentials credentials) {
      Assertion.Require(identity, nameof(identity));
      Assertion.Require(clientApp, nameof(clientApp));
      Assertion.Require(credentials, nameof(credentials));

      this.Identity = identity;

      this.ClientApp = clientApp;

      this.Session = new EmpiriaSession(this, credentials);

      principalsCache.Insert(this.Session.Token, this);

      this.ContextItems = new AssortedDictionary();

      this.RefreshBeforeReturn();
    }


    private void Initialize(EmpiriaIdentity identity, IClientApplication clientApp,
                            IEmpiriaSession session) {
      Assertion.Require(identity, nameof(identity));
      Assertion.Require(session, nameof(session));

      this.Identity = identity;

      this.ClientApp = clientApp;

      this.Session = session;

      principalsCache.Insert(this.Session.Token, this);

      this.ContextItems = new AssortedDictionary();

      this.RefreshBeforeReturn();
    }

    private void RefreshBeforeReturn() {
      this.Session.UpdateEndTime();
    }

    #endregion Helpers


    sealed private class SecurityObjects {

      internal SecurityObjects(EmpiriaPrincipal principal) {

        var service = new Services.AuthorizationService();

        this.Roles = service.GetRoles(principal.Identity, principal.ClientApp);

        this.Permissions = service.GetFeaturesPermissions(principal.Identity, principal.ClientApp);

        this.ObjectAccessRules = service.GetObjectAccessRules(principal.Identity, principal.ClientApp);
      }


      internal FixedList<IObjectAccessRule> ObjectAccessRules {
        get;
      }

      internal FixedList<string> Permissions {
        get;
      }

      internal FixedList<string> Roles {
        get;
      }

    }  // inner class SecurityObjects

  } // class EmpiriaPrincipal

} // namespace Empiria.Security
