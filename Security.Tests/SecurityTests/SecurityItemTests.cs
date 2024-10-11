/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Management                          Component : Test cases                            *
*  Assembly : Empiria.Security.Tests.dll                   Pattern   : Unit tests                            *
*  Type     : SecurityItemTests                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Test cases for security items.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Security;

namespace Empiria.Tests.Security {

  /// <summary>Test cases for security items.</summary>
  public class SecurityItemTests {

    #region Facts

    [Fact]
    public void Should_Read_All_Security_Items() {
      var sut = BaseObject.GetFullList<SecurityItem>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class SecurityItemTests

} // namespace Empiria.Tests.Security
