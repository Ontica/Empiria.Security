/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Use cases Layer                       *
*  Assembly : Empiria.Security.dll                         Pattern   : Use case interactor                   *
*  Type     : SecurityContextsUseCases                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Use cases that retrieve security contexts information.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Services;

using Empiria.Security.SecurityItems.Adapters;

namespace Empiria.Security.SecurityItems.UseCases {

  /// <summary>Use cases that retrieve security contexts information.</summary>
  public class SecurityContextsUseCases : UseCase {

    #region Constructors and parsers

    protected SecurityContextsUseCases() {
      // no-op
    }

    static public SecurityContextsUseCases UseCaseInteractor() {
      return CreateInstance<SecurityContextsUseCases>();
    }


    #endregion Constructors and parsers

    #region Use cases

    public FixedList<NamedEntityDto> SecurityContexts() {
      var contexts = SecurityContext.GetList();

      return contexts.MapToNamedEntityList();
    }


    public FixedList<FeatureDto> SecurityContextFeatures(string contextUID) {
      Assertion.Require(contextUID, nameof(contextUID));

      var context = SecurityContext.Parse(contextUID);

      var features = Feature.GetList(context.SoftwareSystem);

      features = features.FindAll(x => x.IsAssignable);

      return SecurityItemsMapper.Map(features);
    }


    public FixedList<NamedEntityDto> SoftwareSystemRoles(string contextUID) {
      Assertion.Require(contextUID, nameof(contextUID));

      var context = SecurityContext.Parse(contextUID);

      return Role.GetList(context.SoftwareSystem)
                 .MapToNamedEntityList();
    }


    #endregion Helpers

  }  // class SecurityContextsUseCases

}  // namespace Empiria.Security.SecurityItems.UseCases
