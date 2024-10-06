/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Domain Layer                          *
*  Assembly : Empiria.Security.dll                         Pattern   : Service provider                      *
*  Type     : PasswordStrength                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides services to verify password strength.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Security.Subjects {

  /// <summary>Provides services to verify password strength.</summary>
  public class PasswordStrength {

    #region Fields

    private readonly EmpiriaUser _user;
    private readonly string _password;

    #endregion Fields

    #region Constructors and parsers

    private PasswordStrength(string password) {
      _password = password;
    }


    internal PasswordStrength(EmpiriaUser user, string password) {
      Assertion.Require(user, nameof(user));
      Assertion.Require(password, nameof(password));

      _user = user;
      _password = password;
    }


    static public void AssertIsValid(string password) {
      Assertion.Require(password, nameof(password));

      var instance = new PasswordStrength(password);

      instance.VerifyLength();
      instance.VerifyCharactersCombination();
    }

    #endregion Constructors and parsers

    #region Methods

    internal void VerifyStrength() {
      VerifyLength();
      VerifyCharactersCombination();
      VerifyNoUserDataPortions();
    }

    #endregion Methods

    #region Helpers

    private void VerifyCharactersCombination() {
      int counter = 0;

      if (EmpiriaString.ContainsAnyChar(_password, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")) {
        counter++;
      }
      if (EmpiriaString.ContainsAnyChar(_password, "abcdefghijklmnopqrstuvwxyz")) {
        counter++;
      }
      if (EmpiriaString.ContainsAnyChar(_password, "0123456789")) {
        counter++;
      }
      if (EmpiriaString.ContainsAnyChar(_password, @"~¡!@#$€£%^*&¿?-+='_""(){}[]\/|ñÑ<>.,:;")) {
        counter++;
      }
      Validate.IsTrue(counter >= 3, "Passwords must contain characters from " +
                                    "at least three of the following four categories " +
                                    "arranged in any order:\n" +
                                    "English uppercase characters(A through Z)\n" +
                                    "English lowercase characters(A through Z)\n" +
                                    "Base 10 digits (0 through 9)\n" +
                                    "Non-alphabetic characters like: ~!@#$%^*&;?.+_");
    }


    private void VerifyLength() {
      Validate.IsTrue(_password.Length >= 8,
                      "Passwords must be at least eight (8) characters in length.");
    }


    private void VerifyNoUserDataPortions() {
      if (EmpiriaString.ContainsSegment(_password, _user.UserName, 3) ||
          EmpiriaString.ContainsSegment(_password, _user.FullName, 3) ||
          EmpiriaString.ContainsSegment(_password, _user.EMail, 3)) {

        Validate.Fail("Passwords must not contain significant portions " +
                      "(three or more contiguous characters) of the user name, " +
                      "first name, last name, or email address.");
      }
    }


    #endregion Helpers

  }  // class PasswordStrength

}  // namespace Empiria.Security.Subjects
