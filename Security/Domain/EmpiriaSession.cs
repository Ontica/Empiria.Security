/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : EmpiriaSession                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : This class represents an Empiria system user session.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;

using Empiria.Security.Data;

namespace Empiria.Security {

  /// <summary>This class represents an Empiria system user session.</summary>
  internal sealed class EmpiriaSession : IIdentifiable, IEmpiriaSession {

    #region Constructors and parsers

    private EmpiriaSession() {
      // Required by Empiria Framework.
    }


    internal EmpiriaSession(IEmpiriaPrincipal principal, IUserCredentials credentials) {
      Assertion.Require(principal, nameof(principal));
      Assertion.Require(credentials, nameof(credentials));

      this.ServerId = ExecutionServer.ServerId;

      this.ClientAppId = principal.ClientApp.Id;

      this.UserId = principal.Identity.User.Contact.Id;

      this.UserHostAddress = credentials.UserHostAddress;

      this.ExtendedData = credentials.ContextData;

      this.Token = this.CreateToken();

      this.Save();
    }


    static internal IEmpiriaSession ParseActive(string sessionToken) {

      EmpiriaSession session = SessionsDataService.GetSession(sessionToken);

      if (session.IsStillActive) {
        return session;
      } else {
        EmpiriaLog.FailedOperationLog(session, "Authentication", "La sesión de trabajo ya expiró.");

        throw new SecurityException(SecurityException.Msg.ExpiredSessionToken);
      }
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("SessionId", ConvertFrom = typeof(long))]
    public int Id {
      get;
      private set;
    }


    public string UID {
      get {
        return this.Token;
      }
    }


    [DataField("SessionToken")]
    public string Token {
      get;
      private set;
    } = string.Empty;


    public string TokenType {
      get {
        return "bearer";
      }
    }


    [DataField("UserId")]
    public int UserId {
      get;
      private set;
    }


    [DataField("ClientAppId")]
    public int ClientAppId {
      get;
      private set;
    } = -1;


    [DataField("ServerId")]
    public int ServerId {
      get;
      private set;
    }


    [DataField("UserHostAddress")]
    public string UserHostAddress {
      get;
      private set;
    } = string.Empty;


    [DataField("ExpiresIn")]
    public int ExpiresIn {
      get; private set;
    } = 3600;


    [DataField("RefreshToken")]
    public string RefreshToken {
      get;
      private set;
    } = string.Empty;


    [DataField("StartTime")]
    public DateTime StartTime {
      get;
      private set;
    } = DateTime.Now;


    [DataField("EndTime")]
    public DateTime EndTime {
      get;
      private set;
    } = ExecutionServer.DateMaxValue;


    public bool IsStillActive {
      get {
        return EndTime == ExecutionServer.DateMaxValue;
      }
    }


    [DataField("SessionExtData")]
    public JsonObject ExtendedData {
      get;
      private set;
    } = new JsonObject();


    #endregion Properties

    #region Methods

    public void Close() {
      SessionsDataService.CloseSession(this);
    }


    public void UpdateEndTime() {
      this.EndTime = DateTime.Now;
    }

    #endregion Methods

    #region Helpers

    private string CreateToken() {
      string token = $"|{UserId}|{ServerId}|{StartTime.ToString("yyyy-MM-dd_HH:mm:ss")}|{ExtendedData}|";

      return Cryptographer.CreateHashCode(token) + Guid.NewGuid().ToString();
    }


    private void Save() {
      this.RefreshToken = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

      this.Id = SessionsDataService.CreateSession(this);
    }

    #endregion Helpers

  } //class EmpiriaSession

} //namespace Empiria.Security
