/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                      Component : Interface adapters                    *
*  Assembly : Empiria.Security.dll                         Pattern   : Data Transfer Object                  *
*  Type     : PrincipalDto                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO with data representing an authenticated user or principal.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Security.UseCases {

  /// <summary>Output DTO with data representing an authenticated user or principal.</summary>
  public class PrincipalDto {

    internal PrincipalDto() {
      // no-op
    }


    public IdentityDto Identity {
      get; internal set;
    }


    public FixedList<string> Permissions {
      get; internal set;
    }

  }  // class PrincipalDto



  public class IdentityDto {

    internal IdentityDto() {
      // no-op
    }

    public string Name {
      get; internal set;
    }

  }  // class IdentityDto

}  // namespace Empiria.Security.UseCases
