/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization services                *
*  Assembly : Empiria.Security.dll                         Pattern   : Information holder                    *
*  Type     : SecurityContext                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds information about a security context.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Security.Data;

namespace Empiria.Security {

  /// <summary>Holds information about a security context.</summary>
  internal class SecurityContext : SecurityItem, INamedEntity {

    #region Constructors and parsers

    protected SecurityContext(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }


    static internal new SecurityContext Parse(int id) {
      return BaseObject.ParseId<SecurityContext>(id);
    }


    static internal SecurityContext Parse(string uid) {
      return BaseObject.ParseKey<SecurityContext>(uid);
    }


    static internal SecurityContext ParseWith(IClientApplication clientApp) {
      return ((ClientApplication) clientApp).SecurityContext;
    }


    static internal FixedList<SecurityContext> GetList() {
      var list = SecurityItemsDataReader.GetSecurityItems<SecurityContext>(SecurityItemType.SecurityContext);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }


    static internal FixedList<SecurityContext> GetList(IIdentifiable subject) {
      return SecurityItemsDataReader.GetSubjectTargetItems<SecurityContext>(subject, SecurityContext.Empty,
                                                                            SecurityItemType.SubjectContext);
    }


    static internal new SecurityContext Empty => BaseObject.ParseEmpty<SecurityContext>();

    #endregion Constructors and parsers

    #region Properties

    public string Key {
      get {
        return base.BaseKey;
      }
    }


    public string Name {
      get {
        return ExtensionData.Get("contextName", this.Key);
      }
    }


    internal SoftwareSystem SoftwareSystem {
      get {
        return SoftwareSystem.Parse(base.TargetId);
      }
    }

    #endregion Properties

  }  // class SecurityContext

}  // namespace Empiria.Security
