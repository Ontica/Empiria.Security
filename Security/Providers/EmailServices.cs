/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Management                          Component : Providers                             *
*  Assembly : Empiria.Security.dll                         Pattern   : Service provider                      *
*  Types    : EmailServices                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Sends email messages when something happens in the context of security-related operations.     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.IO;

using Empiria.Contacts;

using Empiria.Messaging.EMailDelivery;

namespace Empiria.Security.Providers {

  /// <summary>Sends email messages when something happens in the context of security-related operations.</summary>
  static internal class EmailServices {

    #region Methods

    static internal void SendPasswordChangedWarningEMail() {
      Person person = (Person) ExecutionServer.CurrentContact;

      var body = GetTemplate("YourPasswordWasChanged");

      body = ParseGeneralFields(body, person);

      var content = new EmailContent($"Su contraseña de acceso al sistema ha cambiado", body, true);

      SendEmail(content, person);
    }


    static internal void SendPasswordChangedWarningEMail(Contact contact,
                                                         string userID,
                                                         string newPassword) {
      var body = GetTemplate("YourPasswordWasChanged");

      body = ParseGeneralFields(body, contact, userID, newPassword);

      var content = new EmailContent($"Su contraseña de acceso al sistema ha cambiado", body, true);

      SendEmail(content, contact);
    }

    #endregion Methods

    #region Helpers

    static private string GetTemplate(string templateName) {
      string templatesPath = ConfigurationData.GetString("Templates.Path");

      string fileName = Path.Combine(templatesPath, $"template.email.{templateName}.html");

      return File.ReadAllText(fileName);
    }


    static private string ParseGeneralFields(string body, Contact contact) {
      body = body.Replace("{{TO-NAME}}", contact.ShortName);

      return body;
    }


    static private string ParseGeneralFields(string body, Contact contact,
                                             string userID, string newPassword) {
      body = body.Replace("{{TO_NAME}}", contact.ShortName);
      body = body.Replace("{{USER_ID}}", userID);
      body = body.Replace("{{PASSWORD}}", newPassword);

      return body;
    }

    static private void SendEmail(EmailContent content, Contact sendToPerson) {
      var sendTo = new SendTo(sendToPerson.EMail, sendToPerson.ShortName);

      var sender = new EmailSender();

      sender.Send(sendTo, content);
    }


    #endregion Helpers

  }  // class EMailServices

}  // namespace Empiria.Security.Providers
