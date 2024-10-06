/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Interface adapters                    *
*  Assembly : Empiria.Security.dll                         Pattern   : Output Data Transfer Object           *
*  Types    : SubjectDto                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output data transfer object for subject information.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Subjects.Adapters {

  /// <summary>Output data transfer object for subject information.</summary>
  public class SubjectDto {

    public string UID {
      get; internal set;
    }

    public string FullName {
      get; internal set;
    }

    public string EmployeeNo {
      get; internal set;
    }

    public string JobPosition {
      get; internal set;
    }

    public NamedEntityDto Workarea {
      get; internal set;
    }

    public string UserID {
      get; internal set;
    }

    public string EMail {
      get; internal set;
    }

    public DateTime CredentialsLastUpdate {
      get; internal set;
    }

    public DateTime LastAccess {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class SubjectDto

}  // namespace Empiria.Security.Subjects.Adapters
