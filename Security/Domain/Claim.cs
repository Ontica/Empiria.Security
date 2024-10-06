/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication services               *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : Claim                                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Contains attributes that identifies a subject, like a userID or password.                      *
*             Subjects are users, applications, systems, services or computers.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Security.Data;

namespace Empiria.Security {

  /// <summary>Contains attributes that identifies a subject, like a userID or password.
  /// Subjects are users, applications, systems, services or computers.</summary>
  internal class Claim : SecurityItem {

    #region Constructors and parsers

    internal Claim(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }


    static internal new Claim Parse(int id) {
      return BaseObject.ParseId<Claim>(id);
    }


    static internal Claim TryParse(SecurityItemType claimType, int subjectId) {

      return SecurityItemsDataReader.TryGetSubjectItemWithId<Claim>(SecurityContext.Empty, claimType,
                                                                    subjectId);
    }


    static internal Claim TryParseWithKey(SecurityItemType claimType, string securityKey) {

      return SecurityItemsDataReader.TryGetSubjectItemWithKey<Claim>(SecurityContext.Empty, claimType,
                                                                     securityKey);
    }


    #endregion Constructors and parsers

    #region Properties

    public string Key {
      get {
        return base.BaseKey;
      }
    }


    public int SubjectId {
      get {
        return base.BaseSubjectId;
      }
    }


    internal T GetAttribute<T>(string attributeName) {
      Assertion.Require(attributeName, nameof(attributeName));

      return base.ExtensionData.Get<T>(attributeName);
    }


    internal T GetAttribute<T>(string attributeName, T defaultValue) {
      Assertion.Require(attributeName, nameof(attributeName));

      return base.ExtensionData.Get<T>(attributeName, defaultValue);
    }


    internal bool HasAttribute(string attributeName) {
      Assertion.Require(attributeName, nameof(attributeName));

      return base.ExtensionData.Contains(attributeName);
    }

    #endregion Properties

  }  // class Claim

}  // namespace Empiria.Security
