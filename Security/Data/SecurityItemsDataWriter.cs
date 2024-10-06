/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Data access layer                     *
*  Assembly : Empiria.Security.dll                         Pattern   : Write data service                    *
*  Type     : SecurityItemsDataWriter                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Stores security items data.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;
using Empiria.StateEnums;

namespace Empiria.Security.Data {

  /// <summary>Stores security items data.</summary>
  static internal class SecurityItemsDataWriter {

    #region Methods

    static internal void CreateSecurityItem(SecurityItemData o) {
      o.AssignIdentificator(GetNextSecurityItemId());

      WriteSecurityItem(o);
    }


    static internal void RemoveSecurityItem(SecurityItemData o) {
      o.Status = EntityStatus.Deleted;

      WriteSecurityItem(o);
    }

    internal static void UpdateSecurityItem(SecurityItemData o) {
      WriteSecurityItem(o);
    }

    #endregion Methods

    #region Helpers

    static int GetNextSecurityItemId() {
      return DataWriter.CreateId("SecurityItems");
    }

    static void WriteSecurityItem(SecurityItemData o) {
      var op = DataOperation.Parse("writeSecurityItem", o.Id, o.UID,
                                   o.SecurityItemType.Id,
                                   o.ContextId, o.SubjectId, o.TargetId,
                                   o.Key, o.ExtData.ToString(), o.Keywords,
                                   o.LastUpdate, o.UpdatedById,
                                   (char) o.Status, o.DataIntegrity);

      DataWriter.Execute(op);
    }

    #endregion Helpers

  }  // class SecurityItemsDataWriter

}  // namespace Empiria.Security.Data
