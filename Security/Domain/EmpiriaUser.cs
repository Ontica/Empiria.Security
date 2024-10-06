/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : EmpiriaUser                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a system's user.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.StateEnums;

namespace Empiria.Security {

  /// <summary>Represents a system's user.</summary>
  internal sealed class EmpiriaUser : IEmpiriaUser {

    private int PASSWORD_EXPIRATION_DAYS = ConfigurationData.Get("PasswordExpirationDays", 45);

    #region Constructors and parsers

    private EmpiriaUser() {
      // Required by Empiria Framework
    }


    static internal EmpiriaUser Parse(Claim userData) {
      Assertion.Require(userData, nameof(userData));

      return new EmpiriaUser {
        Contact = Contact.Parse(userData.SubjectId),
        UserName = userData.Key,
        Status = userData.Status,
        IsAuthenticated = false,
        MustChangePassword = userData.GetAttribute(ClaimAttributeNames.MustChangePassword, false),
        PasswordUpdatedDate = userData.GetAttribute(ClaimAttributeNames.PasswordUpdatedDate, DateTime.Today),
        PasswordNeverExpires = userData.GetAttribute(ClaimAttributeNames.PasswordNeverExpires, false)
      };
    }


    #endregion Constructors and parsers

    #region Authenticate methods

    static internal IEmpiriaUser Authenticate(Claim userData) {
      Assertion.Require(userData, nameof(userData));

      var user = EmpiriaUser.Parse(userData);

      user.EnsureCanAuthenticate();

      user.IsAuthenticated = true;

      return user;
    }

    #endregion Authenticate methods

    #region Public propertiese

    public bool IsActive {
      get {
        return this.Status == EntityStatus.Active;
      }
    }

    public bool IsAuthenticated {
      get;
      private set;
    }


    public bool MustChangePassword {
      get;
      private set;
    }


    public bool PasswordExpired {
      get {
        return PasswordNeverExpires == false && PasswordUpdatedDate.AddDays(PASSWORD_EXPIRATION_DAYS) < DateTime.Now;
      }
    }

    private bool PasswordNeverExpires {
      get;
      set;
    }

    public DateTime PasswordUpdatedDate {
      get;
      private set;
    }


    public string UserName {
      get;
      private set;
    }


    public string FullName {
      get {
        return this.Contact.FullName;
      }
    }


    public string EMail {
      get {
        return this.Contact.EMail;
      }
    }


    public Contact Contact {
      get; private set;
    }


    private EntityStatus Status {
      get; set;
    }


    #endregion Public properties

    #region Private methods

    private void EnsureCanAuthenticate() {
      if (!this.IsActive) {
        var securityException = new SecurityException(SecurityException.Msg.UserAccountIsSuspended, this.UserName);

        EmpiriaLog.FailedOperationLog(this.Contact, "Authentication", securityException.Message);

        throw securityException;
      }

      if (this.PasswordExpired) {
        var securityException = new SecurityException(SecurityException.Msg.UserPasswordExpired, this.UserName);

        EmpiriaLog.FailedOperationLog(this.Contact, "Authentication", securityException.Message);

        throw securityException;
      }

      if (this.MustChangePassword) {
        var securityException = new SecurityException(SecurityException.Msg.MustChangePassword, this.UserName);

        EmpiriaLog.FailedOperationLog(this.Contact, "Authentication", securityException.Message);

        throw securityException;
      }


    }

    #endregion Private methods

  } // class EmpiriaUser

} // namespace Empiria.Security
