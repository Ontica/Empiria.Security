/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization services                *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : Role                                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an identity role that holds feature access permissions.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Security.Data;

namespace Empiria.Security {

  /// <summary>Represents an identity role that holds feature access permissions.</summary>
  internal class Role : SecurityItem, INamedEntity {

    #region Constructors and parsers

    internal Role(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }


    static internal new Role Parse(int id) {
      return BaseObject.ParseId<Role>(id);
    }


    static internal Role Parse(string uid) {
      return BaseObject.ParseKey<Role>(uid);
    }


    static internal FixedList<Role> GetList(SoftwareSystem softwareSystem) {

      return SecurityItemsDataReader.GetSoftwareSystemItems<Role>(softwareSystem,
                                                                  SecurityItemType.SoftwareSystemRole);
    }


    static internal FixedList<Role> GetSubjectRoles(IIdentifiable subject,
                                                    SecurityContext context) {

      return SecurityItemsDataReader.GetSubjectTargetItems<Role>(subject, context,
                                                                 SecurityItemType.SubjectRole);
    }


    static internal bool IsSubjectInRole(IIdentifiable subject,
                                         SecurityContext context,
                                         string roleKey) {

      FixedList<Role> subjectRoles = GetSubjectRoles(subject, context);

      return subjectRoles.Contains(x => x.Key == roleKey);
    }

    #endregion Constructors and parsers

    public string Key {
      get {
        return base.BaseKey;
      }
    }


    public string Name {
      get {
        return ExtensionData.Get("roleName", base.BaseKey);
      }
    }


    Feature[] _grants;

    internal Feature[] Grants {
      get {
        if (_grants == null) {
          _grants = ExtensionData.GetList<Feature>("grants", false)
                                 .ToArray();
        }

        return _grants;
      }
    }

    Feature[] _revokes;

    internal Feature[] Revokes {
      get {
        if (_revokes == null) {
          _revokes = ExtensionData.GetList<Feature>("revokes", false)
                                  .ToArray();
        }

        return _revokes;
      }
    }

    ObjectAccessRule[] _objectsGrants;

    public ObjectAccessRule[] ObjectsGrants {
      get {
        if (_objectsGrants == null) {
          _objectsGrants = ExtensionData.GetList<ObjectAccessRule>("objectsGrants", false)
                                        .ToArray();

        }

        return _objectsGrants;
      }
    }

  }  // class Role

}  // namespace Empiria.Security
