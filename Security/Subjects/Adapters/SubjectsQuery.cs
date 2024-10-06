/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Interface adapters                    *
*  Assembly : Empiria.Security.dll                         Pattern   : Query Data Transfer Object            *
*  Types    : SubjectsQuery                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query input DTO used for security subjects searching.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;

namespace Empiria.Security.Subjects.Adapters {

  /// <summary>Query input DTO used for security subjects searching.</summary>
  public class SubjectsQuery {

    public string ContextUID {
      get; set;
    } = string.Empty;


    public string WorkareaUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;

  }  // class SubjectsQuery



  /// <summary>Extension methods for SearchSubjectsQuery interface adapter.</summary>
  static public class SubjectsQueryExtension {

    #region Methods

    static public string MapToFilterString(this SubjectsQuery query) {
      string contextFilter = BuildContextFilter(query.ContextUID);
      string workareaFilter = BuildWorkareaFilter(query.WorkareaUID);
      string keywordsFilter = BuildKeywordsFilter(query.Keywords);

      var filter = new Filter(contextFilter);

      filter.AppendAnd(workareaFilter);
      filter.AppendAnd(keywordsFilter);

      return filter.ToString();
    }

    #endregion Methods


    #region Helpers

    static private string BuildContextFilter(string contextUID) {
      if (contextUID.Length == 0) {
        return string.Empty;
      }

      var context = SecurityContext.Parse(contextUID);

      return $"SubjectId IN (" +
             $"SELECT SubjectId FROM SecurityItems " +
             $"WHERE SecurityItemTypeId = {SecurityItemType.SubjectContext.Id} AND " +
             $"TargetId = {context.Id} AND " +
             $"SecurityItemStatus <> 'X')";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords.Length == 0) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLikeKeywords("contactkeywords", keywords);
     }


    static private string BuildWorkareaFilter(string workareaUID) {
      if (workareaUID.Length == 0) {
        return string.Empty;
      }

      return $"OrganizationId = {Organization.Parse(workareaUID).Id}";
    }

    #endregion Helpers

  }  // class SubjectsQueryExtension

}  // namespace Empiria.Security.Subjects.Adapters
