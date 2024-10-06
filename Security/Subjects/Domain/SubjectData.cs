/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Domain Layer                          *
*  Assembly : Empiria.Security.dll                         Pattern   : Information Holder                    *
*  Type     : SubjectData                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds subject's data.                                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.StateEnums;

using Empiria.Contacts;

namespace Empiria.Security.Subjects {

  /// <summary>Holds subject's data.</summary>
  internal class SubjectData {

    [DataField("ContactId")]
    internal Contact Contact {
      get; private set;
    }


    [DataField("ContactUID")]
    public string UID {
      get; private set;
    }


    public string FullName {
      get {
        return Contact.FullName;
      }
    }

    public string EmployeeNo {
      get {
        if (Contact is Person p) {
          return p.EmployeeNo;
        }

        return string.Empty;
      }
    }


    public string JobPosition {
      get {
        if (Contact is Person p) {
          return p.JobPosition;
        }

        return string.Empty;
      }
    }


    public Organization Workarea {
      get {
        return Contact.Organization;
      }
    }


    [DataField("SecurityItemKey")]
    public string UserID {
      get; private set;
    }


    public string EMail {
      get {
        return Contact.EMail;
      }
    }


    [DataField("LastUpdate")]
    public DateTime CredentialsLastUpdate {
      get; private set;
    }


    [DataField("LastUpdate")]
    public DateTime LastAccess {
      get; private set;
    }


    [DataField("SecurityItemStatus", Default = EntityStatus.Suspended)]
    public EntityStatus Status {
      get; private set;
    }

  }  // class SubjectData

} // namespace Empiria.Security.Subjects
