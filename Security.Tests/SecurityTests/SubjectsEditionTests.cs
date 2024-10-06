/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Management                          Component : Test cases                            *
*  Assembly : Empiria.Security.Tests.dll                   Pattern   : Services tests                        *
*  Type     : SubjectsEditionTests                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Test cases for security subjects edition.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.StateEnums;

using Empiria.Security.Subjects.UseCases;
using Empiria.Security.Subjects.Adapters;

namespace Empiria.Tests.Security {

  /// <summary>Test cases for security subjects edition.</summary>
  public class SubjectsEditionTests {

    private readonly SubjectUseCases _usecases;

    #region Initialization

    public SubjectsEditionTests() {
      TestsCommonMethods.Authenticate();

      _usecases = SubjectUseCases.UseCaseInteractor();

    }

    ~SubjectsEditionTests() {
      _usecases.Dispose();
    }

    #endregion Initialization

    #region Facts

    [Fact]
    public void Should_Active_A_Subject() {

      SubjectDto sut = _usecases.ActivateSubject(TestingConstants.SUBJECT_UID);

      Assert.NotNull(sut);
      Assert.True(sut.Status.UID == EntityStatus.Active.ToString());
    }


    [Fact]
    public void Should_Suspend_A_Subject() {

      SubjectDto sut = _usecases.SuspendSubject(TestingConstants.SUBJECT_UID);

      Assert.NotNull(sut);
      Assert.True(sut.Status.UID == EntityStatus.Suspended.ToString());
    }

    #endregion Facts

  }  // class SubjectsEditionTests

} // namespace Empiria.Tests.Security
