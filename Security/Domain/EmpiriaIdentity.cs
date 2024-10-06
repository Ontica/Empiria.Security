/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : EmpiriaIdentity                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an authenticated user in Empiria Framework.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Security {

  /// <summary>Describes authentication modes.</summary>
  public enum AuthenticationMode {

    None,

    Basic,

    Forms,

    Realm,

  }  // enum AuthenticationMode



  /// <summary>Represents an authenticated user in Empiria Framework.</summary>
  internal class EmpiriaIdentity : IEmpiriaIdentity {

    private readonly AuthenticationMode _authenticationMode;

    #region Constructors and parsers

    internal EmpiriaIdentity(IEmpiriaUser user, AuthenticationMode mode) {
      Assertion.Require(user, nameof(user));

      this.User = user;
      _authenticationMode = mode;
    }

    #endregion Constructors and parsers

    #region Public properties


    public string AuthenticationType {
      get {
        return _authenticationMode.ToString();
      }
    }


    public bool IsAuthenticated {
      get {
        return _authenticationMode != AuthenticationMode.None;
      }
    }


    public string Name {
      get {
        return this.User.UserName;
      }
    }


    public IEmpiriaUser User {
      get;
    }

    #endregion Public properties

  } // class EmpiriaIdentity

} // namespace Empiria.Security
