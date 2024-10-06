/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : SoftwareSystem                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a software system.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Represents a software system.</summary>
  internal sealed class SoftwareSystem : SecurityItem, INamedEntity {

    internal SoftwareSystem(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new SoftwareSystem Parse(int id) {
      return BaseObject.ParseId<SoftwareSystem>(id);
    }


    public string Name {
      get {
        return ExtensionData.Get("systemName", base.BaseKey);
      }
    }


  } // class SoftwareSystem

} // namespace Empiria.Security
