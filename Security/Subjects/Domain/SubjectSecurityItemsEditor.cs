/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Domain Layer                          *
*  Assembly : Empiria.Security.dll                         Pattern   : Service provider                      *
*  Type     : SubjectSecurityItemsEditor                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides services for subject's security items edition.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.StateEnums;

using Empiria.Security.Data;
using Empiria.Security.Subjects.Adapters;

namespace Empiria.Security.Subjects {

  /// <summary>Provides services for subject's security items edition.</summary>
  internal class SubjectSecurityItemsEditor {

    private readonly IIdentifiable _subject;
    private readonly SecurityContext _context;

    #region Constructors and parsers

    public SubjectSecurityItemsEditor(IIdentifiable subject) {
      Assertion.Require(subject, nameof(subject));

      _subject = subject;
      _context = SecurityContext.Empty;
    }


    public SubjectSecurityItemsEditor(IIdentifiable subject, SecurityContext context) {
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(context, nameof(context));

      _subject = subject;
      _context = context;
    }

    #endregion Constructors and parsers

    internal void ActivateSubjectCredentials() {
      var data = SecurityItemData.Parse(SecurityItemType.SubjectCredentials,
                                        SecurityContext.Empty, _subject,
                                        SecurityItem.Empty);

      Assertion.Require(data.Status == EntityStatus.Suspended,
                        $"User account can't be activated. Its current status is {data.Status}.");

      data.Status = EntityStatus.Active;

      SecurityItemsDataWriter.UpdateSecurityItem(data);
    }


    internal void AssignContext(SecurityContext context) {
      Assertion.Require(context, nameof(context));

      var data = new SecurityItemData(SecurityItemType.SubjectContext,
                                      SecurityContext.Empty, _subject, context);

      SecurityItemsDataWriter.CreateSecurityItem(data);
    }


    internal void AssignFeature(Feature feature) {
      Assertion.Require(feature, nameof(feature));

      var data = new SecurityItemData(SecurityItemType.SubjectFeature,
                                      _context, _subject, feature);

      SecurityItemsDataWriter.CreateSecurityItem(data);
    }


    internal void AssignRole(Role role) {
      Assertion.Require(role, nameof(role));

      var data = new SecurityItemData(SecurityItemType.SubjectRole,
                                      _context, _subject, role);

      SecurityItemsDataWriter.CreateSecurityItem(data);
    }


    internal void CreateSubjectCredentials(SubjectFields fields) {
      Assertion.Require(fields, nameof(fields));

      var data = new SecurityItemData(SecurityItemType.SubjectCredentials,
                                      SecurityContext.Empty, _subject,
                                      SecurityItem.Empty);

      data.Key = fields.UserID;

      data.ExtData.Set(ClaimAttributeNames.ContactName, fields.FullName);
      data.ExtData.Set(ClaimAttributeNames.Password, string.Empty);

      data.Keywords = EmpiriaString.BuildKeywords(fields.FullName, fields.EmployeeNo,
                                                  fields.JobPosition, fields.EMail,
                                                  fields.GetWorkarea().FullName);

      SecurityItemsDataWriter.CreateSecurityItem(data);
    }


    internal EntityStatus GetCredentialsStatus() {
      var data = SecurityItemData.Parse(SecurityItemType.SubjectCredentials,
                                        SecurityContext.Empty, _subject,
                                        SecurityItem.Empty);

      return data.Status;
    }


    internal void MarkCredentialsAsExpired() {
      var data = SecurityItemData.Parse(SecurityItemType.SubjectCredentials,
                                        SecurityContext.Empty, _subject,
                                        SecurityItem.Empty);

      data.ExtData.Set(ClaimAttributeNames.MustChangePassword, true);

      SecurityItemsDataWriter.UpdateSecurityItem(data);
    }


    internal void RemoveSubjectCredentials() {
      var data = SecurityItemData.Parse(SecurityItemType.SubjectCredentials,
                                        SecurityContext.Empty, _subject,
                                        SecurityItem.Empty);

      data.ExtData.Set(ClaimAttributeNames.Password, string.Empty);

      SecurityItemsDataWriter.RemoveSecurityItem(data);
    }


    internal void SuspendSubjectCredentials() {
      var data = SecurityItemData.Parse(SecurityItemType.SubjectCredentials,
                                        SecurityContext.Empty, _subject,
                                        SecurityItem.Empty);

      Assertion.Require(data.Status == EntityStatus.Active,
                        $"User account can't be suspended. Its current status is {data.Status}.");

      data.Status = EntityStatus.Suspended;

      SecurityItemsDataWriter.UpdateSecurityItem(data);
    }


    internal void UnassignContext(SecurityContext context) {
      Assertion.Require(context, nameof(context));

      var data = SecurityItemData.Parse(SecurityItemType.SubjectContext,
                                        SecurityContext.Empty,
                                        _subject, target: context);

      SecurityItemsDataWriter.RemoveSecurityItem(data);
    }


    internal void UnassignFeature(Feature feature) {
      Assertion.Require(feature, nameof(feature));

      var data = SecurityItemData.Parse(SecurityItemType.SubjectFeature,
                                        _context, _subject, feature);

      SecurityItemsDataWriter.RemoveSecurityItem(data);
    }


    internal void UnassignRole(Role role) {
      Assertion.Require(role, nameof(role));

      var data = SecurityItemData.Parse(SecurityItemType.SubjectRole,
                                        _context, _subject, role);

      SecurityItemsDataWriter.RemoveSecurityItem(data);
    }


    internal void UpdateSubjectCredentials(string userPassword, bool mustChangePassword) {

      var data = SecurityItemData.Parse(SecurityItemType.SubjectCredentials,
                                        SecurityContext.Empty, _subject,
                                        SecurityItem.Empty);

      data.ExtData.Set(ClaimAttributeNames.Password, userPassword);
      data.ExtData.Set(ClaimAttributeNames.PasswordUpdatedDate, DateTime.Now);

      if (data.ExtData.Get<bool>(ClaimAttributeNames.PasswordNeverExpires, false)) {
        data.ExtData.Set(ClaimAttributeNames.MustChangePassword, false);
      } else {
        data.ExtData.Set(ClaimAttributeNames.MustChangePassword, mustChangePassword);
      }

      SecurityItemsDataWriter.UpdateSecurityItem(data);
    }

  }  // class SubjectSecurityItemsEditor

}  // namespace Empiria.Security.Subjects
