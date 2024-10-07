/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                    Component : Interface adapters                      *
*  Assembly : Empiria.Security.dll                       Pattern   : Mapper class                            *
*  Type     : PrincipalMapper                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for principal instances.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Security.UseCases {

  /// <summary>Mapping methods for principal instances.</summary>
  static internal class PrincipalMapper {

    internal static PrincipalDto Map(IEmpiriaPrincipal principal) {
      return new PrincipalDto {
        Identity = MapIdentity(principal.Identity),
        Permissions = principal.Permissions
      };
    }

    #region Helper methods

    static private IdentityDto MapIdentity(IEmpiriaIdentity identity) {
      return new IdentityDto {
        Name = identity.Name
      };
    }

    #endregion Helper methods

  }  // class PrincipalMapper

}  // namespace Empiria.Security.UseCases
