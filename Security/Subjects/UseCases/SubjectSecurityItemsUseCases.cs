/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Use cases Layer                       *
*  Assembly : Empiria.Security.dll                         Pattern   : Use case interactor                   *
*  Type     : SubjectSecurityItemsUseCases                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Use cases for subject's assigned security items.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Contacts;
using Empiria.Security.Data;

using Empiria.Services;

namespace Empiria.Security.Subjects.UseCases {

  /// <summary>Use cases for subject's assigned security items.</summary>
  public class SubjectSecurityItemsUseCases : UseCase {

    #region Constructors and parsers

    protected SubjectSecurityItemsUseCases() {
      // no-op
    }

    static public SubjectSecurityItemsUseCases UseCaseInteractor() {
      return CreateInstance<SubjectSecurityItemsUseCases>();
    }


    #endregion Constructors and parsers

    #region Use cases

    public void AssignContext(string subjectUID, string contextUID) {
      IIdentifiable subject = GetSubject(subjectUID);
      SecurityContext context = GetContext(contextUID);

      var securityEditor = new SubjectSecurityItemsEditor(subject);

      securityEditor.AssignContext(context);

      var subjectData = GetSubjectData(subjectUID);

      EmpiriaPrincipal.CloseAllSessions(subject);

      EmpiriaLog.PermissionsLog(subjectData.Contact, "Alta de aplicación", context.Name);
    }


    public void AssignFeature(string subjectUID, string contextUID, string featureUID) {
      Assertion.Require(featureUID, nameof(featureUID));

      var feature = Feature.Parse(featureUID);

      SubjectSecurityItemsEditor securityEditor = GetSubjectSecurityItemsEditor(subjectUID, contextUID);

      securityEditor.AssignFeature(feature);

      var subjectData = GetSubjectData(subjectUID);

      EmpiriaPrincipal.CloseAllSessions(subjectData.Contact);

      EmpiriaLog.PermissionsLog(subjectData.Contact, "Alta de permiso", feature.Name);
    }


    public void AssignRole(string subjectUID, string contextUID, string roleUID) {
      Assertion.Require(roleUID, nameof(roleUID));

      var role = Role.Parse(roleUID);

      SubjectSecurityItemsEditor securityEditor = GetSubjectSecurityItemsEditor(subjectUID, contextUID);

      securityEditor.AssignRole(role);

      var subjectData = GetSubjectData(subjectUID);

      EmpiriaPrincipal.CloseAllSessions(subjectData.Contact);

      EmpiriaLog.PermissionsLog(subjectData.Contact, "Alta de rol", role.Name);
    }


    public FixedList<NamedEntityDto> GetSubjectContexts(string subjectUID) {
      IIdentifiable subject = GetSubject(subjectUID);

      FixedList<SecurityContext> contexts = SecurityContext.GetList(subject);

      return contexts.MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetSubjectFeatures(string subjectUID, string contextUID) {
      IIdentifiable subject = GetSubject(subjectUID);
      SecurityContext context = GetContext(contextUID);

      FixedList<Feature> features = Feature.GetSubjectFeatures(subject, context);

      return features.MapToNamedEntityList();
    }



    public FixedList<NamedEntityDto> GetSubjectRoles(string subjectUID, string contextUID) {
      IIdentifiable subject = GetSubject(subjectUID);
      SecurityContext context = GetContext(contextUID);

      FixedList<Role> roles = Role.GetSubjectRoles(subject, context);

      return roles.MapToNamedEntityList();
    }


    public void UnassignContext(string subjectUID, string contextUID) {
      IIdentifiable subject = GetSubject(subjectUID);
      SecurityContext context = GetContext(contextUID);

      SubjectSecurityItemsEditor securityEditor = GetSubjectSecurityItemsEditor(subjectUID, contextUID);

      FixedList<Role> roles = Role.GetSubjectRoles(subject, context);

      foreach (var role in roles) {
        securityEditor.UnassignRole(role);
      }

      FixedList<Feature> features = Feature.GetSubjectFeatures(subject, context);

      foreach (var feature in features) {
        securityEditor.UnassignFeature(feature);
      }

      securityEditor.UnassignContext(context);

      var subjectData = GetSubjectData(subjectUID);

      EmpiriaPrincipal.CloseAllSessions(subject);

      EmpiriaLog.PermissionsLog(subjectData.Contact, "Baja de aplicación", context.Name);
    }


    public void UnassignFeature(string subjectUID, string contextUID, string featureUID) {
      Assertion.Require(featureUID, nameof(featureUID));

      var feature = Feature.Parse(featureUID);

      SubjectSecurityItemsEditor securityEditor = GetSubjectSecurityItemsEditor(subjectUID, contextUID);

      var subject = GetSubjectData(subjectUID);

      securityEditor.UnassignFeature(feature);

      EmpiriaPrincipal.CloseAllSessions(subject.Contact);

      EmpiriaLog.PermissionsLog(subject.Contact, "Baja de permiso", feature.Name);
    }


    public void UnassignRole(string subjectUID, string contextUID, string roleUID) {
      Assertion.Require(roleUID, nameof(roleUID));

      var role = Role.Parse(roleUID);

      SubjectSecurityItemsEditor securityEditor = GetSubjectSecurityItemsEditor(subjectUID, contextUID);

      var subject = GetSubjectData(subjectUID);

      securityEditor.UnassignRole(role);

      EmpiriaPrincipal.CloseAllSessions(subject.Contact);

      EmpiriaLog.PermissionsLog(subject.Contact, "Baja de rol", role.Name);
    }

    #endregion Use cases

    #region Helpers

    private SecurityContext GetContext(string contextUID) {
      Assertion.Require(contextUID, nameof(contextUID));

      return SecurityContext.Parse(contextUID);
    }


    private IIdentifiable GetSubject(string subjectUID) {
      Assertion.Require(subjectUID, nameof(subjectUID));

      return Contact.Parse(subjectUID);
    }

    private SubjectData GetSubjectData(string subjectUID) {
      Assertion.Require(subjectUID, nameof(subjectUID));

      var contact = Contact.Parse(subjectUID);

      return SubjectsDataService.GetSubject(contact);
    }

    private SubjectSecurityItemsEditor GetSubjectSecurityItemsEditor(string subjectUID,
                                                                     string contextUID) {
      IIdentifiable subject = GetSubject(subjectUID);
      SecurityContext context = GetContext(contextUID);

      return new SubjectSecurityItemsEditor(subject, context);
    }

    #endregion Helpers

  }  // class SubjectSecurityItemsUseCases

}  // namespace Empiria.Security.Subjects.UseCases
