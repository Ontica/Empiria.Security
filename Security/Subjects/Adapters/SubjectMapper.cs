/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Interface adapters                    *
*  Assembly : Empiria.Security.dll                         Pattern   : Mapper class                          *
*  Type     : SubjectMapper                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Mapper for subject instances.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.StateEnums;

namespace Empiria.Security.Subjects.Adapters {

  /// <summary>Mapper for subject instances.</summary>
  static internal class SubjectMapper {

    static internal FixedList<SubjectDto> Map(FixedList<SubjectData> subjects) {
      return subjects.Select(x => Map(x))
                     .ToFixedList();
    }


    static internal SubjectDto Map(SubjectData subject) {
      return new SubjectDto {
        UID = subject.UID,
        UserID = subject.UserID,
        FullName = subject.FullName,
        EMail = subject.EMail,
        EmployeeNo = subject.EmployeeNo,
        JobPosition = subject.JobPosition,
        LastAccess = subject.LastAccess,
        Workarea = subject.Workarea.MapToNamedEntity(),
        CredentialsLastUpdate = subject.CredentialsLastUpdate,
        Status = new NamedEntityDto(subject.Status.ToString(), subject.Status.GetName()),
      };
    }

  }  // class SubjectMapper

}  // namespace Empiria.Security.Subjects.Adapters
