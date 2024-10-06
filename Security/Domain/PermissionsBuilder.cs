/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization services                *
*  Assembly : Empiria.Security.dll                         Pattern   : Service provider                      *
*  Type     : PermissionsBuilder                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Builds a string permissions array for a given identity.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.StateEnums;

namespace Empiria.Security {

  /// <summary>Builds a string permissions array for a given identity.</summary>
  internal class PermissionsBuilder {

    private readonly EmpiriaIdentity _subject;
    private readonly SecurityContext _context;

    internal PermissionsBuilder(EmpiriaIdentity subject, SecurityContext context) {
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(context, nameof(context));

      _subject = subject;
      _context = context;
    }

    #region Methods

    internal FixedList<Feature> BuildFeatures() {
      var features = new List<Feature>(64);

      FillIdentityFeatures(features);

      FixedList<Role> identityRoles = BuildRoles();

      FillRoleGrantedFeatures(features, identityRoles);

      RemoveRoleRevokedFeatures(features, identityRoles);

      return features.Distinct()
                     .ToFixedList()
                     .FindAll(x => x.Status == EntityStatus.Active);
    }


    internal FixedList<ObjectAccessRule> BuildObjectAccessRules() {

      FixedList<Feature> features = BuildFeatures();

      var objectRules = new List<ObjectAccessRule>(64);

      foreach (var feature in features) {
        objectRules.AddRange(feature.ObjectsGrants);
      }

      var roles = BuildRoles();

      foreach (var role in roles) {
        objectRules.AddRange(role.ObjectsGrants);
      }

      return objectRules.Distinct()
                        .ToFixedList();
    }


    internal FixedList<Role> BuildRoles() {
      return Role.GetSubjectRoles(_subject.User.Contact, _context);
    }

    #endregion Methods

    #region Helpers

    private void FillIdentityFeatures(List<Feature> list) {

      FixedList<Feature> identityFeatures = Feature.GetSubjectFeatures(_subject.User.Contact, _context);

      list.AddRange(identityFeatures);

      foreach (var feature in identityFeatures) {
        list.AddRange(feature.Requires);

        foreach (var require in feature.Requires) {
          list.AddRange(require.Requires);

          foreach (var item in require.Requires) {
            list.AddRange(item.Requires);
          }
        }
      }
    }


    private void FillRoleGrantedFeatures(List<Feature> list,
                                         FixedList<Role> roles) {
      foreach (var role in roles) {
        list.AddRange(role.Grants);

        foreach (var grant in role.Grants) {
          list.AddRange(grant.Requires);

          foreach (var require in grant.Requires) {
            list.AddRange(require.Requires);

            foreach (var item in require.Requires) {
              list.AddRange(item.Requires);
            }
          }
        }
      }
    }


    private void RemoveRoleRevokedFeatures(List<Feature> list,
                                           FixedList<Role> roles) {
      foreach (var role in roles) {
        foreach (var revoke in role.Revokes) {
          list.Remove(revoke);
        }
      }
    }

    #endregion Helpers

  }  // class PermissionsBuilder

}  // namespace Empiria.Security
