/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization services                *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : ObjectAccessRule                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds information about an object access permission rule.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Security {

  /// <summary>Holds information about an object access permission rule.</summary>
  internal class ObjectAccessRule {

    #region Constructors and parsers

    static internal ObjectAccessRule Parse(JsonObject data) {
      return new ObjectAccessRule {
       TypeName = data.Get<string>("typeName"),
       ObjectsUIDs = data.GetList<string>("uids").ToFixedList()
      };
    }

    #endregion Constructors and parsers

    #region Properties

    internal string TypeName {
      get; private set;
    }

    internal FixedList<string> ObjectsUIDs {
      get; private set;
    }

    #endregion Properties

  }  // class ObjectAccessRule

}  // namespace Empiria.Security
