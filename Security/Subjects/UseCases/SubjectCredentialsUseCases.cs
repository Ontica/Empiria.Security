/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Subjects Management                 Component : Use cases Layer                       *
*  Assembly : Empiria.Security.dll                         Pattern   : Use case interactor                   *
*  Type     : SubjectCredentialsUseCases                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Use cases for update user's credentials.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Contacts;

using Empiria.Services;
using Empiria.StateEnums;

using Empiria.Security.Data;

using Empiria.Security.Providers;
using Empiria.Security.Subjects.Adapters;

namespace Empiria.Security.Subjects.UseCases {

  /// <summary>Use cases for update user's credentials.</summary>
  public class SubjectCredentialsUseCases : UseCase {

    #region Constructors and parsers

    static public SubjectCredentialsUseCases UseCaseInteractor() {
      return CreateInstance<SubjectCredentialsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public string GenerateNewCredentialsToken(UserCredentialsDto credentials) {
      Assertion.Require(credentials, nameof(credentials));

      return SecurityTokenGenerator.GenerateToken(credentials, SecurityTokenType.UpdateCredentials);
    }


    public void ResetCredentials(string contactUID) {
      Assertion.Require(contactUID, nameof(contactUID));

      var contact = Contact.Parse(contactUID);

      SubjectData subject = SubjectsDataService.GetSubject(contact);

      string newPassword = GeneratePassword();

      var editor = new SubjectSecurityItemsEditor(contact);

      editor.UpdateSubjectCredentials(EncryptPassword(subject.UserID, newPassword), true);

      EmpiriaLog.UserManagementLog(subject.Contact, "Se reseteó la contraseña de acceso al sistema");

      EmpiriaPrincipal.CloseAllSessions(contact);

      if (SecurityParameters.SendPasswordsUsingEmail) {
        EmailServices.SendPasswordChangedWarningEMail(contact, subject.UserID, newPassword);

      } else {

        throw new ServiceException("El servidor de correo no está configurado.",
            $"La contraseña asignada a '{subject.UserID}' no pudo enviarse por correo electrónico." +
            $"Sin embargo, esta es la contraseña que le fue asignada: {newPassword}");
      }
    }


    public void UpdateCredentials(UpdateCredentialsFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var service = new Services.AuthenticationService();

      service.AuthenticateClientApp(fields.AppKey);

      var entropy = SecurityTokenGenerator.PopToken(fields, SecurityTokenType.UpdateCredentials);

      Claim claim = service.GetSubjectAuthenticationClaim(fields.UserID, fields.CurrentPassword, entropy);

      var contact = Contact.Parse(claim.SubjectId);

      var editor = new SubjectSecurityItemsEditor(contact);

      Assertion.Require(editor.GetCredentialsStatus() == EntityStatus.Active,
                        "La cuenta de acceso al sistema ha sido bloqueada (suspendida). Por motivos de seguridad, " +
                        "no es posible actualizar la contraseña.");

      string newPassword = EncryptPassword(fields.UserID, Cryptographer.Decrypt(fields.NewPassword, entropy, true));

      Assertion.Require(claim.GetAttribute<string>(ClaimAttributeNames.Password) != newPassword,
                        "La nueva contraseña debe ser distinta a la contraseña actual.");

      editor.UpdateSubjectCredentials(newPassword, false);

      EmpiriaLog.UserManagementLog(contact, "La persona usuaria modificó su propia contraseña de acceso al sistema");

      if (SecurityParameters.SendPasswordsUsingEmail) {
        EmailServices.SendPasswordChangedWarningEMail();
      }
    }

    #endregion Use cases

    #region Helpers

    private string EncryptPassword(string userID, string password) {

      return Cryptographer.Encrypt(EncryptionMode.EntropyKey,
                                   Cryptographer.GetSHA256(password), userID);
    }


    private string GeneratePassword() {
      var list = SecurityParameters.PasswordRandomNames;

      int position = EmpiriaMath.GetRandom(0, list.Count - 1);

      return $"{list[position]}@{EmpiriaMath.GetRandom(1000, 99999)}";
    }

    #endregion Helpers

  }  // class SubjectCredentialsUseCases

}  // namespace Empiria.Security.Subjects.UseCases
