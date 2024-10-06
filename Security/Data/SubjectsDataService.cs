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

    static internal SubjectData GetSubject(IIdentifiable contact) {
      string sql = "SELECT * FROM " +
                   "SecurityItems INNER JOIN Contacts " +
                   "ON SecurityItems.SubjectId = Contacts.ContactId " +
                   $"WHERE SecurityItemTypeId = {SecurityItemType.SubjectCredentials.Id} AND " +
                   $"SubjectId = {contact.Id}";

      return DataReader.GetPlainObject<SubjectData>(DataOperation.Parse(sql));
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

      return DataReader.GetPlainObjectFixedList<SubjectData>(DataOperation.Parse(sql));
    }


    static internal SubjectData TryGetSubjectWithUserID(string userID) {
      string sql = "SELECT * FROM " +
                   "SecurityItems INNER JOIN Contacts " +
                   "ON SecurityItems.SubjectId = Contacts.ContactId " +
                  $"WHERE SecurityItemTypeId = {SecurityItemType.SubjectCredentials.Id} AND " +
                  $"LOWER(SecurityItemKey) = '{userID.ToLower()}'";

      return DataReader.GetPlainObject<SubjectData>(DataOperation.Parse(sql), null);
    }


    static internal FixedList<Organization> Workareas() {
      string sql = "SELECT * FROM Contacts " +
                   "WHERE ContactTags LIKE '%systems-users-org%' AND " +
                   "ContactStatus <> 'X' " +
                   "ORDER BY ContactFullName";

      return DataReader.GetFixedList<Organization>(DataOperation.Parse(sql));
    }


    #region Legacy MhParticipants integration

    static internal int TryGetFormerParticipantId(string userID) {
      if (!USES_LEGACY_MH_PARTICIPANTS) {
        return 0;
      }

      var formerUserID = userID.Length <= 24 ?
                         userID : userID.Substring(0, 24);

      string sql = "SELECT ParticipantId " +
                   "FROM MHParticipants " +
                  $"WHERE LOWER(ParticipantKey) = '{formerUserID.ToLower()}'";

      return Convert.ToInt32(DataReader.GetScalar<long>(DataOperation.Parse(sql), 0));
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

    #endregion Legacy MhParticipants integration

  } // class SubjectsDataService

} // namespace Empiria.Security.Data
