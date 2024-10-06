/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                      Component : Services Layer                        *
*  Assembly : Empiria.Security.dll                         Pattern   : Service provider                      *
*  Type     : AuthenticationAttemptsRegister               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Keeps a registry of user authentication attempts.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.Security.Services {

  /// <summary>Keeps a registry of user authentication attempts.</summary>
  internal class AuthenticationAttemptsRegister {

    static private int MAX_AUTHENTICATION_ATTEMPTS =
                                    ConfigurationData.Get<int>("MaxAuthenticationAttempts", 5);

    static private Dictionary<string, int> autenticationAttempts = new Dictionary<string, int>();

    static internal void Add(string userID) {
      if (autenticationAttempts.ContainsKey(userID)) {
        autenticationAttempts[userID] = autenticationAttempts[userID] + 1;
      } else {
        autenticationAttempts.Add(userID, 1);
      }
    }


    static internal bool MaxAttemptsReached(string userID) {
      if (autenticationAttempts.ContainsKey(userID)) {
        return autenticationAttempts[userID] >= MAX_AUTHENTICATION_ATTEMPTS;
      }
      return false;
    }


    static internal void Remove(string userID) {
      autenticationAttempts.Remove(userID);
    }

  }  // class AuthenticationAttemptsRegister

}  // namespace Empiria.Security.Services
