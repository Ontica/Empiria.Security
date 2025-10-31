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
using Empiria.Security.Data;
using Empiria.Security.Subjects;

namespace Empiria.Tests.Security {

  /// <summary>Test cases for security items.</summary>
  public class SecurityItemTests {

    #region Facts

    [Fact]
    public void Should_Parse_All_Features() {
      var features = BaseObject.GetFullList<Feature>();

      Assert.NotNull(features);

      foreach (var feature in features) {
        Assert.NotNull(feature.Requires);
        Assert.NotNull(feature.ObjectsGrants);
      }
    }


    [Fact]
    public void Should_Parse_All_Roles() {
      var roles = BaseObject.GetFullList<Role>();

      Assert.NotNull(roles);

      foreach (var role in roles) {
        Assert.NotNull(role.Grants);
        Assert.NotNull(role.ObjectsGrants);
        Assert.NotNull(role.Revokes);
      }
    }


    [Fact]
    public void Should_Parse_All_Subjects_Security_Items() {
      FixedList<SubjectData> subjects = SubjectsDataService.SearchSubjects(string.Empty);

      Assert.NotNull(subjects);
      Assert.NotEmpty(subjects);

      foreach (var subject in subjects.Select(x => x.Contact)) {
        var subjectContexts = SecurityContext.GetList(subject);

        foreach (var context in subjectContexts) {
          Assert.NotNull(Role.GetSubjectRoles(subject, context));
          Assert.NotNull(Feature.GetSubjectFeatures(subject, context));
        }
      }
    }


    [Fact]
    public void Should_Read_All_Security_Items() {
      var sut = BaseObject.GetFullList<SecurityItem>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

  }  // class SecurityItemTests

} // namespace Empiria.Tests.Security
