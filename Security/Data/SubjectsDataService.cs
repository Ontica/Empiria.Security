/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management               Component : Data Access Layer                       *
*  Assembly : Empiria.Security.dll                       Pattern   : Data Services                           *
*  Type     : SubjectsDataService                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data read and write methods for user management services.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Data;

using Empiria.Security.Subjects;

namespace Empiria.Security.Data {

  static internal class SubjectsDataService {

    static private readonly bool USES_LEGACY_MH_PARTICIPANTS = ConfigurationData.Get("UsesLegacyMHParticipants", false);
    static private readonly bool USES_PARTIES = ConfigurationData.Get("UsesParties", false);

    static internal SubjectData GetSubject(IIdentifiable contact) {
      SubjectData subjectData = TryGetSubject(contact);


      Assertion.Require(subjectData, $"A subject with Id was not found.");

      return subjectData;
    }


    static internal FixedList<SubjectData> SearchSubjects(string filter) {
      string sql = "SELECT * FROM " +
                   "SecurityItems INNER JOIN Contacts " +
                   "ON SecurityItems.SubjectId = Contacts.ContactId " +
                   $"WHERE SecurityItemTypeId = {SecurityItemType.SubjectCredentials.Id}";


      if (filter.Length != 0) {
        sql += " AND " + filter;
      }

      sql += $" ORDER BY ContactFullName";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<SubjectData>(op);
    }


    static internal SubjectData TryGetSubject(IIdentifiable contact) {

      string sql = "SELECT * FROM " +
                   "SecurityItems INNER JOIN Contacts " +
                   "ON SecurityItems.SubjectId = Contacts.ContactId " +
                   $"WHERE SecurityItemTypeId = {SecurityItemType.SubjectCredentials.Id} AND " +
                   $"SubjectId = {contact.Id}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<SubjectData>(op, null);
    }


    static internal SubjectData TryGetSubjectWithUserID(string userID) {

      Assertion.Require(userID, nameof(userID));

      EmpiriaString.EnsureIsSafe(userID, "Unsafe userID");
      Assertion.Require(!userID.Contains("'"), "The userID must not contain quotes.");

      string sql = "SELECT * FROM " +
                   "SecurityItems INNER JOIN Contacts " +
                   "ON SecurityItems.SubjectId = Contacts.ContactId " +
                  $"WHERE SecurityItemTypeId = {SecurityItemType.SubjectCredentials.Id} AND " +
                  $"LOWER(SecurityItemKey) = '{userID.ToLower()}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<SubjectData>(op, null);
    }


    static internal FixedList<Organization> Workareas() {
      string sql = "SELECT * FROM Contacts " +
                   "WHERE ContactTags LIKE '%systems-users-org%' AND " +
                   "ContactStatus <> 'X' " +
                   "ORDER BY Initials, ContactFullName";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Organization>(op);
    }


    #region Legacy MhParticipants integration

    static internal int TryGetFormerParticipantId(string userID) {

      if (!USES_LEGACY_MH_PARTICIPANTS) {
        return 0;
      }

      var formerUserID = userID.Length <= 24 ?
                         userID : userID.Substring(0, 24);

      EmpiriaString.EnsureIsSafe(formerUserID, "Unsafe formerUserID");
      Assertion.Require(!formerUserID.Contains("'"), "The formerUserID must not contain quotes.");

      string sql = "SELECT ParticipantId " +
                   "FROM MHParticipants " +
                  $"WHERE LOWER(ParticipantKey) = '{formerUserID.ToLower()}'";

      var op = DataOperation.Parse(sql);

      return Convert.ToInt32(DataReader.GetScalar<long>(op, 0));
    }


    static internal void WriteAsParticipant(SubjectData subject) {
      if (!USES_LEGACY_MH_PARTICIPANTS) {
        return;
      }

      var fullname = subject.Contact.FullName.Length <= 64 ?
                          subject.Contact.FullName : subject.Contact.FullName.Substring(0, 64);

      var userID = subject.UserID.Length <= 24 ?
                          subject.UserID : subject.UserID.Substring(0, 24);

      var op = DataOperation.Parse("write_mhparticipant", subject.Contact.Id,
          fullname, userID, subject.Contact.FullName, 'U', 'A',
          new DateTime(2022, 01, 01), new DateTime(2049, 12, 31));

      DataWriter.Execute(op);
    }

    internal static void WriteAsParty() {
      if (!USES_PARTIES) {
        return;
      }

      DataWriter.Execute(DataOperation.Parse("apd_party_tmp", 1));
    }

    #endregion Legacy MhParticipants integration

  } // class SubjectsDataService

} // namespace Empiria.Security.Data
