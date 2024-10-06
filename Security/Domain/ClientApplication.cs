/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : ClientApplication                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a client application.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DataTypes;

namespace Empiria.Security {

  /// <summary>Represents a client application.</summary>
  internal class ClientApplication : SecurityItem, IClientApplication {

    #region Constructors and parsers

    internal ClientApplication(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }


    static internal new ClientApplication Parse(int id) {
      return BaseObject.ParseId<ClientApplication>(id);
    }


    static public ClientApplication TryParse(string clientAppKey) {
      Assertion.Require(clientAppKey, "clientAppKey");

      return BaseObject.TryParse<ClientApplication>($"SecurityItemKey = '{clientAppKey}'");
    }


    static public ClientApplication ParseActive(string clientAppKey) {
      Assertion.Require(clientAppKey, "clientAppKey");

      var app = BaseObject.ParseKey<ClientApplication>($"SecurityItemKey = '{clientAppKey}'");

      Assertion.Require(app.Status == StateEnums.EntityStatus.Active,
                        "Client application is not active");

      return app;
    }


    public static void AssertIsActive(string appKey) {
      ClientApplication.ParseActive(appKey);
    }


    #endregion Constructors and parsers

    #region Properties

    public string Key {
      get {
        return base.BaseKey;
      }
    }


    public string Name {
      get {
        return ExtensionData.Get("clientAppName", base.BaseKey);
      }
    }


    internal SecurityContext SecurityContext {
      get {
        return SecurityContext.Parse(base.TargetId);
      }
    }


    public FixedList<NameValuePair> WebApiAddresses {
      get {
        return new FixedList<NameValuePair>();
      }
    }


    public string ClaimsToken {
      get {
        return base.UID;
      }
    }

    #endregion Properties

  }  // class ClientApplication

}  // namespace Empiria.Security
