/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Interface adapters                    *
*  Assembly : Empiria.Security.dll                         Pattern   : Mapper class                          *
*  Type     : SecurityItemsMapper                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Methods used to map security items.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.SecurityItems.Adapters {

  static internal class SecurityItemsMapper {

    internal static FixedList<FeatureDto> Map(FixedList<Feature> features) {
      return features.Select(x => Map(x))
                     .ToFixedList();
    }


    static private FeatureDto Map(Feature feature) {
      return new FeatureDto {
        UID = feature.UID,
        Name = feature.Name,
        Description = feature.Description,
        Group = feature.Group
      };
    }

  }  // class SecurityItemsMapper


}  // namespace Empiria.Security.Subjects.Adapters
