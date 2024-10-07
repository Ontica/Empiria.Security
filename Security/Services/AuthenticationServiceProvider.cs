/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Services                              *
*  Assembly : Empiria.Security.dll                         Pattern   : Service provider                      *
*  Type     : AuthenticationServiceProvider                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides user authentication services.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

using Empiria.Contacts;

using Empiria.Security.Providers;

using Empiria.Security.Data;
using Empiria.Security.Subjects;

namespace Empiria.Security.Services {

  /// <summary>Provides user authentication services.</summary>
  internal class AuthenticationServiceProvider: IAuthenticationProvider {

    #region Services

    public IEmpiriaPrincipal Authenticate(string sessionToken, string userHostAddress) {
      Assertion.Require(sessionToken, nameof(sessionToken));
      Assertion.Require(userHostAddress, nameof(userHostAddress));

      EmpiriaPrincipal principal = EmpiriaPrincipal.TryParseWithToken(sessionToken);

      if (principal != null) {
        return principal;
      }

      IEmpiriaSession session = EmpiriaSession.ParseActive(sessionToken);

      if (SecurityParameters.EnsureSimilarUserHostAddress &&
          session.UserHostAddress != userHostAddress) {

        var exception = new SecurityException(SecurityException.Msg.InvalidUserHostAddress,
                                              userHostAddress);

        EmpiriaLog.FailedOperationLog(session, "Authentication", exception.Message);

        throw exception;
      }


      var userData = Claim.TryParse(SecurityItemType.SubjectCredentials, session.UserId);

      if (userData == null) {
        throw new SecurityException(SecurityException.Msg.EnsureClaimFailed,
          "No se encontró la cuenta de acceso al sistema. Posiblemente dicha cuenta ya fue eliminada.");
      }

      IEmpiriaUser user = EmpiriaUser.Authenticate(userData);

      var identity = new EmpiriaIdentity(user, AuthenticationMode.Realm);

      var clientApplication = ClientApplication.Parse(session.ClientAppId);

      return new EmpiriaPrincipal(identity, clientApplication, session);
    }


    public IEmpiriaPrincipal Authenticate(IUserCredentials credentials) {
      Assertion.Require(credentials, nameof(credentials));

      Claim userData = GetSubjectAuthenticationClaim(credentials.UserID,
                                                     credentials.Password,
                                                     credentials.Entropy);

      IEmpiriaUser user = EmpiriaUser.Authenticate(userData);

      var identity = new EmpiriaIdentity(user, AuthenticationMode.Basic);

      IClientApplication clientApplication = AuthenticateClientApp(credentials.AppKey);

      CloseActiveUserSessions(user);

      MarkCredentialsAsExpiredIfNeeded(userData);

      return new EmpiriaPrincipal(identity, clientApplication, credentials);
    }


    public IClientApplication AuthenticateClientApp(string clientAppKey) {
      Assertion.Require(clientAppKey, clientAppKey);

      ClientApplication application = ClientApplication.TryParse(clientAppKey);

      if (application == null) {

        var exception = new SecurityException(SecurityException.Msg.InvalidClientAppKey, clientAppKey);

        EmpiriaLog.FailedOperationLog("AuthenticateClientApp", exception);

        throw exception;
      }

      if (application.Status != EntityStatus.Active) {
        var exception = new SecurityException(SecurityException.Msg.ClientAppKeyIsSuspended, clientAppKey);

        EmpiriaLog.FailedOperationLog("AuthenticateClientApp", exception);

        throw exception;
      }

      return application;
    }


    public Claim GetSubjectAuthenticationClaim(string userID, string password, string entropy) {
      Assertion.Require(userID, nameof(userID));
      Assertion.Require(password, nameof(password));
      Assertion.Require(entropy, nameof(entropy));

      var claim = Claim.TryParseWithKey(SecurityItemType.SubjectCredentials, userID);

      // User not found
      if (claim == null) {

        EmpiriaLog.FailedOperationLog("Authentication",
                                      $"Se intentó ingresar al sistema con una cuenta que no existe: '{userID}'");

        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }

      var contact = Contact.Parse(claim.SubjectId);

      if (claim.Status == EntityStatus.Suspended) {

        var exception = new SecurityException(SecurityException.Msg.UserAccountIsSuspended, userID);

        EmpiriaLog.FailedOperationLog(contact, "Authentication", exception.Message);

        throw exception;
      }

      var storedPassword = claim.GetAttribute<string>(ClaimAttributeNames.Password);

      string decryptedPassword = DecryptPassword(userID, storedPassword, entropy);

      if (decryptedPassword == password) {

        AuthenticationAttemptsRegister.Remove(userID);

        return claim;
      }

      //Invalid password

      AuthenticationAttemptsRegister.Add(userID);

      if (AuthenticationAttemptsRegister.MaxAttemptsReached(userID)) {
        throw SuspendUserAccount(claim);
      } else {


        EmpiriaLog.FailedOperationLog(contact, "Authentication", "La contraseña proporcionada no es correcta.");

        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }
    }

    #endregion Services

    #region Helpers

    private void CloseActiveUserSessions(IEmpiriaUser user) {
      if (!ConfigurationData.Get("OneActiveSessionPerUser", false)) {
        return;
      }
      EmpiriaPrincipal.CloseAllSessions(user.Contact);
    }


    private string DecryptPassword(string userID, string storedPassword, string entropy) {

      string decrypted = Cryptographer.Decrypt(storedPassword, userID);

      decrypted = Cryptographer.GetSHA256(decrypted + entropy);

      return decrypted;
    }


    private void MarkCredentialsAsExpiredIfNeeded(Claim userData) {
      if (userData.HasAttribute(ClaimAttributeNames.MustChangePassword) ||
          userData.HasAttribute(ClaimAttributeNames.PasswordUpdatedDate) ||
          userData.HasAttribute(ClaimAttributeNames.PasswordNeverExpires)) {
        return;
      }

      var editor = new SubjectSecurityItemsEditor(Contact.Parse(userData.SubjectId));

      editor.MarkCredentialsAsExpired();
    }


    private SecurityException SuspendUserAccount(Claim claim) {
      var contact = Contact.Parse(claim.SubjectId);

      var editor = new SubjectSecurityItemsEditor(contact);

      if (editor.GetCredentialsStatus() == EntityStatus.Active) {
        editor.SuspendSubjectCredentials();
      }

      var exception = new SecurityException(SecurityException.Msg.UserAccountHasBeenBlocked);

      EmpiriaLog.UserManagementLog(contact, "Authentication", exception.Message);
      EmpiriaLog.FailedOperationLog(contact, "Authentication", exception.Message);

      return exception;
    }

    #endregion Helpers

  }  // class AuthenticationServiceProvider

}  // namespace Empiria.Security.Services
