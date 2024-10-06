/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Services                            Component : Test cases                            *
*  Assembly : Empiria.Security.Tests                       Pattern   : Testing constants                     *
*  Type     : TestingConstants                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides testing constants for Empiria Security Services.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Tests.Security {

  /// <summary>Provides testing constants for Empiria Security Services.</summary>
  static public class TestingConstants {

    static internal string SUBJECT_UID => ConfigurationData.GetString("SUBJECT_UID");

  }  // class TestingConstants

}  // namespace Empiria.Tests.Security
