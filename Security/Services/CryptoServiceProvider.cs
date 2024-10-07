/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization services                *
*  Assembly : Empiria.Security.dll                         Pattern   : Service provider                      *
*  Type     : CryptoServiceProvider                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides subject's authorization services.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Security;
using System.Security.Cryptography;

using Empiria.Security.Providers;

namespace Empiria.Security.Services {

  /// <summary>Provides subject's authorization services.</summary>
  internal class CryptoServiceProvider : ICryptoServiceProvider {

    public RSACryptoServiceProvider GetRSAProvider(string privateKeyFilePath,
                                                     SecureString password) {

      Assertion.Require(privateKeyFilePath, nameof(privateKeyFilePath));
      Assertion.Require(password, nameof(password));

      return RSAProvider.GetProvider(privateKeyFilePath, password);
    }


    public RSACryptoServiceProvider GetRSASystemProvider() {
      return RSAProvider.GetSystemProvider();
    }

  }  // class CryptoServiceProvider

}  // namespace Empiria.Security.Services
