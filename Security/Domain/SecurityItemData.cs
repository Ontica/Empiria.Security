/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain layer                          *
*  Assembly : Empiria.Security.dll                         Pattern   : Information Holder                    *
*  Type     : SecurityItemData                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds data about a security item.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;
using Empiria.StateEnums;

using Empiria.Security.Data;

namespace Empiria.Security {

  /// <summary>Holds data about a security item.</summary>
  internal class SecurityItemData {

    private SecurityItemData() {
      // Required by Empiria Framework.
    }

    internal SecurityItemData(SecurityItemType itemType,
                              SecurityContext context,
                              IIdentifiable subject,
                              IIdentifiable target) {
      Assertion.Require(itemType, nameof(itemType));
      Assertion.Require(context, nameof(context));
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(target, nameof(target));

      this.SecurityItemType = itemType;
      this.ContextId = context.Id;
      this.SubjectId = subject.Id;
      this.TargetId = target.Id;
    }

    static internal SecurityItemData Parse(SecurityItemType itemType,
                                           SecurityContext context,
                                           IIdentifiable subject,
                                           IIdentifiable target) {
      Assertion.Require(itemType, nameof(itemType));
      Assertion.Require(context, nameof(context));
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(target, nameof(target));

      return SecurityItemsDataReader.ParseSecurityItemData(itemType, context, subject, target);
    }


    internal void AssignIdentificator(int id) {
      Assertion.Require(id > 0, nameof(id));

      if (this.Id != 0 || this.UID.Length != 0) {
        Assertion.RequireFail("Cannot assign object identificators for existing objects.");
      }

      this.Id = id;

      this.UID = Guid.NewGuid()
                     .ToString();

      this.DataIntegrity = Guid.NewGuid()
                               .ToString();
    }


    [DataField("SecurityItemId")]
    internal int Id {
      get; private set;
    }


    [DataField("SecurityItemUID")]
    internal string UID {
      get; private set;
    } = string.Empty;


    [DataField("SecurityItemTypeId")]
    internal SecurityItemType SecurityItemType {
      get; private set;
    }


    [DataField("ContextId")]
    internal int ContextId {
      get; private set;
    }


    [DataField("SubjectId")]
    internal int SubjectId {
      get; private set;
    }


    [DataField("TargetId")]
    internal int TargetId {
      get; private set;
    }


    [DataField("SecurityItemKey")]
    internal string Key {
      get; set;
    } = string.Empty;


    [DataField("SecurityItemExtData")]
    internal JsonObject ExtData {
      get; set;
    } = new JsonObject();


    [DataField("SecurityItemKeywords")]
    internal string Keywords {
      get; set;
    } = string.Empty;


    [DataField("LastUpdate")]
    internal DateTime LastUpdate {
      get; set;
    } = DateTime.Now;


    [DataField("UpdatedById")]
    internal int UpdatedById {
      get; set;
    } = -1;


    [DataField("SecurityItemStatus", Default = EntityStatus.Active)]
    internal EntityStatus Status {
      get; set;
    } = EntityStatus.Active;


    [DataField("SecurityItemDIF")]
    internal string DataIntegrity {
      get; private set;
    } = string.Empty;

  }  // class SecurityItemData

}  // namespace Empiria.Security
