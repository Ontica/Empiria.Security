/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Interface adapters                    *
*  Assembly : Empiria.Security.dll                         Pattern   : Mapper class                          *
*  Type     : FeatureDto, ObjectAccessRuleDto              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTOs used to describe security items.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Security.Providers;

namespace Empiria.Security.SecurityItems.Adapters {

  /// <summary>Output DTO that describes features.</summary>
  public class FeatureDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Group {
      get; internal set;
    }

  }  // class FeatureDto



  /// <summary>Output DTO that describes an object access rule.</summary>
  internal class ObjectAccessRuleDto : IObjectAccessRule {

    public string TypeName {
      get; internal set;
    }

    public string[] ObjectsUIDs {
      get; internal set;
    }

  }  // class ObjectAccessRuleDto

}  // namespace Empiria.Security.Subjects.Adapters
