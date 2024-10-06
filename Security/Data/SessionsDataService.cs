/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Data access layer                     *
*  Assembly : Empiria.Security.dll                         Pattern   : Data service                          *
*  Type     : SessionsDataService                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Empiria sessions data service.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Data;

namespace Empiria.Security.Data {

  /// <summary>Empiria sessions data service.</summary>
  static internal class SessionsDataService {

    static internal void CloseAllSessions(Contact user) {
      var op = DataOperation.Parse("doCloseAllUserSessions", user.Id, DateTime.Now);

      DataWriter.Execute(op);
    }


    static internal void CloseSession(EmpiriaSession o) {
      var op = DataOperation.Parse("doCloseUserSession", o.Token, o.EndTime);

      DataWriter.Execute(op);
    }


    static internal int CreateSession(EmpiriaSession o) {
      var op = DataOperation.Parse("apdUserSession", o.Token, o.ServerId,
                        o.ClientAppId, o.UserId, o.ExpiresIn,
                        o.RefreshToken, o.UserHostAddress,
                        o.ExtendedData.ToString(), o.StartTime, o.EndTime);

      return DataWriter.Execute<int>(op);
    }


    static internal EmpiriaSession GetSession(string sessionToken) {
      var sql = "SELECT * FROM UserSessions " +
                $"WHERE SessionToken = '{EmpiriaString.Truncate(sessionToken, 255)}'";

      var op = DataOperation.Parse(sql);

      var session = DataReader.GetPlainObject<EmpiriaSession>(op, null);

      if (session == null) {
        var exception = new SecurityException(SecurityException.Msg.SessionTokenNotFound, sessionToken);

        EmpiriaLog.FailedOperationLog("Authentication", exception);

        throw exception;
      }

      return session;
    }

  } // class SessionsDataService

} // namespace Empiria.Security.Data
