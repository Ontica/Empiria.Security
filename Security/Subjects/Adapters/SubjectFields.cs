/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Interface adapters                    *
*  Assembly : Empiria.Security.dll                         Pattern   : Input Data Transfer Object            *
*  Types    : SubjectFields                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input data transfer object for subject information.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Contacts;

namespace Empiria.Security.Subjects.Adapters {

  /// <summary>Input data transfer object for subject information.</summary>
  public class SubjectFields {

    public string FullName {
      get; set;
    } = string.Empty;


    public string UserID {
      get; set;
    } = string.Empty;


    public string EMail {
      get; set;
    } = string.Empty;


    public string WorkareaUID {
      get; set;
    } = string.Empty;


    public string JobPosition {
      get; set;
    } = string.Empty;


    public string EmployeeNo {
      get; set;
    } = string.Empty;


    internal void EnsureValid() {
      Assertion.Require(this.FullName, "fields.FullName");
      Assertion.Require(this.UserID, "fields.UserID");
      Assertion.Require(this.EMail, "fields.EMail");
    }


    internal Organization GetWorkarea() {
      return Organization.Parse(this.WorkareaUID);
    }

  }  // class SubjectFields

}  // namespace Empiria.Security.Subjects.Adapters
