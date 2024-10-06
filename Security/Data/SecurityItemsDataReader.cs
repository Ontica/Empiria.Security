/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Data access layer                     *
*  Assembly : Empiria.Security.dll                         Pattern   : Read only data service                *
*  Type     : SecurityItemsDataReader                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Reads security items data.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Security.Data {

  /// <summary>Reads security items data.</summary>
  static internal class SecurityItemsDataReader {

    static internal FixedList<T> GetContextItems<T>(SecurityContext context,
                                                    SecurityItemType itemType) where T : SecurityItem {
      Assertion.Require(context, nameof(context));
      Assertion.Require(itemType, nameof(itemType));

      string sql = $"SELECT * FROM SecurityItems " +
                   $"WHERE ContextId = {context.Id} AND " +
                   $"SecurityItemTypeId = {itemType.Id} AND " +
                   $"SecurityItemStatus = 'A'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<T>(op);
    }


    static internal FixedList<T> GetSecurityItems<T>(SecurityItemType itemType) where T : SecurityItem {
      Assertion.Require(itemType, nameof(itemType));

      string sql = $"SELECT * FROM SecurityItems " +
                   $"WHERE SecurityItemTypeId = {itemType.Id} AND " +
                   $"SecurityItemStatus <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<T>(op);
    }


    static internal FixedList<T> GetSoftwareSystemItems<T>(SoftwareSystem softwareSystem,
                                                           SecurityItemType itemType) where T : SecurityItem {
      Assertion.Require(softwareSystem, nameof(softwareSystem));
      Assertion.Require(itemType, nameof(itemType));

      string sql = $"SELECT * FROM SecurityItems " +
                   $"WHERE SecurityItemTypeId = {itemType.Id} AND " +
                   $"TargetId = {softwareSystem.Id} AND " +
                   $"SecurityItemStatus <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<T>(op);
    }


    static internal FixedList<T> GetSubjectTargetItems<T>(IIdentifiable subject,
                                                          SecurityContext context,
                                                          SecurityItemType itemType) where T : SecurityItem {
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(context, nameof(context));
      Assertion.Require(itemType, nameof(itemType));

      string sql = $"SELECT TargetId FROM SecurityItems " +
                   $"WHERE SubjectId = {subject.Id} AND " +
                   $"ContextId = {context.Id} AND " +
                   $"SecurityItemTypeId = {itemType.Id} AND " +
                   $"SecurityItemStatus = 'A'";

      var op = DataOperation.Parse(sql);

      var targets = DataReader.GetFieldValues<int>(op);

      return targets.ToFixedList()
                    .Select(targetId => SecurityItem.Parse<T>(targetId))
                    .ToFixedList();
    }


    static internal SecurityItemData ParseSecurityItemData(SecurityItemType itemType,
                                                           SecurityContext context,
                                                           IIdentifiable subject,
                                                           IIdentifiable target) {
      string sql = $"SELECT * FROM SecurityItems " +
                   $"WHERE SecurityItemTypeId = {itemType.Id} AND " +
                   $"SubjectId = {subject.Id} AND " +
                   $"ContextId = {context.Id} AND " +
                   $"TargetId = {target.Id} AND " +
                   $"SecurityItemStatus <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<SecurityItemData>(op);
    }


    static internal T TryGetSubjectItemWithId<T>(SecurityContext context,
                                                 SecurityItemType itemType,
                                                 int subjectId) where T : SecurityItem {
      Assertion.Require(context, "context");
      Assertion.Require(itemType, "itemType");
      Assertion.Require(subjectId != -1, "subjectId");

      string sql = $"SELECT * FROM SecurityItems " +
                   $"WHERE ContextId = {context.Id} AND " +
                   $"SecurityItemTypeId = {itemType.Id} AND " +
                   $"SubjectId = {subjectId} AND " +
                   $"SecurityItemStatus <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetObject<T>(op, null);
    }


    static internal T TryGetSubjectItemWithKey<T>(SecurityContext context,
                                                  SecurityItemType itemType,
                                                  string securityItemKey) where T : SecurityItem {
      Assertion.Require(context, "context");
      Assertion.Require(itemType, "itemType");
      Assertion.Require(securityItemKey, "securityItemKey");

      string sql = $"SELECT * FROM SecurityItems " +
                   $"WHERE ContextId = {context.Id} AND " +
                   $"SecurityItemTypeId = {itemType.Id} AND " +
                   $"SecurityItemKey = '{securityItemKey}' AND " +
                   $"SecurityItemStatus <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetObject<T>(op, null);
    }


  }  // class SecurityItemsDataReader

}  // namespace Empiria.Security.Data
